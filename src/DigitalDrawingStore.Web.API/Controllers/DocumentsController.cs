using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unity;
using XperiCad.Common.Infrastructure.Behaviours.Commands;
using XperiCad.Common.Infrastructure.Feedback;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Documents.Watermark.Factories;
using XperiCad.DigitalDrawingStore.BL.Impl.Services.Factories;
using XperiCad.DigitalDrawingStore.Web.API.Commands;
using XperiCad.DigitalDrawingStore.Web.API.Commands.Get;
using XperiCad.DigitalDrawingStore.Web.API.Commands.Set;
using XperiCad.DigitalDrawingStore.Web.API.DTO;
using XperiCad.DigitalDrawingStore.Web.API.Extensions;

namespace XperiCad.DigitalDrawingStore.Web.API.Controllers
{
    [Route("api/Documents")]
    [Authorize(Policy = Constants.Authorization.Policies.USER)]
    public class DocumentsController : Controller
    {
        #region GET
        [Route("")]
        public async Task<JsonResponse<IEnumerable<DocumentWithPath>>> Documents(string searchText)
        {
            var container = new ContainerFactory().CreateContainer();
            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<IEnumerable<DocumentWithPath>>();

            var getDocumentsCommand = new GetDocumentsActionCommand(searchText);
            commandInvoker.AddCommand(getDocumentsCommand);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse.ToJsonResponse();
        }

        public async Task<JsonResponse<IDictionary<string, string>>> Metadata(Guid documentId)
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<IDictionary<string, string>>();

            var command = new GetDocumentMetadataActionCommand(documentId);
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse.ToJsonResponse();
        }

        public async Task<IActionResponse<IDictionary<Guid, string>>> GetTargetOfDocumentUsages()
        {
            var container = new ContainerFactory().CreateContainer();

            var documentServiceFactory = new DocumentServiceFactory();
            var documentService = documentServiceFactory.CreateDocumentService(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<IDictionary<Guid, string>>();

            var command = new GetTargetOfDocumentUsageListCommand(documentService);
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return command.ActionResponse;
        }

        public async Task<JsonResponse<string>> DocumentPreview(
            Guid id,
            float watermarkOpacity,
            int fontSize,
            string sideWatermarkPosition,
            int centeredWatermarkHorizontalOffset,
            int centeredWatermarkVerticalOffset,
            string targetOfDocumentUsage,
            string username,
            string clientMachineName,
            string clientIp)
        {
            var container = new ContainerFactory().CreateContainer();
            
            var documentServiceFactory = new DocumentServiceFactory();
            var documentService = documentServiceFactory.CreateDocumentService(Constants.Documents.Resources.APPLICATION_CONFIGURATION_FILE_PATH);

            var getDocumentByIdCommandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var getDocumentByIdCommandInvoker = getDocumentByIdCommandInvokerFactory.CreateCommandInvoker();
            var getDocumentByIdCommand = new GetDocumentByIdActionCommand(id, documentService);
            getDocumentByIdCommandInvoker.AddCommand(getDocumentByIdCommand);
            await getDocumentByIdCommandInvoker.ExecuteAllAsync();

            var documentResponse = getDocumentByIdCommand.ActionResponse;
            if (documentResponse.IsOkay)
            {
                var documentWatermarkFactory = new DocumentWatermarkFactory();
                var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
                var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<string>();

                var downloadPreviewCommand = new DownloadPreviewActionCommand(
                   documentResponse.ResponseObject,
                   documentWatermarkFactory,
                   watermarkOpacity,
                   fontSize,
                   targetOfDocumentUsage,
                   centeredWatermarkHorizontalOffset,
                   centeredWatermarkVerticalOffset,
                   sideWatermarkPosition,
                   username,
                   clientMachineName,
                   clientIp);

                commandInvoker.AddCommand(downloadPreviewCommand);
                await commandInvoker.ExecuteAllAsync();

                return commandInvoker.ActionResponse.ToJsonResponse();
            }

            var getDocumentResponse = getDocumentByIdCommand.ActionResponse.ToJsonResponse();
            return new JsonResponse<string>(string.Empty, getDocumentResponse.FeedbackMessages, getDocumentResponse.IsOkay);
        }
        #endregion

        #region PUT
        [Route("")]
        [Authorize(Policy = Constants.Authorization.Policies.ADMIN)]
        public async Task<IActionResponse<bool>> UpdateMetadata(Guid documentId, string metadataName, string metadataValue, string oldMetadataName)
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<bool>();

            var command = new UpdateDocumentMetadataActionCommand(documentId, metadataName, metadataValue, oldMetadataName);
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse;
        }

        [Route("UpdateDocumentCategoryRelation")]
        [Authorize(Policy = Constants.Authorization.Policies.ADMIN)]
        public async Task<IActionResponse<bool>> UpdateDocumentCategoryRelation(Guid documentId, Guid newCategoryId)
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<bool>();
            var feedbackMessageFacktory = container.Resolve<IFeedbackMessageFactory>();

            var command = new UpdateDocumentCategoryRelationCommand(documentId, newCategoryId, feedbackMessageFacktory);
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse;
        }
        #endregion
    }
}
