using System.Net.Mail;
using XperiCad.Common.Core.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.Web.API.DTO;
using XperiCad.Common.Infrastructure.Feedback;
using Unity;
using XperiCad.Common.Infrastructure.Application;
using XperiCad.DigitalDrawingStore.Web.API.Resources.i18n;

namespace XperiCad.DigitalDrawingStore.Web.API.Commands
{
    public class UpdateFeedbackPropertiesActionCommand : AActionCommand<JsonResponse<string>>
    {
        private const string EMAIL_RECIPIENTS_SEP_STRING = ",";

        #region Fields
        private readonly string _selectedCulture;
        private readonly IFeedbackProperties _feedbackProperties;
        private readonly string _senderEmail;
        private readonly string _emailRecipients;
        private readonly string _smtpHost;
        private readonly string _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _isUseDefaultSmtpCredentials;
        private readonly string _isEnableSsl;
        #endregion

        #region Properties
        public override bool CanExecute => true;
        #endregion

        #region ctor
        public UpdateFeedbackPropertiesActionCommand(
            string senderEmail,
            string emailRecipients,
            string smtpHost,
            string smtpPort,
            string smtpUsername,
            string smtpPassword,
            string isUseDefaultSmtpCredentials,
            string isEnableSsl
            )
        {
            var feedbackPropertiesService = new FeedbackPropertiesServiceFactory().CreateFeedbackPropertyService(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
            _feedbackProperties = new FeedbackProperties(feedbackPropertiesService);

            if (string.IsNullOrWhiteSpace(senderEmail))
            {
                throw new ArgumentException($"'{nameof(senderEmail)}' cannot be null or whitespace.", nameof(senderEmail));
            }
            if (string.IsNullOrWhiteSpace(emailRecipients))
            {
                throw new ArgumentException($"'{nameof(emailRecipients)}' cannot be null or whitespace.", nameof(emailRecipients));
            }
            if (string.IsNullOrWhiteSpace(smtpHost))
            {
                throw new ArgumentException($"'{nameof(smtpHost)}' cannot be null or whitespace.", nameof(smtpHost));
            }
            if (string.IsNullOrWhiteSpace(smtpPort))
            {
                throw new ArgumentException($"'{nameof(smtpPort)}' cannot be null or whitespace.", nameof(smtpPort));
            }
            if (string.IsNullOrWhiteSpace(smtpUsername))
            {
                throw new ArgumentException($"'{nameof(smtpUsername)}' cannot be null or whitespace.", nameof(smtpUsername));
            }
            if (string.IsNullOrWhiteSpace(smtpPassword))
            {
                throw new ArgumentException($"'{nameof(smtpPassword)}' cannot be null or whitespace.", nameof(smtpPassword));
            }
            if (string.IsNullOrWhiteSpace(isUseDefaultSmtpCredentials))
            {
                throw new ArgumentException($"'{nameof(isUseDefaultSmtpCredentials)}' cannot be null or whitespace.", nameof(isUseDefaultSmtpCredentials));
            }
            if (string.IsNullOrWhiteSpace(isUseDefaultSmtpCredentials))
            {
                throw new ArgumentException($"'{nameof(isEnableSsl)}' cannot be null or whitespace.", nameof(isEnableSsl));
            }

            var container = new ContainerFactory().CreateContainer();
            var commonApplicationProperties = container.Resolve<ICommonApplicationProperties>();
            _selectedCulture = commonApplicationProperties.GeneralApplicationProperties.SelectedCulture.LanguageCountryCode;

            _senderEmail = senderEmail;
            _emailRecipients = emailRecipients;
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
            _isUseDefaultSmtpCredentials = isUseDefaultSmtpCredentials;
            _isEnableSsl = isEnableSsl;
        }
        #endregion

        #region AActionCommand members
        public override void Execute()
        {
            ExecuteAsync().RunSynchronously();
        }

        public async override Task ExecuteAsync()
        {
            var responses = new List<FeedbackMessage>();

            //Sender email
            if (IsEmailAddressValid(_senderEmail))
            {
                responses.Add(GetFeedbackMessage(await _feedbackProperties.UpdateSenderEmailAsync(_senderEmail)));
            }
            else
            {
                responses.Add(GetFeedbackMessage(Feedback.Fatal_Wrong_Email_Format));
            }

            //Email recipients
            var emailRecipients = _emailRecipients.Replace("\n", ",");
            emailRecipients = emailRecipients.Replace(" ", "");
            var emailRecipientsList = emailRecipients.Split(EMAIL_RECIPIENTS_SEP_STRING);
            
            var filteredEmailRecipientsList = new List<string>();
            foreach (var emailRecipient in emailRecipientsList)
            {
                if (!string.IsNullOrWhiteSpace(emailRecipient))
                {
                    filteredEmailRecipientsList.Add(emailRecipient);
                }
            }

            var emailsValid = true;
            foreach (var emailRecipient in filteredEmailRecipientsList)
            {
                if (!IsEmailAddressValid(emailRecipient))
                {
                    emailsValid = false;
                    break;
                }
            }

            if (emailsValid)
            {
                responses.Add(GetFeedbackMessage(await _feedbackProperties.UpdateEmailRecipientsAsync(filteredEmailRecipientsList)));
            }
            else
            {
                responses.Add(GetFeedbackMessage(Feedback.Fatal_Wrong_Email_Format));
            }

            //Smtp host
            responses.Add(GetFeedbackMessage(await _feedbackProperties.UpdateSmtpHostAsync(_smtpHost)));

            //Smtp 
            var parsedPort = int.TryParse(_smtpPort, out int smtpPortParsed);
            if (parsedPort)
            {
                if (smtpPortParsed >= 0 && smtpPortParsed <= 65535)
                {
                    responses.Add(GetFeedbackMessage(await _feedbackProperties.UpdateSmtpPortAsync(smtpPortParsed)));
                }
                else
                {
                    responses.Add(GetFeedbackMessage(Feedback.Fatal_Port_Number_Not_In_Range));
                }
            }
            else
            {
                responses.Add(GetFeedbackMessage(Feedback.Fatal_Not_Number));
            }

            //Smtp username / password
            responses.Add(GetFeedbackMessage(await _feedbackProperties.UpdateSmtpUsernameAsync(_smtpUsername)));
            responses.Add(GetFeedbackMessage(await _feedbackProperties.UpdateSmtpPasswordAsync(_smtpPassword)));

            //use default credentials
            var useDefaultCredentials = _isUseDefaultSmtpCredentials == "true";
            responses.Add(GetFeedbackMessage(await _feedbackProperties.UpdateIsUseDefaultCredentialsAsync(useDefaultCredentials)));

            //eanble ssl
            var enableSsl = _isEnableSsl == "true";
            responses.Add(GetFeedbackMessage(await _feedbackProperties.UpdateIsEnableSslAsync(enableSsl)));

            ResolveAction(GetJsonResponse(responses));
        }
        #endregion

        #region Private members
        private bool IsEmailAddressValid(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private JsonResponse<string> GetJsonResponse(List<FeedbackMessage> feedbackMessages)
        {
            var success = true;
            var element = 0;
            IFeedbackResource response;
            for (; element < feedbackMessages.Count(); element++)
            {
                if (feedbackMessages.ElementAt(element).Severity != Severity.Information)
                {
                    success = false;
                    break;
                }
            }
            response = success ? Feedback.Information_Successfully_Modified : Feedback.Fatal_Couldnt_Modify;
            var responseMessage = response.CultureResource.GetCultureString(_selectedCulture).FirstOrDefault().Value;
            return new JsonResponse<string>(responseMessage, feedbackMessages, success);
        }
        private FeedbackMessage GetFeedbackMessage(bool success)
        {
            return GetFeedbackMessage(success ? Feedback.Information_Successfully_Modified : Feedback.Fatal_Couldnt_Modify);
        }
        private FeedbackMessage GetFeedbackMessage(IFeedbackResource feedbackResource)
        {
            var feedbackMessage = feedbackResource.CultureResource.GetCultureString(_selectedCulture).FirstOrDefault().Value;
            return new FeedbackMessage(feedbackResource.Severity, feedbackMessage);
        }
        #endregion
    }
}
