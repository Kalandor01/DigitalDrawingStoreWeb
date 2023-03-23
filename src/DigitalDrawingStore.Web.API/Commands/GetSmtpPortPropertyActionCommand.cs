using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.BL.Services;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands
{
    public class GetSmtpPortPropertyActionCommand : AActionCommand<int>
    {
        #region Fields
        private readonly IFeedbackPropertiesService _feedbackPropertyService;
        #endregion

        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region ctor
        public GetSmtpPortPropertyActionCommand()
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
            var feedbackPromise = await _feedbackPropertyService.QuerySmtpPortAsync();
            QueueFeedback(feedbackPromise);

            if (feedbackPromise.IsOkay)
            {
                var response = 0;

                var isSuccessfullyParsed = int.TryParse(feedbackPromise.ResponseObject, out int port);
                if (isSuccessfullyParsed)
                {
                    response = port;
                }

                ResolveAction(response);
            }
        }
        #endregion
    }
}
