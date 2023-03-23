using System.Data;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents
{
    internal class MsSqlDocumentCategory : IDocumentCategory
    {
        #region Properties
        public Guid Id { get; }
        public bool IsDesigned { get; }
        #endregion

        #region Fields
        private readonly IDataSource _msSqlDataSource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDictionary<string, string> _sqlTableNames;
        #endregion

        #region ctor
        public MsSqlDocumentCategory(
            Guid id,
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames,
            bool isDesigned)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException($"Argument {nameof(id)} could not be an empty Guid.");
            }

            Id = id;

            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
            IsDesigned = isDesigned;
        }

        public MsSqlDocumentCategory(
            Guid id,
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException($"Argument {nameof(id)} could not be an empty Guid.");
            }

            Id = id;

            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
            IsDesigned = false;
        }
        #endregion

        #region IDocumentCategory members
        public async void SetDocumentCategoryEntityAsync(Guid entityId)
        {
            var parameters = _dataParameterFactory
                                .ConfigureParameter("@DocumentCategoryId", SqlDbType.UniqueIdentifier, Id)
                                .ConfigureParameter("@DocumentMetadataDefinitionId", SqlDbType.UniqueIdentifier, entityId)
                                .GetConfiguredParameters();

            _ = await _msSqlDataSource.PerformCommandAsync(
                $" INSERT INTO {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORY_ENTITIES_TABLE_NAME_KEY]}" +
                $" (DocumentCategoryId, DocumentMetadataDefinitionId)" +
                $" VALUES (@DocumentCategoryId, @DocumentMetadataDefinitionId)", parameters);
        }
        #endregion

        #region Private members
        public async Task<string> GetDisplayNameAsync()
        {
            if (!IsNecessaryTablesExists())
            {
                return string.Empty;
            }

            var parameters = _dataParameterFactory
                                .ConfigureParameter("@CategoryId", SqlDbType.UniqueIdentifier, Id)
                                .GetConfiguredParameters();

            var documentCategoryEntities = await _msSqlDataSource.PerformQueryAsync(
                $"   SELECT Id, DisplayName FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORIES_TABLE_NAME_KEY]}"
                + $" WHERE Id = @CategoryId",
                parameters, "DisplayName");

            var displayName = documentCategoryEntities.ResponseObject?.FirstOrDefault()?.Attributes["DisplayName"]?.ToString();
            return displayName ?? string.Empty;
        }

        public async Task<IDictionary<string, string>> GetAttributesAsync()
        {
            var result = new Dictionary<string, string>();

            if (!IsNecessaryTablesExists())
            {
                return result;
            }

            var parameters = _dataParameterFactory
                                .ConfigureParameter("@CategoryId", SqlDbType.UniqueIdentifier, Id)
                                .GetConfiguredParameters();

            var documentCategoryEntities = await _msSqlDataSource.PerformQueryAsync(
                $"   SELECT dce.Id, dmd.ExtractedName FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORY_ENTITIES_TABLE_NAME_KEY]} dce"
                + $" INNER JOIN {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY]} dmd"
                + $"   ON dmd.Id = dce.DocumentMetadataDefinitionId"
                + $" WHERE dce.DocumentCategoryId = @CategoryId",
                parameters, "ExtractedName");

            if (documentCategoryEntities != null)
            {
                foreach (var documentCategoryEntity in documentCategoryEntities.ResponseObject)
                {
                    var extractedName = documentCategoryEntity?.Attributes["ExtractedName"]?.ToString();
                    result.Add(extractedName ?? Guid.NewGuid().ToString(), extractedName ?? string.Empty);
                }
            }

            return result;
        }

        private bool IsNecessaryTablesExists()
        {
            var isDocumentCategoriesTableExists = _sqlTableNames.TryGetValue(Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORIES_TABLE_NAME_KEY, out _);
            var isDocumentCategoryEntitiesTableExists = _sqlTableNames.TryGetValue(Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORY_ENTITIES_TABLE_NAME_KEY, out _);

            if (!isDocumentCategoriesTableExists)
            {
                // TODO: feedback
                return false;
            }

            if (!isDocumentCategoryEntitiesTableExists)
            {
                // TODO: feedback
                return false;
            }

            return true;
        }
        #endregion
    }
}
