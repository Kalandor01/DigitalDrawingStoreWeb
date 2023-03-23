using DigitalDrawingStore.Web.UI.Views.Shared;
using Microsoft.AspNetCore.Mvc;
using XperiCad.DigitalDrawingStore.Web.API.Authorization;

namespace DigitalDrawingStore.Web.UI.Controllers
{
	public class BaseController : Controller
    {
        protected SharedViewModel SharedViewModel { get; }

		public BaseController()
		{
			SharedViewModel = new SharedViewModel(new SecurityFacade());
		}

		protected IActionResult SharedView()
		{
			return View(SharedViewModel);
		}
    }
}
