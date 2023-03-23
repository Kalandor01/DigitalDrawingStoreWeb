using Microsoft.AspNetCore.Mvc;
using Unity;
using XperiCad.Common.Infrastructure.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Application.Factories;
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
        public async Task<JsonResponse<string>> GetSenderEmail()
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<string>();

            var command = new GetSenderEmailPropertyActionCommand();
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse.ToJsonResponse();
        }

        public async Task<JsonResponse<IEnumerable<string>>> GetEmailRecipitents()
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<IEnumerable<string>>();

            var command = new GetEmailRecipientsPropertyActionCommand();
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse.ToJsonResponse();
        }

        public async Task<JsonResponse<string>> GetSmtpHost()
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<string>();

            var command = new GetSmtpHostPropertyActionCommand();
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse.ToJsonResponse();
        }

        public async Task<JsonResponse<int>> GetSmtpPort()
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<int>();

            var command = new GetSmtpPortPropertyActionCommand();
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse.ToJsonResponse();
        }

        public async Task<JsonResponse<string>> GetSmtpUsername()
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<string>();

            var command = new GetSmtpUsernamePropertyActionCommand();
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse.ToJsonResponse();
        }

        public async Task<JsonResponse<string>> GetSmtpPassword()
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<string>();

            var command = new GetSmtpPasswordPropertyActionCommand();
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse.ToJsonResponse();
        }

        public async Task<JsonResponse<bool>> GetIsUseDefaultCredentials()
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<bool>();

            var command = new GetUseDefaultSmtpCredentialsPropertyActionCommand();
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse.ToJsonResponse();
        }

        public async Task<JsonResponse<bool>> GetIsEnableSsl()
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<bool>();

            var command = new GetEnableSmtpSslPropertyActionCommand();
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse.ToJsonResponse();
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
