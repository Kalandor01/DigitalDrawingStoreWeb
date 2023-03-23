using XperiCad.Common.Infrastructure.Behaviours.Queries;
using XperiCad.DigitalDrawingStore.BL.Documents.Commands;
using XperiCad.DigitalDrawingStore.BL.Documents.Queries;
using XperiCad.DigitalDrawingStore.BL.Services;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Services
{
    public class FeedbackPropertiesService : IFeedbackPropertiesService
    {
        #region Fields
        private readonly IFeedbackPropertyQuery _feedbackPropertyQuery;
        private readonly IUpdateFeedbackPropertiesCommand _updateFeedbackPropertyCommand;
        #endregion

        #region ctor
        public FeedbackPropertiesService(
            IFeedbackPropertyQuery feedbackPropertyQuery,
            IUpdateFeedbackPropertiesCommand updateFeedbackPropertyCommand)
        {
            _feedbackPropertyQuery = feedbackPropertyQuery ?? throw new ArgumentNullException(nameof(feedbackPropertyQuery));
            _updateFeedbackPropertyCommand = updateFeedbackPropertyCommand ?? throw new ArgumentNullException(nameof(updateFeedbackPropertyCommand));
        }
        #endregion

        #region IFeedbackPropertyService members
        public async Task<IPromise<string>> QuerySenderEmailAsync()
        {
            return await _feedbackPropertyQuery.QueryFeedbackPropertyAsync(Constants.FeedbackProperties.SENDER_EMAIL_NAME_KEY);
        }

        public async Task<IPromise<string>> QueryEmailRecipitentsAsync()
        {
            return await _feedbackPropertyQuery.QueryFeedbackPropertyAsync(Constants.FeedbackProperties.EMAIL_RECIPIENTS_NAME_KEY);
        }

        public async Task<IPromise<string>> QuerySmtpHostAsync()
        {
            return await _feedbackPropertyQuery.QueryFeedbackPropertyAsync(Constants.FeedbackProperties.SMTP_HOST_NAME_KEY);
        }

        public async Task<IPromise<string>> QuerySmtpPortAsync()
        {
            return await _feedbackPropertyQuery.QueryFeedbackPropertyAsync(Constants.FeedbackProperties.SMTP_PORT_NAME_KEY);
        }

        public async Task<IPromise<string>> QuerySmtpUsernameAsync()
        {
            return await _feedbackPropertyQuery.QueryFeedbackPropertyAsync(Constants.FeedbackProperties.SMTP_USERNAME_NAME_KEY);
        }

        public async Task<IPromise<string>> QuerySmtpPasswordAsync()
        {
            return await _feedbackPropertyQuery.QueryFeedbackPropertyAsync(Constants.FeedbackProperties.SMTP_PASSWORD_NAME_KEY);
        }

        public async Task<IPromise<string>> QueryIsUseDefaultCredentialsAsync()
        {
            return await _feedbackPropertyQuery.QueryFeedbackPropertyAsync(Constants.FeedbackProperties.IS_USE_DEFAULT_SMTP_CREDENTIALS_NAME_KEY);
        }

        public async Task<IPromise<string>> QueryIsEnableSslAsync()
        {
            return await _feedbackPropertyQuery.QueryFeedbackPropertyAsync(Constants.FeedbackProperties.IS_SMTP_SSL_ENABLED_NAME_KEY);
        }

        public async Task<bool> UpdateSenderEmailAsync(string propertyValue)
        {
            return await _updateFeedbackPropertyCommand.UpdateFeedbackPropertyAsync(Constants.FeedbackProperties.SENDER_EMAIL_NAME_KEY, propertyValue);
        }

        public async Task<bool> UpdateEmailRecipientsAsync(string propertyValue)
        {
            return await _updateFeedbackPropertyCommand.UpdateFeedbackPropertyAsync(Constants.FeedbackProperties.EMAIL_RECIPIENTS_NAME_KEY, propertyValue);
        }

        public async Task<bool> UpdateSmtpHostAsync(string propertyValue)
        {
            return await _updateFeedbackPropertyCommand.UpdateFeedbackPropertyAsync(Constants.FeedbackProperties.SMTP_HOST_NAME_KEY, propertyValue);
        }

        public async Task<bool> UpdateSmtpPortAsync(string propertyValue)
        {
            return await _updateFeedbackPropertyCommand.UpdateFeedbackPropertyAsync(Constants.FeedbackProperties.SMTP_PORT_NAME_KEY, propertyValue);
        }

        public async Task<bool> UpdateSmtpUsernameAsync(string propertyValue)
        {
            return await _updateFeedbackPropertyCommand.UpdateFeedbackPropertyAsync(Constants.FeedbackProperties.SMTP_USERNAME_NAME_KEY, propertyValue);
        }

        public async Task<bool> UpdateSmtpPasswordAsync(string propertyValue)
        {
            return await _updateFeedbackPropertyCommand.UpdateFeedbackPropertyAsync(Constants.FeedbackProperties.SMTP_PASSWORD_NAME_KEY, propertyValue);
        }

        public async Task<bool> UpdateIsUseDefaultCredentialsAsync(string propertyValue)
        {
            return await _updateFeedbackPropertyCommand.UpdateFeedbackPropertyAsync(Constants.FeedbackProperties.IS_USE_DEFAULT_SMTP_CREDENTIALS_NAME_KEY, propertyValue);
        }

        public async Task<bool> UpdateIsEnableSslAsync(string propertyValue)
        {
            return await _updateFeedbackPropertyCommand.UpdateFeedbackPropertyAsync(Constants.FeedbackProperties.IS_SMTP_SSL_ENABLED_NAME_KEY, propertyValue);
        }
        #endregion
    }
}
