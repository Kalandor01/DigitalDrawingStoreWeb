using Microsoft.AspNetCore.Mvc;
using Unity;
using XperiCad.Common.Infrastructure.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.Web.API.Commands;
using XperiCad.DigitalDrawingStore.Web.API.DTO;
using XperiCad.DigitalDrawingStore.Web.API.Extensions;

namespace XperiCad.DigitalDrawingStore.Web.API.Controllers
{
    public class AdministrationController : Controller
    {
        #region Public Properties
        public readonly IDocumentResourceProperties documentResourceProperties;
        #endregion

        #region ctor
        public AdministrationController()
        {
            documentResourceProperties = new DocumentResourceProperiesFactory().CreateDocumentResourceProperties(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
        }
        #endregion

        #region Public members
        public async Task<FeedbackEntities> GetFeedbackProperties()
        {
            var feedbackService = new FeedbackPropertiesServiceFactory().CreateFeedbackPropertyService(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
            var feedbackProperties = new FeedbackProperties(feedbackService);

            return new FeedbackEntities(
                await feedbackProperties.GetSenderEmailAsync(),
                await feedbackProperties.GetEmailRecipientsAsync(),
                await feedbackProperties.GetSmtpHostAsync(),
                await feedbackProperties.GetSmtpPortAsync(),
                await feedbackProperties.GetSmtpUsernameAsync(),
                await feedbackProperties.GetSmtpPasswordAsync(),
                await feedbackProperties.GetIsUseDefaultCredentialsAsync(),
                await feedbackProperties.GetIsEnableSslAsync()
            );
        }

        public async Task<JsonResponse<string>> UpdateApplicationConfiguration(
            string? documentDatabaseConnectionString
        )
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<JsonResponse<string>>();

            var command = new UpdateDocumentDatabaseConnectionStringActionCommand(
                documentDatabaseConnectionString,
                Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse.ResponseObject;
        }

        public async Task<JsonResponse<string>> UpdateFeedbackProperties(
            string? senderEmail, string? emailRecipients, string? smtpHost,
            string? smtpPort, string? smtpUsername, string? smtpPassword,
            string? isUseDefaultCredentials, string? isEnableSsl)
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
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
        #endregion

        #region Private members
        #endregion
    }
}
