using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.BL.Services;
using XperiCad.DigitalDrawingStore.Web.API.DTO;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands
{
    public class GetDocumentCategoryEntitiesActionCommand : AActionCommand<DocumentCategoryEntities>
    {
        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region Fields
        private IDocumentService _documentService;
        private readonly Guid _categoryId;
        #endregion

        #region ctor
        public GetDocumentCategoryEntitiesActionCommand(Guid categoryId)
        {
            var documentServiceFactory = new DocumentServiceFactory(); // TODO: inject
            _documentService = documentServiceFactory.CreateDocumentService(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
            _categoryId = categoryId;
        }
        #endregion

        #region ICommand members
        public override void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public override async Task ExecuteAsync()
        {
            var response = default(DocumentCategoryEntities);

            var documentMetadataDefinitionsPromise = await _documentService.QueryDocumentCategoryEntitiesAsync();
            QueueFeedback(documentMetadataDefinitionsPromise);

            var documentCategoryAttributesPromise = await _documentService.QueryDocumentCategoryEntitiesAsync(_categoryId);
            QueueFeedback(documentCategoryAttributesPromise);

            if (documentMetadataDefinitionsPromise.IsOkay && documentCategoryAttributesPromise.IsOkay)
            {
                var usedEntities = documentCategoryAttributesPromise.ResponseObject;
                var unusedEntities = documentMetadataDefinitionsPromise.ResponseObject;

                foreach (var documentCategoryAttribute in usedEntities)
                {
                    unusedEntities.Remove(documentCategoryAttribute.Key);
                }

                response = new DocumentCategoryEntities(usedEntities, unusedEntities);
            }

            ResolveAction(response);
        }
        #endregion
    }
}
