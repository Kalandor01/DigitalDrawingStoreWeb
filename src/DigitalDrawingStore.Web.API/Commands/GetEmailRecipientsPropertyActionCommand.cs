using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.BL.Services;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands
{
    public class GetEmailRecipientsPropertyActionCommand : AActionCommand<IEnumerable<string>>
    {
        #region Constants
        private static readonly string SPLIT_STRING = ";";
        #endregion

        #region Fields
        private readonly IFeedbackPropertiesService _feedbackPropertyService;
        #endregion

        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region ctor
        public GetEmailRecipientsPropertyActionCommand()
        {
            var feedbackPropertyService = new FeedbackPropertiesServiceFactory().CreateFeedbackPropertyService(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
            _feedbackPropertyService = feedbackPropertyService ?? throw new ArgumentNullException(nameof(feedbackPropertyService));
        }
        #endregion

        #region AActionCommand members
        public override void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public async override Task ExecuteAsync()
        {
            var response = new List<string>();

            var feedbackPromise = await _feedbackPropertyService.QueryEmailRecipitentsAsync();
            QueueFeedback(feedbackPromise);

            if (feedbackPromise.IsOkay)
            {
                var responseString = feedbackPromise.ResponseObject;
                if (responseString is not null)
                {
                    foreach (var email in responseString.Split(SPLIT_STRING))
                    {
                        if (email is not null)
                        {
                            response.Add(email);
                        }
                    }
                }
            }

            ResolveAction(response);
        }
        #endregion
    }
}
