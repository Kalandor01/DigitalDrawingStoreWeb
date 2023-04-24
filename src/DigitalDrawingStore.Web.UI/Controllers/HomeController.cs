using DigitalDrawingStore.Web.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using XperiCad.Common.Infrastructure.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Services;
using HomeApiController = XperiCad.DigitalDrawingStore.Web.API.Controllers.HomeController;

namespace DigitalDrawingStore.Web.UI.Controllers
{
    [Authorize(Policy = Constants.Autorization.Policies.USER)]
    public class HomeController : BaseController
    {
        #region Fields
        private readonly HomeApiController _homeController;
        #endregion

        #region ctor
        public HomeController()
        {
            // TODO: implement DI for ApiModule
            _homeController = new HomeApiController();
        }
        #endregion

        #region PUT
        [Route("ChangeLanguage")]
        public async Task<IActionResponse<bool>> ChangeLanguage(string languageString)
        {
            return await _homeController.UpdateLanguage(languageString);
        }

        [Route("GetEmptyCategoryText")]
        public async Task<IActionResponse<string?>> GetEmptyCategoryText()
        {
            return await _homeController.GetTranslationStringText(CultureProperty.EMPTY_CATEGORY_TEXT);
        }

        [Route("GetOpenEditorText")]
        public async Task<IActionResponse<string?>> GetOpenEditorText()
        {
            return await _homeController.GetTranslationStringText(CultureProperty.OPEN_EDITOR_TEXT);
        }

        [Route("GetCloseEditorText")]
        public async Task<IActionResponse<string?>> GetCloseEditorText()
        {
            return await _homeController.GetTranslationStringText(CultureProperty.CLOSE_EDITOR_TEXT);
        }

        [Route("GetTopLeftWatermarkText")]
        public async Task<IActionResponse<string?>> GetTopLeftWatermarkText()
        {
            return await _homeController.GetTranslationStringText(CultureProperty.TOP_LEFT_WATERMARK_TEXT);
        }

        [Route("GetTopRightWatermarkText")]
        public async Task<IActionResponse<string?>> GetTopRightWatermarkText()
        {
            return await _homeController.GetTranslationStringText(CultureProperty.TOP_RIGHT_WATERMARK_TEXT);
        }

        [Route("GetBottomLeftWatermarkText")]
        public async Task<IActionResponse<string?>> GetBottomLeftWatermarkText()
        {
            return await _homeController.GetTranslationStringText(CultureProperty.BOTTOM_LEFT_WATERMARK_TEXT);
        }

        [Route("GetBottomRightWatermarkText")]
        public async Task<IActionResponse<string?>> GetBottomRightWatermarkText()
        {
            return await _homeController.GetTranslationStringText(CultureProperty.BOTTOM_RIGHT_WATERMARK_TEXT);
        }
        #endregion

        public IActionResult Index()
        {
            var title = CultureService.GetPropertyNameTranslation(CultureProperty.HOME_PAGE_NAME, CultureService.GetSelectedCulture());
            return SharedView("[Főoldal]", title);
        }

        public IActionResult Privacy()
        {
            var title = CultureService.GetPropertyNameTranslation(CultureProperty.PRIVACY_PAGE_NAME, CultureService.GetSelectedCulture());
            return SharedView("[Adatvédelmi irányelvek]", title);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewData["Title"] = CultureService.GetPropertyNameTranslation(CultureProperty.ERROR_PAGE_NAME, CultureService.GetSelectedCulture()) ?? "[Hiba]";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}