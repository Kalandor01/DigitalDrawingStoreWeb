using System.Data;
using XperiCad.Common.Core.Exceptions;
using XperiCad.Common.Infrastructure.DataSource;
using XperiCad.DigitalDrawingStore.BL.Documents.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Factories;
using i18n = XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Documents.Commands
{
    internal class MsSqlUpdateDocumentCategoryEntitiesCommand : IUpdateDocumentCategoryEntitiesCommand
    {
        #region Fields
        private readonly IDataSource _msSqlDataSource;
        private readonly IDataParameterFactory _dataParameterFactory;
        private readonly IDictionary<string, string> _sqlTableNames;
        private readonly IDocumentCategoryFactory _documentCategoryFactory;
        #endregion

        #region ctor
        public MsSqlUpdateDocumentCategoryEntitiesCommand(
            IDataSource msSqlDataSource,
            IDataParameterFactory dataParameterFactory,
            IDictionary<string, string> sqlTableNames,
            IDocumentCategoryFactory documentCategoryFactory)
        {
            _msSqlDataSource = msSqlDataSource ?? throw new ArgumentNullException(nameof(msSqlDataSource));
            _dataParameterFactory = dataParameterFactory ?? throw new ArgumentNullException(nameof(dataParameterFactory));
            _sqlTableNames = sqlTableNames ?? throw new ArgumentNullException(nameof(sqlTableNames));
            _documentCategoryFactory = documentCategoryFactory ?? throw new ArgumentNullException(nameof(documentCategoryFactory));
        }
        #endregion

        #region IUpdateDocumentCategoryEntitiesCommand members
        public async Task<bool> UpdateDocumentCategoryEntitiesAsync(Guid documentCategoryId, IDictionary<string, string> categoryEntities, IDictionary<Guid, string> metadataDefinitions)
        {
            //TODO: remove after testing
            //throw new FeedbackException($"{nameof(categoryEntities)}", i18n.Feedback.Warning_DocumentCategoryCouldNotBeProcessed);

            if (Equals(documentCategoryId, Guid.Empty))
            {
                return false;
                throw new FeedbackException($"{nameof(documentCategoryId)}", i18n.Feedback.Error_CouldNotUpdateBecauseGivenIdIsInvalid);
            }

            if (categoryEntities == null || categoryEntities.Count == 0)    //TODO: ask if categoryEntities.Count == 0 is a valid use case
            {
                return false;
                throw new FeedbackException($"{nameof(categoryEntities)}", i18n.Feedback.Error_CouldNotUpdateBecauseCategoryEntitiesAreInvalid);
            }

            if (metadataDefinitions == null || metadataDefinitions.Count == 0)
            {
                return false;
                throw new FeedbackException($"{nameof(metadataDefinitions)}", i18n.Feedback.Error_CouldNotUpdateBecauseMetadataDefinitionsAreInvalid);
            }

            var documentCategory = await _documentCategoryFactory.CreateDocumentCategoryAsync(documentCategoryId);

            DeleteCurrentEntitiesAsync(documentCategoryId);

            foreach (var metadataDefinition in metadataDefinitions)
            {
                if (metadataDefinition.Key != Guid.Empty && metadataDefinition.Value != null)
                {
                    if (categoryEntities.ContainsKey(metadataDefinition.Value))
                    {
                        documentCategory.SetDocumentCategoryEntityAsync(metadataDefinition.Key);
                    }
                }
            }

            return true;
        }
        #endregion

        #region Private methods
        private async void DeleteCurrentEntitiesAsync(Guid id)
        {
            var parameters = _dataParameterFactory
                                .ConfigureParameter("@CategoryId", SqlDbType.UniqueIdentifier, id)
                                .GetConfiguredParameters();

            _ = await _msSqlDataSource.PerformCommandAsync(
            $"   DELETE FROM {_sqlTableNames[Constants.Documents.Resources.DatabaseTables.DOCUMENT_CATEGORY_ENTITIES_TABLE_NAME_KEY]}"
            + $" WHERE DocumentCategoryId = @CategoryId",
            parameters);
        }
        #endregion
    }
}
