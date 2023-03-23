using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Services;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands
{
    public class GetTargetOfDocumentUsageListCommand : AActionCommand<IDictionary<Guid, string>>
    {
        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region Fields
        private readonly IDocumentService _documentService;
        #endregion

        #region ctor
        public GetTargetOfDocumentUsageListCommand(IDocumentService documentService)
        {
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
            var result = await _documentService.QueryAllTargetOfDocumentUsageAsync();
            
            QueueFeedback(result);
            ResolveAction(result.ResponseObject);
        }
        #endregion
    }
}
