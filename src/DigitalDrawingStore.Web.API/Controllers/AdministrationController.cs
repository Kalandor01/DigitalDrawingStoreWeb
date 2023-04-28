using Microsoft.AspNetCore.Mvc;
using Unity;
using XperiCad.Common.Infrastructure.Application;
using XperiCad.Common.Infrastructure.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.Web.API.Commands.Set;
using XperiCad.DigitalDrawingStore.Web.API.DTO;
using XperiCad.DigitalDrawingStore.Web.API.Resources.i18n;

namespace XperiCad.DigitalDrawingStore.Web.API.Controllers
{
    public class AdministrationController : Controller
    {
        #region Fields
        private readonly IUnityContainer _container;
        private readonly string _selectedCulture;
        private readonly IFeedbackProperties _feedbackProperties;
        #endregion

        #region Public Properties
        public readonly IDocumentResourceProperties documentResourceProperties;
        #endregion

        #region ctor
        public AdministrationController()
        {
            documentResourceProperties = new DocumentResourceProperiesFactory().CreateDocumentResourceProperties(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);

            var feedbackService = new FeedbackPropertiesServiceFactory().CreateFeedbackPropertyService(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
            _feedbackProperties = new FeedbackProperties(feedbackService);
            _container = new ContainerFactory().CreateContainer();
            var commonApplicationProperties = _container.Resolve<ICommonApplicationProperties>();
            _selectedCulture = commonApplicationProperties.GeneralApplicationProperties.SelectedCulture.LanguageCountryCode;
        }
        #endregion

        #region Public members
        public async Task<FeedbackEntities> GetFeedbackProperties()
        {
            

            return new FeedbackEntities(
                await _feedbackProperties.GetSenderEmailAsync(),
                await _feedbackProperties.GetEmailRecipientsAsync(),
                await _feedbackProperties.GetSmtpHostAsync(),
                await _feedbackProperties.GetSmtpPortAsync(),
                await _feedbackProperties.GetSmtpUsernameAsync(),
                await _feedbackProperties.GetSmtpPasswordAsync(),
                await _feedbackProperties.GetIsUseDefaultCredentialsAsync(),
                await _feedbackProperties.GetIsEnableSslAsync()
            );
        }

        public async Task<JsonResponse<string>> UpdateApplicationConfiguration(
            string? documentDatabaseConnectionString
        )
        {
            if (!string.IsNullOrWhiteSpace(documentDatabaseConnectionString))
            {
                var commandInvokerFactory = _container.Resolve<ICommandInvokerFactory>();
                var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<JsonResponse<string>>();

                var command = new UpdateDocumentDatabaseConnectionStringActionCommand(
                    documentDatabaseConnectionString,
                    Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
                commandInvoker.AddCommand(command);
                await commandInvoker.ExecuteAllAsync();

                return commandInvoker.ActionResponse.ResponseObject;
            }
            else
            {
                return GetEmptyFieldResponse();
            }
        }

        public async Task<JsonResponse<string>> UpdateFeedbackProperties(
            string? senderEmail, string? emailRecipients, string? smtpHost,
            string? smtpPort, string? smtpUsername, string? smtpPassword,
            string? isUseDefaultCredentials, string? isEnableSsl)
        {
            if (
                !string.IsNullOrWhiteSpace(senderEmail) &&
                !string.IsNullOrWhiteSpace(emailRecipients) &&
                !string.IsNullOrWhiteSpace(smtpHost) &&
                !string.IsNullOrWhiteSpace(smtpPort) &&
                !string.IsNullOrWhiteSpace(smtpUsername) &&
                !string.IsNullOrWhiteSpace(smtpPassword) &&
                !string.IsNullOrWhiteSpace(isUseDefaultCredentials) &&
                !string.IsNullOrWhiteSpace(isEnableSsl)
            )
            {
                var commandInvokerFactory = _container.Resolve<ICommandInvokerFactory>();
                var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<JsonResponse<string>>();

                var command = new UpdateFeedbackPropertiesActionCommand(
                senderEmail,
                emailRecipients,
                smtpHost,
                smtpPort,
                smtpUsername,
                smtpPassword,
                isUseDefaultCredentials,
                isEnableSsl
                );
                commandInvoker.AddCommand(command);
                await commandInvoker.ExecuteAllAsync();

                return commandInvoker.ActionResponse.ResponseObject;
            }
            else
            {
                return GetEmptyFieldResponse();
            }
        }
        #endregion

        #region Private members
        private JsonResponse<string> GetEmptyFieldResponse()
        {
            var feedbackResource = Feedback.Fatal_Empty_Field;
            var feedbackMessage = feedbackResource.CultureResource.GetCultureString(_selectedCulture).FirstOrDefault().Value;
            var feedbackMessages = new List<FeedbackMessage> { new FeedbackMessage(feedbackResource.Severity, feedbackMessage) };
            return new JsonResponse<string>(feedbackMessage, feedbackMessages, false);
        }
        #endregion
    }
}
