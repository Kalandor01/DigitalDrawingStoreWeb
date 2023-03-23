using System.Data;
using XperiCad.Common.Core.Exceptions;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents.Commands;
using i18n = XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents.Commands
{
    internal class MsSqlUpdateDocumentCategoryRelationCommand : IUpdateDocumentCategoryRelationCommand
    {
        private readonly IDataSource _msSqlDataSource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDictionary<string, string> _sqlTableNames;

        #region ctor
        public MsSqlUpdateDocumentCategoryRelationCommand(
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames)
        {
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
        }
        #endregion

        public async Task<bool> UpdateDocumentCategoryRelationAsync(Guid documentId, Guid newCategoryId)
        {
            if (documentId == Guid.Empty)
            {
                throw new FeedbackException($"{nameof(documentId)}", i18n.Feedback.Error_CouldNotUpdateBecauseGivenIdIsInvalid);
            }

            if (newCategoryId == Guid.Empty)
            {
                throw new FeedbackException($"{nameof(newCategoryId)}", i18n.Feedback.Error_CouldNotUpdateBecauseGivenIdIsInvalid);
            }

            var parameters = _dataParameterFactory
                            .ConfigureParameter("@DocumentId", SqlDbType.UniqueIdentifier, documentId)
                            .ConfigureParameter("@NewCategoryId", SqlDbType.UniqueIdentifier, newCategoryId)
                            .GetConfiguredParameters();

            _ = await _msSqlDataSource.PerformCommandAsync(
                $"UPDATE {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENTS_TABLE_NAME_KEY]}"
                + $" SET DocumentCategoryId = @NewCategoryId"
                + $" WHERE Id = @DocumentId",
                parameters);

            return true;
        }
    }
}
