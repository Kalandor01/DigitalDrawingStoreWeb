using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using XperiCad.Common.Core.DataSource;
using XperiCad.Common.Infrastructure.DataSource;

namespace DigitalDrawingStore.Listener.Service.Document.Resources
{
    internal class MsSqlDocumentResource : IDocumentResource
    {
        #region Fields
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDataSource _msSqlDataSource;
        private readonly IDictionary<string, string> _sqlTableNames;
        #endregion

        #region Constructor
        public MsSqlDocumentResource(
            IDataParameterFactory dataParameterFactory,
            IDataSource msSqlDataSource,
            IDictionary<string, string> sqlTableNames)
        {
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
        }
        #endregion

        #region IDocumentResource members
        public void SaveDocuments(IEnumerable<IRawDocument> rawDocuments)
        {
            if (rawDocuments is null)
            {
                throw new ArgumentNullException(nameof(rawDocuments));
            }

            foreach (var rawDocument in rawDocuments)
            {
                var documentCategory = rawDocument.GetAttribute(Constants.RawDocumentAttributeNames.DOCUMENT_CATEGORY);
                var documentCategoryId = GetCategoryIdByName(documentCategory);
                if (documentCategoryId == Guid.Empty)
                {
                    InsertNewDocumentCategory(documentCategory);
                    documentCategoryId = GetCategoryIdByName(documentCategory);
                }

                var documentId = InsertNewDocument(documentCategoryId, rawDocument.DocumentData.DocumentPath);

                foreach (var metadata in rawDocument.GetAllAttributes())
                {
                    var metadataDefinitionId = GetMetadataDefinitionByName(metadata.Key);
                    if (metadataDefinitionId == Guid.Empty)
                    {
                        InsertMetadataDefinition(metadata.Key);
                        metadataDefinitionId = GetMetadataDefinitionByName(metadata.Key);
                    }

                    InsertMetadata(documentId, metadataDefinitionId, metadata.Value);
                }
            }
        }
        #endregion

        #region Private members
        private void InsertNewDocumentCategory(string categoryName)
        {
            var parameters = _dataParameterFactory
                                .ConfigureParameter("@Id", SqlDbType.UniqueIdentifier, Guid.NewGuid())
                                .ConfigureParameter("@IsDesigned", SqlDbType.Bit, 0)
                                .ConfigureParameter("@DisplayName", SqlDbType.VarChar, categoryName, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                                .GetConfiguredParameters();

            var sqlScript = $"INSERT INTO {_sqlTableNames[Constants.DocumentDatabase.DOCUMENT_CATEGORIES_TABLE_NAME_KEY]} (Id, IsDesigned, DisplayName)"
                        + $"VALUES (@Id, @IsDesigned, @DisplayName)";

            _msSqlDataSource.PerformCommand(sqlScript, parameters);
        }

        private Guid InsertNewDocument(Guid categoryId, string documentPath)
        {
            var documentId = Guid.NewGuid();

            var parameters = _dataParameterFactory
                                .ConfigureParameter("@Id", SqlDbType.UniqueIdentifier, documentId)
                                .ConfigureParameter("@DocumentCategoryId", SqlDbType.UniqueIdentifier, categoryId)
                                .ConfigureParameter("@Path", SqlDbType.VarChar, documentPath, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                                .GetConfiguredParameters();

            var sqlScript = $"INSERT INTO {_sqlTableNames[Constants.DocumentDatabase.DOCUMENTS_TABLE_NAME_KEY]} (Id, DocumentCategoryId, Path)"
                        + $"VALUES (@Id, @DocumentCategoryId, @Path)";

            _msSqlDataSource.PerformCommand(sqlScript, parameters);

            return documentId;
        }

        private Guid GetCategoryIdByName(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return Guid.Empty;
            }

            var parameters = _dataParameterFactory
                                .ConfigureParameter("@DisplayName", SqlDbType.VarChar, categoryName, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                                .GetConfiguredParameters();

            var sqlScript = $"SELECT Id"
                + $" FROM {_sqlTableNames[Constants.DocumentDatabase.DOCUMENT_CATEGORIES_TABLE_NAME_KEY]}"
                + $" WHERE DisplayName = @DisplayName";

            var categoriesResult = _msSqlDataSource.PerformQuery(sqlScript, parameters);
            var categoryId = categoriesResult.FirstOrDefault()?.Id ?? Guid.Empty;

            return categoryId;
        }

        private Guid GetMetadataDefinitionByName(string metadataName)
        {
            if (string.IsNullOrWhiteSpace(metadataName))
            {
                return Guid.Empty;
            }

            var parameters = _dataParameterFactory
                                .ConfigureParameter("@ExtractedName", SqlDbType.VarChar, metadataName, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                                .GetConfiguredParameters();

            var sqlScript = $"SELECT Id"
                + $" FROM {_sqlTableNames[Constants.DocumentDatabase.DOCUMENT_METADATA_DEFINITIONS_TABLE_NAME_KEY]}"
                + $" WHERE ExtractedName = @ExtractedName";

            var queryResult = _msSqlDataSource.PerformQuery(sqlScript, parameters);
            var categoryId = queryResult.FirstOrDefault()?.Id ?? Guid.Empty;

            return categoryId;
        }

        private void InsertMetadataDefinition(string metadataName)
        {
            var parameters = _dataParameterFactory
                                .ConfigureParameter("@Id", SqlDbType.UniqueIdentifier, Guid.NewGuid())
                                .ConfigureParameter("@ExtractedName", SqlDbType.VarChar, metadataName, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                                .GetConfiguredParameters();

            var sqlScript = $"INSERT INTO {_sqlTableNames[Constants.DocumentDatabase.DOCUMENT_METADATA_DEFINITIONS_TABLE_NAME_KEY]} (Id, ExtractedName)"
                        + $"VALUES (@Id, @ExtractedName)";

            _msSqlDataSource.PerformCommand(sqlScript, parameters);
        }

        private void InsertMetadata(Guid documentId, Guid metadataDefinitionId, string value)
        {
            var parameters = _dataParameterFactory
                                .ConfigureParameter("@Id", SqlDbType.UniqueIdentifier, Guid.NewGuid())
                                .ConfigureParameter("@DocumentId", SqlDbType.UniqueIdentifier, documentId)
                                .ConfigureParameter("@DocumentMetadataDefinitionId", SqlDbType.UniqueIdentifier, metadataDefinitionId)
                                .ConfigureParameter("@Value", SqlDbType.VarChar, value, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                                .GetConfiguredParameters();

            var sqlScript = $"INSERT INTO {_sqlTableNames[Constants.DocumentDatabase.DOCUMENT_METADATA_TABLE_NAME_KEY]} (Id, DocumentId, DocumentMetadataDefinitionId, Value)"
                        + $"VALUES (@Id, @DocumentId, @DocumentMetadataDefinitionId, @Value)";

            _msSqlDataSource.PerformCommand(sqlScript, parameters);
        }
        #endregion
    }
}
