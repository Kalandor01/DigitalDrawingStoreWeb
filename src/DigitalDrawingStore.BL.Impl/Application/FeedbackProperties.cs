using System.Data;
using XperiCad.DigitalDrawingStore.BL.Services;

namespace XperiCad.DigitalDrawingStore.BL.Impl.Application
{
    public class FeedbackProperties : IFeedbackProperties
    {
        #region Fields
        private readonly IFeedbackPropertiesService _feedbackPropertiesService;
        #endregion

        #region ctor
        public FeedbackProperties(IFeedbackPropertiesService feedbackPropertiesService)
        {
            _feedbackPropertiesService = feedbackPropertiesService ?? throw new ArgumentNullException(nameof(feedbackPropertiesService));
        }
        #endregion

        #region Private members
        public async Task<string> GetSenderEmailAsync()
        {
            return (await _feedbackPropertiesService.QuerySenderEmailAsync()).ResponseObject;
        }

        public async Task<IEnumerable<string>> GetEmailRecipientsAsync()
        {
            var emailRecipientsProperty = await _feedbackPropertiesService.QueryEmailRecipitentsAsync();
            if (emailRecipientsProperty.IsOkay)
            {
                var emailRecipients = emailRecipientsProperty.ResponseObject.Split(';');

                emailRecipients = emailRecipients.Where(e => !string.IsNullOrWhiteSpace(e)).ToArray();

                return emailRecipients;
            }

            return new List<string>();
        }

        public async Task<string> GetSmtpHostAsync()
        {
            return (await _feedbackPropertiesService.QuerySmtpHostAsync()).ResponseObject;
        }

        public async Task<int> GetSmtpPortAsync()
        {
            var smtpPortProperty = await _feedbackPropertiesService.QuerySmtpPortAsync();
            if (smtpPortProperty.IsOkay)
            {
                var isSuccessfullyParsed = int.TryParse(smtpPortProperty.ResponseObject, out int port);
                if (isSuccessfullyParsed)
                {
                    return port;
                }
            }

            return default;
        }

        public async Task<string> GetSmtpUsernameAsync()
        {
            return (await _feedbackPropertiesService.QuerySmtpUsernameAsync()).ResponseObject;
        }

        public async Task<string> GetSmtpPasswordAsync()
        {
            return (await _feedbackPropertiesService.QuerySmtpPasswordAsync()).ResponseObject;
        }

        public async Task<bool> GetIsUseDefaultCredentialsAsync()
        {
            var isUseDefaultSmtpCredentialsProperty = await _feedbackPropertiesService.QueryIsUseDefaultCredentialsAsync();
            if (isUseDefaultSmtpCredentialsProperty.IsOkay)
            {
                var isSuccessfullyParsed = bool.TryParse(isUseDefaultSmtpCredentialsProperty.ResponseObject, out bool isUseDefaultSmtpCredentials);
                if (isSuccessfullyParsed)
                {
                    return isUseDefaultSmtpCredentials;
                }
            }

            return default;
        }

        public async Task<bool> GetIsEnableSslAsync()
        {
            var isEnableSslProperty = await _feedbackPropertiesService.QueryIsEnableSslAsync();
            if (isEnableSslProperty.IsOkay)
            {
                var isSuccessfullyParsed = bool.TryParse(isEnableSslProperty.ResponseObject, out bool isEnableSsl);
                if (isSuccessfullyParsed)
                {
                    return isEnableSsl;
                }
            }
            return default;
        }

        public async Task<bool> UpdateSenderEmailAsync(string senderEmail)
        {
            return await _feedbackPropertiesService.UpdateSenderEmailAsync(senderEmail);
        }

        public async Task<bool> UpdateEmailRecipientsAsync(IEnumerable<string> emailRecipients)
        {
            var emailRecipientsString = string.Join(";", emailRecipients);
            return await _feedbackPropertiesService.UpdateEmailRecipientsAsync(emailRecipientsString);
        }

        public async Task<bool> UpdateSmtpHostAsync(string smtpHost)
        {
            return await _feedbackPropertiesService.UpdateSmtpHostAsync(smtpHost);
        }

        public async Task<bool> UpdateSmtpPortAsync(int smtpPort)
        {
            return await _feedbackPropertiesService.UpdateSmtpPortAsync(smtpPort.ToString());
        }

        public async Task<bool> UpdateSmtpUsernameAsync(string smtpUsername)
        {
            return await _feedbackPropertiesService.UpdateSmtpUsernameAsync(smtpUsername);
        }

        public async Task<bool> UpdateSmtpPasswordAsync(string smtpPassword)
        {
            return await _feedbackPropertiesService.UpdateSmtpPasswordAsync(smtpPassword);
        }

        public async Task<bool> UpdateIsUseDefaultCredentialsAsync(bool isUseDefaultCredentials)
        {
            return await _feedbackPropertiesService.UpdateIsUseDefaultCredentialsAsync(isUseDefaultCredentials.ToString());
        }

        public async Task<bool> UpdateIsEnableSslAsync(bool isEnableSsl)
        {
            return await _feedbackPropertiesService.UpdateIsEnableSslAsync(isEnableSsl.ToString());
        }
        #endregion
    }
}
