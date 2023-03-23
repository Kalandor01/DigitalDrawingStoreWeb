using System.Data;
using XperiCad.Common.Core.DataSource;
using XperiCad.Common.Core.Exceptions;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories;
using i18n = XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents.Commands
{
    internal class MsSqlUpdateDocumentMetadataCommand : IUpdateDocumentMetadataCommand
    {
        private readonly IDataSource _msSqlDataSource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDictionary<string, string> _sqlTableNames;
        #region Fields
        private readonly IDocumentFactory _documentFactory;
        #endregion

        #region ctor
        public MsSqlUpdateDocumentMetadataCommand(
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames,
            IDocumentFactory documentFactory)
        {
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
            _documentFactory = documentFactory ?? throw new ArgumentNullException(nameof(documentFactory));
        }
        #endregion

        #region IUpdateDocumentMetadataCommand members
        public async Task<bool> UpdateDocumentMetadata(Guid documentId, string metadataName, string metadataValue, string oldMetadataName)
        {
            // TODO: conscheck in feedbackQueue

            //TODO: use this feedback message if a table was not found and throw an exception
            //feedbackQueue.Add(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.Error_NoSuchTableFoundInDatabase, "TODO: tableName"));

            if (Equals(documentId, Guid.Empty))
            {
                return false;
                throw new FeedbackException($"{nameof(documentId)}", i18n.Feedback.Error_CouldNotUpdateBecauseGivenIdIsInvalid);
            }

            if (string.IsNullOrWhiteSpace(metadataName))
            {
                return false;
                throw new FeedbackException($"{nameof(metadataName)}", i18n.Feedback.Error_CouldNotUpdateBecauseGivenMetadataNameIsInvalid);
            }

            if (string.IsNullOrWhiteSpace(oldMetadataName))
            {
                return false;
                throw new FeedbackException($"{nameof(oldMetadataName)}", i18n.Feedback.Error_CouldNotUpdateBecauseGivenMetadataNameIsInvalid);
            }

            var document = _documentFactory.CreatePdfDocument(documentId, "temp/path");

            if (metadataValue != (await document.GetAttribute<string?>(oldMetadataName) ?? string.Empty))
            {
                await document.SetAttribute(oldMetadataName, metadataValue);
            }

            if (oldMetadataName != metadataName)
            {
                var parameters = _dataParameterFactory
                                .ConfigureParameter("@OldMetadataName", SqlDbType.VarChar, oldMetadataName, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                                .ConfigureParameter("@MetadataName", SqlDbType.VarChar, metadataName, SqlTypeLengthConstants.VARCHAR_MAX_LENGTH)
                                .GetConfiguredParameters();

                _ = _msSqlDataSource.PerformCommand(
                    $"   UPDATE {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENTS_METADATA_DEFINITIONS_TABLE_NAME_KEY]}"
                    + $" SET ExtractedName = @MetadataName"
                    + $" WHERE ExtractedName = @OldMetadataName",
                    parameters);
            }

            return true;
        }
        #endregion
    }
}
