using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.BL.Services;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands.Get
{
    public class GetDocumentMetadataActionCommand : AActionCommand<IDictionary<string, string>>
    {
        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region Fields
        private IDocumentService _documentService;
        private readonly Guid _documentId;
        #endregion

        #region ctor
        public GetDocumentMetadataActionCommand(Guid documentId)
        {
            var documentServiceFactory = new DocumentServiceFactory(); // TODO: inject
            _documentService = documentServiceFactory.CreateDocumentService(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
            _documentId = documentId;
        }
        #endregion

        #region ICommand members
        public override void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public override async Task ExecuteAsync()
        {
            var response = default(IDictionary<string, string>);

            var documentMetadataPromise = await _documentService.QueryDocumentMetadataAsync(_documentId);
            QueueFeedback(documentMetadataPromise);

            if (documentMetadataPromise.IsOkay)
            {
                response = documentMetadataPromise.ResponseObject;
            }

            ResolveAction(response);
        }
        #endregion
    }
}
