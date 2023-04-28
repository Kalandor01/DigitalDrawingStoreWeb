using DigitalDrawingStore.Web.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using XperiCad.Common.Infrastructure.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.BL.Impl.Services;
using HomeApiController = XperiCad.DigitalDrawingStore.Web.API.Controllers.HomeController;
using BaseApiController = XperiCad.DigitalDrawingStore.Web.API.Controllers.BaseController;

namespace DigitalDrawingStore.Web.UI.Controllers
{
    [Authorize(Policy = Constants.Autorization.Policies.USER)]
    public class HomeController : BaseController
    {
        #region Fields
        private readonly HomeApiController _homeController;
        private readonly BaseApiController _baseController;
        #endregion

        #region ctor
        public HomeController()
        {
            // TODO: implement DI for ApiModule
            _homeController = new HomeApiController();
            _baseController = new BaseApiController();
        }
        #endregion

        #region PUT
        [Route("ChangeLanguage")]
        public async Task<IActionResponse<bool>> ChangeLanguage(string languageString)
        {
            return await _homeController.UpdateLanguage(languageString);
        }

        [Route("GetEditorTexts")]
        public async Task<IActionResponse<Dictionary<string, string?>>> GetEditorTexts()
        {
            return await _baseController.GetTranslationStringTextDictionary(new Dictionary<string, CultureProperty>
            {
                ["open"] = CultureProperty.OPEN_EDITOR_TEXT,
                ["close"] = CultureProperty.CLOSE_EDITOR_TEXT
            });
        }

        [Route("GetCommonTexts")]
        public async Task<IActionResponse<Dictionary<string, string?>>> GetCommonTexts()
        {
            return await _baseController.GetTranslationStringTextDictionary(new Dictionary<string, CultureProperty>
            {
                ["emptyCategory"] = CultureProperty.EMPTY_CATEGORY_TEXT,
                ["topLeft"] = CultureProperty.TOP_LEFT_WATERMARK_TEXT,
                ["topRight"] = CultureProperty.TOP_RIGHT_WATERMARK_TEXT,
                ["bottomLeft"] = CultureProperty.BOTTOM_LEFT_WATERMARK_TEXT,
                ["bottomRight"] = CultureProperty.BOTTOM_RIGHT_WATERMARK_TEXT,

                ["unknownErrorTitle"] = CultureProperty.UNKNOWN_ERROR_TITLE_TEXT,
                ["unknownError"] = CultureProperty.UNKNOWN_ERROR_TEXT,
                ["errorTitle"] = CultureProperty.ERROR_TITLE_TEXT,
                ["warningTitle"] = CultureProperty.WARNING_TITLE_TEXT,
                ["informationTitle"] = CultureProperty.INFORMATION_TITLE_TEXT,
                ["attributesNotFound"] = CultureProperty.ATTRIBUTES_NOT_FOUND_TEXT,
                ["changesWillBeLost"] = CultureProperty.CHANGES_WILL_BE_LOST_TEXT,
                ["dialogEdit"] = CultureProperty.DIALOG_EDIT_TEXT,
                ["dialogSave"] = CultureProperty.DIALOG_SAVE_TEXT,
                ["dialogCancel"] = CultureProperty.DIALOG_CANCEL_TEXT,
                ["dialogYesCheckValue"] = CultureProperty.DIALOG_YES_CHECK_VALUE,
            });
        }

        [Route("GetCategoriesTexts")]
        public async Task<IActionResponse<Dictionary<string, string?>>> GetCategoriesTexts()
        {
            return await _baseController.GetTranslationStringTextDictionary(new Dictionary<string, CultureProperty>
            {
                ["categoriesTableName"] = CultureProperty.CATEGORIES_TABLE_NAME_TEXT,
                ["documentCategoriesColumn"] = CultureProperty.DOCUMENT_CATEGORIES_COLUMN_TEXT,
                ["isDesignedColumn"] = CultureProperty.IS_DESIGNED_COLUMN_TEXT,
                ["tableActionsColumn"] = CultureProperty.TABLE_ACTIONS_COLUMN_TEXT,
                ["isDesignedYes"] = CultureProperty.IS_DESIGNED_YES_TEXT,
                ["isDesignedNo"] = CultureProperty.IS_DESIGNED_NO_TEXT,
                ["editCategory"] = CultureProperty.EDIT_CATEGORY_TEXT,
                ["addedMetadataColumn"] = CultureProperty.ADDED_METADATA_COLUMN_TEXT,
                ["notAddedMetadataColumn"] = CultureProperty.NOT_ADDED_METADATA_COLUMN_TEXT,
                ["closeEditorWindow"] = CultureProperty.CLOSE_EDITOR_WINDOW_TEXT,
                ["saveEditorWindow"] = CultureProperty.SAVE_EDITOR_WINDOW_TEXT
            });
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