using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unity;
using XperiCad.Common.Infrastructure.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Services;
using XperiCad.DigitalDrawingStore.Web.API.Commands;

namespace XperiCad.DigitalDrawingStore.Web.API.Controllers
{
    [Route("api/Home")]
    [Authorize(Policy = Constants.Authorization.Policies.USER)]
    public class HomeController : Controller
    {
        #region PUT
        [Route("")]
        public async Task<IActionResponse<bool>> UpdateLanguage(string languageCodeString)
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<bool>();

            var command = new UpdateLanguageActionCommand(languageCodeString);
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse;
        }
        #endregion
    }
}
