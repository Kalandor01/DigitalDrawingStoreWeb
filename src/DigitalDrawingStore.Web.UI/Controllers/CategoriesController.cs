using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XperiCad.Common.Infrastructure.Behaviours.Commands;
using XperiCad.DigitalDrawingStore.Web.API.DTO;
using CategoriesApiController = XperiCad.DigitalDrawingStore.Web.API.Controllers.CategoriesController;

namespace DigitalDrawingStore.Web.UI.Controllers
{
    [Route("Categories")]
    [Authorize(Policy = Constants.Autorization.Policies.USER)]
    public class CategoriesController : Controller
    {
        #region Fields
        private readonly CategoriesApiController _categoriesController;
        #endregion

        #region ctor
        public CategoriesController()
        {
            // TODO: implement DI for ApiModule
            _categoriesController = new CategoriesApiController();

        }
        #endregion

        #region GET
        [Route("")]
        public async Task<JsonResponse<IEnumerable<DocumentCategory>>> Categories(string searchText)
        {
            return await _categoriesController.Categories(searchText);
        }

        [Route("Metadata")]
        public async Task<JsonResponse<DocumentCategoryEntities>> Metadata(Guid categoryId)
        {
            return await _categoriesController.Metadata(categoryId);
        }
        #endregion

        #region PUT
        [Route("UpdateCategory")]
        [Authorize(Policy = Constants.Autorization.Policies.ADMIN)]
        public async Task<IActionResponse<bool>> UpdateCategory(Guid categoryId, string categoryName, string isDesigned)
        {
            return await _categoriesController.UpdateCategory(categoryId, categoryName, isDesigned);
        }

        // TODO: investigate and fix id issue
        [Route("UpdateMetadata")]
        [Authorize(Policy = Constants.Autorization.Policies.ADMIN)]
        public async Task<IActionResponse<bool>> UpdateMetadata(string result)
        {
            return await _categoriesController.UpdateMetadata(result);
        }
        #endregion
    }
}
