using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Documents;
using XperiCad.DigitalDrawingStore.BL.Services;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands.Get
{
    public class GetDocumentByIdActionCommand : AActionCommand<IDocument>
    {
        #region Fields
        private readonly Guid _documentId;
        private readonly IDocumentService _documentService;
        #endregion

        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region ctor
        public GetDocumentByIdActionCommand(Guid documentId, IDocumentService documentService)
        {
            _documentId = documentId;
            _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
        }
        #endregion

        #region AActionCommand members
        public override void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public async override Task ExecuteAsync()
        {
            var documentPromise = await _documentService.GetDocumentByIdAsync(_documentId);
            QueueFeedback(documentPromise);

            if (documentPromise.IsOkay)
            {
                ResolveAction(documentPromise.ResponseObject);
            }
        }
        #endregion
    }
}
