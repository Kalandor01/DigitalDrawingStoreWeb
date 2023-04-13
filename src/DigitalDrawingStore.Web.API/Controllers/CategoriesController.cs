using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unity;
using XperiCad.Common.Infrastructure.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Services;
using XperiCad.DigitalDrawingStore.Web.API.Commands;
using XperiCad.DigitalDrawingStore.Web.API.DTO;
using XperiCad.DigitalDrawingStore.Web.API.Extensions;

namespace XperiCad.DigitalDrawingStore.Web.API.Controllers
{
    [Route("api/Categories")]
    [Authorize(Policy = Constants.Authorization.Policies.USER)]
    public class CategoriesController : Controller
    {
        #region GET
        [Route("")]
        public async Task<JsonResponse<IEnumerable<DocumentCategory>>> Categories(string searchText)
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<IEnumerable<DocumentCategory>>();

            var getDocumentsCommand = new GetDocumentCategoriesActionCommand(searchText);
            commandInvoker.AddCommand(getDocumentsCommand);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse.ToJsonResponse();
        }

        public async Task<JsonResponse<DocumentCategoryEntities>> Metadata(Guid categoryId)
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<DocumentCategoryEntities>();

            var command = new GetDocumentCategoryEntitiesActionCommand(categoryId);
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse.ToJsonResponse();
        }
        #endregion

        #region PUT
        [Route("")]
        [Authorize(Policy = Constants.Authorization.Policies.ADMIN)]
        public async Task<IActionResponse<bool>> UpdateCategory(Guid categoryId, string categoryName, string isDesigned)
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<bool>();

            var command = new UpdateDocumentCategoryAttributesActionCommand(categoryId, categoryName, isDesigned);
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse;
        }

        // TODO: investigate and fix this
        [Route("")]
        [Authorize(Policy = Constants.Authorization.Policies.ADMIN)]
        public async Task<IActionResponse<bool>> UpdateMetadata(string result)
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<bool>();

            var command = new UpdateDocumentCategoryEntitiesActionCommand(result);
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse;
        }
        #endregion
    }
}
