using System.Data;
using XperiCad.Common.Core.Behaviours.Queries;
using XperiCad.Common.Core.DataSource;
using XperiCad.Common.Core.Exceptions;
using XperiCad.Common.Infrastructure.Behaviours.Queries;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.Common.Infrastructure.Feedback;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories;
using XperiCad.DigitalDrawingStore.BL.Impl.Extensions;
using Constants = XperiCad.DigitalDrawingStore.BL.Impl.Constants;
using i18n = XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n;

namespace XperiCad.DigitalDrawingStore.BL.Documents.Queries
{
    internal class MsSqlDocumentQuery : AQuery, IDocumentQuery
    {
        #region Constants
        private const int MAX_DOCUMENTS_FROM_QUERY = 200;
        #endregion

        #region Fields
        private readonly IDataSource _msSqlDataSource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDictionary<string, string> _sqlTableNames;
        private readonly IDocumentFactory _documentFactory;
        private readonly IFeedbackMessageFactory _feedbackMessageFactory;
        #endregion

        #region Constructors
        public MsSqlDocumentQuery(
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames,
            IDocumentFactory documentFactory,
            IFeedbackMessageFactory feedbackMessageFactory)
        {
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
            _documentFactory = documentFactory ?? throw new ArgumentNullException(nameof(documentFactory));
            _feedbackMessageFactory = feedbackMessageFactory ?? throw new ArgumentNullException(nameof(feedbackMessageFactory));
        }
        #endregion

        #region IDocumentQuery members
        public Task<IPromise<IEnumerable<IDocument>>> QueryDocumentsAsync(string searchText)
        {
            return QueryDocumentsAsync(Guid.Empty, searchText);
        }

        public async Task<IPromise<IEnumerable<IDocument>>> QueryDocumentsAsync(Guid categoryId, string searchText)
        {
            var feedbackQueue = GetFeedbackQueue();

            var documentEntities = await PerformQuery(feedbackQueue, categoryId, searchText);

            var documents = ConvertEntitiesToDocuments(feedbackQueue, documentEntities);
            return ResolvePromise(documents, feedbackQueue);
        }

        public async Task<IPromise<IDictionary<string, string>>> QueryDocumentMetadataAsync(Guid documentId)
        {
            var feedbackQueue = GetFeedbackQueue();

            //TODO: use this feedback message if a table was not found and reject the promise
            //feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_NoSuchTableFoundInDatabase, "TODO: tableName"));

            var parameters = _dataParameterFactory
                                .ConfigureParameter("@DocumentId", SqlDbType.UniqueIdentifier, documentId)
                                .GetConfiguredParameters();

            var documentMetadataResult = await _msSqlDataSource.PerformQueryAsync(
                $"   SELECT dm.Id, Value, dmd.ExtractedName FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_TABLE_NAME_KEY]} dm"
                + $" INNER JOIN {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENTS_TABLE_NAME_KEY]} d"
                + $"   ON d.Id = dm.DocumentId"
                + $" INNER JOIN {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY]} dmd"
                + $"   ON dm.DocumentMetadataDefinitionId = dmd.Id"
                + $" WHERE d.Id = @DocumentId",
                parameters, "ExtractedName", "Value");

            var result = ConvertToDictionary(feedbackQueue, documentMetadataResult.ResponseObject);
            return ResolvePromise(result, feedbackQueue);
        }

        public async Task<IPromise<IDictionary<Guid, string>>> QueryAllTargetOfDocumentUsageAsync()
        {
            var feedbackQueue = GetFeedbackQueue();

            // TODO: conscheck for tables
            //TODO: use this feedback message if a table was not found and reject the promise
            //feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_NoSuchTableFoundInDatabase, "TODO: tableName"));

            var sqlScript = $" SELECT Id, PropertyValue"
                          + $" FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.APPLICATION_PROPERTIES_DICTIONARY_TABLE_NAME_KEY]} apd"
                          + $" WHERE apd.ApplicationPropertiesId = (SELECT Id FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.APPLICATION_PROPERTIES_TABLE_NAME_KEY]} WHERE PropertyKey = 'TargetOfUsage')";

            var responseEntities = await _msSqlDataSource.PerformQueryAsync(sqlScript, "PropertyValue");

            IDictionary<Guid, string> result = new Dictionary<Guid, string>();

            if (responseEntities != null)
            {
                foreach (var responseEntity in responseEntities.ResponseObject)
                {
                    var propertyValue = responseEntity.Attributes["PropertyValue"].ToString() ?? string.Empty;

                    if (!string.IsNullOrEmpty(propertyValue))
                    {
                        result.Add(responseEntity.Id, propertyValue);
                    }
                    else
                    {
                        feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Warning_TargetOfDocumentUsageCouldNotBeProcessed));
                    }
                }
            }
            else
            {
                feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_NoTargetOfDocumentUsageFoundInDatabase));
                return RejectPromise<IDictionary<Guid, string>>(feedbackQueue);
            }
            result = result.OrderByValue();

            return ResolvePromise(result, feedbackQueue);
        }

        public async Task<IPromise<IDocument>> GetDocumentByIdAsync(Guid documentId)
        {
            var feedbackQueue = GetFeedbackQueue();

            // TODO: conscheck for tables
            var table = _sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENTS_TABLE_NAME_KEY];
            //var testScript = $"SELECT CASE WHEN OBJECT_ID('dbo.{table}', 'U') IS NOT NULL THEN 1 ELSE 0 END";
            //var testRes = await _msSqlDataSource.PerformQuery(testScript);

            //TODO: use this feedback message if a table was not found and reject the promise
            //feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_NoSuchTableFoundInDatabase, "TODO: tableName"));

            var sqlScript =
                            $" SELECT Id, Path"
                          + $" FROM {table} d"
                          + $" WHERE d.Id = @DocumentId";

            _dataParameterFactory
                .ConfigureParameter("@DocumentId", SqlDbType.UniqueIdentifier, documentId);

            var parameters = _dataParameterFactory.GetConfiguredParameters();

            var result = await _msSqlDataSource.PerformQueryAsync(sqlScript, parameters, "Path");

            var documentEntity = result.ResponseObject.FirstOrDefault();

            var id = documentEntity?.Id ?? default(Guid);
            var documentPath = documentEntity?.Attributes[Constants.Documents.AttributeKeys.DOCUMENT_PATH]?.ToString() ?? null;

            if (documentEntity == null || id == Guid.Empty || string.IsNullOrWhiteSpace(documentPath))
            {
                var error_NoSuchDocumentFoundInDatabase = _feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_NoSuchDocumentFoundInDatabase);
                return RejectPromise<IDocument>(error_NoSuchDocumentFoundInDatabase);
            }

            var document = _documentFactory.CreatePdfDocument(id, documentPath);
            return ResolvePromise(document, feedbackQueue);
        }
        #endregion

        #region Private members
        private async Task<IEnumerable<IEntity>> PerformQuery(ICollection<IFeedbackMessage> feedbackQueue, Guid categoryId, string searchText)
        {
            // TODO: Implement actual feedback messages
            // feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Test));
            // TODO: check if necessary tables are exists in feedbackQueue
            // TODO: avoid every exception with validations and send feedback if something is bad

            //TODO: use this feedback message if a table was not found and reject the promise
            //feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_NoSuchTableFoundInDatabase, "TODO: tableName"));

            var sqlScript = $"SELECT TOP {MAX_DOCUMENTS_FROM_QUERY} Id, Path"
                          + $" FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENTS_TABLE_NAME_KEY]} d";

            var normalizedSearchText = searchText?.Replace(".", "") ?? string.Empty;

            if (categoryId == Guid.Empty)
            {
                if (!(string.IsNullOrWhiteSpace(searchText) || string.Equals(searchText, "*", StringComparison.OrdinalIgnoreCase)))
                {
                    _dataParameterFactory
                        .ConfigureParameter("@SearchText", SqlDbType.VarChar, searchText, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                        .ConfigureParameter("@NormalizedSearchText", SqlDbType.VarChar, normalizedSearchText, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH);

                    sqlScript += $" WHERE d.Path LIKE CONCAT('%', @SearchText, '%')"
                               + $"   OR d.Id IN ("
                               + $"     SELECT DocumentId FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_TABLE_NAME_KEY]} dm"
                               + $"     WHERE dm.DocumentId = d.Id"
                               + $"       AND"
                               + $"       ("
                               + $"         dm.Value LIKE CONCAT('%', @SearchText, '%')"
                               + $"         OR REPLACE(CONVERT(VARCHAR(MAX), dm.value), '.', '') LIKE CONCAT('%', @NormalizedSearchText, '%')"
                               + $"       )"
                               + $"   )"
                               + $"   OR d.DocumentCategoryId IN ("
                               + $"     SELECT Id"
                               + $"     FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORIES_TABLE_NAME_KEY]} dc"
                               + $"     WHERE dc.Id = d.DocumentCategoryId"
                               + $"       AND"
                               + $"       ("
                               + $"         dc.DisplayName LIKE CONCAT('%', @SearchText, '%')"
                               + $"         OR REPLACE(CONVERT(VARCHAR(MAX), dc.DisplayName), '.', '') LIKE CONCAT('%', @NormalizedSearchText, '%')"
                               + $"       )"
                               + $"   )";
                }
            }
            else
            {
                _dataParameterFactory.ConfigureParameter("@DocumentCategoryId", SqlDbType.UniqueIdentifier, categoryId);

                if (!(string.IsNullOrWhiteSpace(searchText) || string.Equals(searchText, "*", StringComparison.OrdinalIgnoreCase)))
                {
                    _dataParameterFactory
                        .ConfigureParameter("@SearchText", SqlDbType.VarChar, searchText, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                        .ConfigureParameter("@NormalizedSearchText", SqlDbType.VarChar, normalizedSearchText, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH);

                    var test = _sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORIES_TABLE_NAME_KEY];

                    sqlScript += $" WHERE d.Path LIKE CONCAT('%', @SearchText, '%')"
                               + $"   AND d.DocumentCategoryId = @DocumentCategoryId"
                               + $"   OR d.Id IN ("
                               + $"     SELECT DocumentId FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_TABLE_NAME_KEY]} dm"
                               + $"     WHERE dm.DocumentId = d.Id"
                               + $"       AND"
                               + $"       ("
                               + $"         dm.Value LIKE CONCAT('%', @SearchText, '%')"
                               + $"         OR REPLACE(CONVERT(VARCHAR(MAX), dm.value), '.', '') LIKE CONCAT('%', @NormalizedSearchText, '%')"
                               + $"       )"
                               + $"       AND d.DocumentCategoryId = @DocumentCategoryId"
                               + $"   )"
                               + $"   OR d.DocumentCategoryId IN("
                               + $"     SELECT Id FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORIES_TABLE_NAME_KEY]} dc"
                               + $"     WHERE dc.Id = d.DocumentCategoryId"
                               + $"       AND"
                               + $"       ("
                               + $"         dc.DisplayName LIKE CONCAT('%', @SearchText, '%')"
                               + $"         OR REPLACE(CONVERT(VARCHAR(MAX), dc.DisplayName), '.', '') LIKE CONCAT('%', @NormalizedSearchText, '%')"
                               + $"       )"
                               + $"       AND d.DocumentCategoryId = @DocumentCategoryId"
                               + $"   )";
                }
                else
                {
                    sqlScript += $" WHERE d.DocumentCategoryId = @DocumentCategoryId";
                }

            }

            var parameters = _dataParameterFactory.GetConfiguredParameters();

            var result = await _msSqlDataSource.PerformQueryAsync(sqlScript, parameters, "Id", "Path");

            return result.ResponseObject;
        }

        private IEnumerable<IDocument> ConvertEntitiesToDocuments(ICollection<IFeedbackMessage> feedbackQueue, IEnumerable<IEntity> documentEntities)
        {
            var resultDocuments = new List<IDocument>();

            if (documentEntities == null)
            {
                return resultDocuments;
            }

            foreach (var documentEntity in documentEntities)
            {
                if (documentEntity != null && documentEntity.Attributes != null)
                {
                    var isSuccessfullyParsed = Guid.TryParse(documentEntity.Id.ToString(), out Guid documentId);
                    var documentPath = documentEntity.Attributes["Path"].ToString();

                    if (isSuccessfullyParsed && documentPath != null)
                    {
                        resultDocuments.Add(_documentFactory.CreatePdfDocument(documentId, documentPath));
                    }
                    else
                    {
                        feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Warning_DocumentCouldNotBeProcessed));
                    }
                }
            }

            return resultDocuments;
        }

        private IDictionary<string, string> ConvertToDictionary(ICollection<IFeedbackMessage> feedbackQueue, IEnumerable<IEntity>? documentMetadataResult)
        {
            var result = new Dictionary<string, string>();

            if (documentMetadataResult != null && documentMetadataResult.Count() > 0)
            {
                foreach (var entity in documentMetadataResult)
                {
                    if (entity != null && entity.Attributes != null)
                    {
                        var extractedName = entity.Attributes["ExtractedName"].ToString();
                        var value = entity.Attributes["Value"].ToString();

                        if (!string.IsNullOrWhiteSpace(extractedName) && value != null)
                        {
                            result.Add(extractedName, value);
                        }
                        else
                        {
                            feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Warning_DocumentMetadataCouldNotBeProcessed));
                        }
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
