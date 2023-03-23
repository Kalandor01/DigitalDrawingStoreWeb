using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.BL.Services;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands
{
    public class GetEnableSmtpSslPropertyActionCommand : AActionCommand<bool>
    {
        #region Fields
        private readonly IFeedbackPropertiesService _feedbackPropertyService;
        #endregion

        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region ctor
        public GetEnableSmtpSslPropertyActionCommand()
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
            var feedbackPromise = await _feedbackPropertyService.QueryIsEnableSslAsync();
            QueueFeedback(feedbackPromise);

            if (feedbackPromise.IsOkay)
            {
                var responseString = feedbackPromise.ResponseObject;

                var response = false;

                var isSuccessfullyParsed = bool.TryParse(responseString, out bool isEnableSsl);
                if (isSuccessfullyParsed)
                {
                    response =  isEnableSsl;
                }

                ResolveAction(response);
            }
        }
        #endregion
    }
}
