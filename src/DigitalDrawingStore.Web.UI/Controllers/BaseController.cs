﻿using DigitalDrawingStore.Web.UI.Views.Shared;
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

		protected IActionResult SharedView(string defaultTitle, string? translatedTitle = null)
		{
            ViewData["Title"] = translatedTitle ?? defaultTitle;
            return View(SharedViewModel);
		}
    }
}
