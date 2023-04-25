using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unity;
using XperiCad.Common.Infrastructure.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Application;
using XperiCad.DigitalDrawingStore.BL.Impl.Services;
using XperiCad.DigitalDrawingStore.Web.API.Commands.Get;
using XperiCad.DigitalDrawingStore.Web.API.Commands.Set;

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
        
        [Route("")]
        public async Task<IActionResponse<string?>> GetTranslationStringText(CultureProperty culture)
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<string?>();

            var command = new GetTranslationStringActionCommand(culture);
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse;
        }

        [Route("")]
        public async Task<IActionResponse<Dictionary<string, string?>>> GetTranslationStringTextDictionary(IDictionary<string, CultureProperty> propertyDictionary)
        {
            var container = new ContainerFactory().CreateContainer();

            var commandInvokerFactory = container.Resolve<ICommandInvokerFactory>();
            var commandInvoker = commandInvokerFactory.CreateActionCommandInvoker<Dictionary<string, string?>>();

            var command = new GetTranslationStringDictionaryActionCommand(propertyDictionary);
            commandInvoker.AddCommand(command);
            await commandInvoker.ExecuteAllAsync();

            return commandInvoker.ActionResponse;
        }
        #endregion
    }
}
