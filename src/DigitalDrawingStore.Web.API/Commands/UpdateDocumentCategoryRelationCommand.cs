using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.Common.Infrastructure.Feedback;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.BL.Services;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands
{
    public class UpdateDocumentCategoryRelationCommand : AActionCommand<bool>
    {
        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region Fields
        private IDocumentService _documentService;
        private readonly Guid _documentId;
        private readonly Guid _newCategoryId;
        private readonly IFeedbackMessageFactory _feedbackMessageFactory;
        private bool _canExecute;
        #endregion

        #region ctor
        public UpdateDocumentCategoryRelationCommand(Guid documentId, Guid newCategoryId, IFeedbackMessageFactory feedbackMessageFactory)
        {
            _canExecute = true;
            _feedbackMessageFactory = feedbackMessageFactory ?? throw new ArgumentNullException(nameof(feedbackMessageFactory));

            if (documentId == Guid.Empty)
            {
                QueueFeedback(_feedbackMessageFactory.CreateFeedbackMessage(Resources.i18n.Feedback.Error_DocumentIdIsEmpty));
                _canExecute = false;
            }
            if (newCategoryId == Guid.Empty)
            {
                QueueFeedback(_feedbackMessageFactory.CreateFeedbackMessage(Resources.i18n.Feedback.Error_CategoryIdIsEmpty));
                _canExecute = false;
            }

            var documentServiceFactory = new DocumentServiceFactory(); // TODO: inject
            _documentService = documentServiceFactory.CreateDocumentService(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
            _documentId = documentId;
            _newCategoryId = newCategoryId;
        }
        #endregion

        #region ICommand members
        public override void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public override async Task ExecuteAsync()
        {
            var response = false;

            if (_canExecute)
            {
                response = await _documentService.UpdateDocumentCategoryRelationAsync(_documentId, _newCategoryId);
            }

            ResolveAction(response);
        }
        #endregion
    }
}
