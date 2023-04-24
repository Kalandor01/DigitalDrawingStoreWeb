using Newtonsoft.Json;
using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.Common.Infrastructure.Feedback;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.BL.Services;
using XperiCad.DigitalDrawingStore.Web.API.DTO;
using i18n = XperiCad.DigitalDrawingStore.BL.Impl.Resources.i18n;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands.Set
{
    public class UpdateDocumentCategoryEntitiesActionCommand : AActionCommand<bool>
    {
        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region Fields
        private IDocumentService _documentService;
        private readonly string _inputString;
        //private readonly IFeedbackMessageFactory _feedbackMessageFactory;
        #endregion

        #region ctor
        public UpdateDocumentCategoryEntitiesActionCommand(string inputString/*, IFeedbackMessageFactory feedbackMessageFactory*/)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new ArgumentException($"'{nameof(inputString)}' cannot be null or whitespace.", nameof(inputString));
            }

            var documentServiceFactory = new DocumentServiceFactory(); // TODO: inject
            _documentService = documentServiceFactory.CreateDocumentService(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
            _inputString = inputString;
            //_feedbackMessageFactory = feedbackMessageFactory ?? throw new ArgumentNullException(nameof(feedbackMessageFactory));
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

            var usedEntities = JsonConvert.DeserializeObject<DocumentCategoryUsedEntities>(_inputString);

            if (usedEntities != null
                && usedEntities.CategoryId != Guid.Empty
                && usedEntities.Keys != null
                && usedEntities.Values != null)
            {
                var categoryEntities = new Dictionary<string, string>();

                var keysCount = usedEntities.Keys.Length;
                for (int i = 0; i < keysCount; i++)
                {
                    categoryEntities.Add(usedEntities.Keys[i], usedEntities.Values[i]);
                }

                var entityIdPromiseContainer = await _documentService.QueryDocumentMetadataDefinitionsAsync();
                if (entityIdPromiseContainer.IsOkay)
                {
                    var metadataDefinitions = entityIdPromiseContainer.ResponseObject;
                    response = await _documentService.UpdateDocumentCategoryEntitiesAsync(usedEntities.CategoryId, categoryEntities, metadataDefinitions);
                }
            }

            if (response)
            {
                //QueueFeedback(_feedbackMessageFactory.CreateFeedbackMessage(i18n.Feedback.));
            }

            ResolveAction(response);
        }
        #endregion
    }
}
