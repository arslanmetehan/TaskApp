using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskApp.Models;
using TaskApp.Services;

namespace TaskApp.Controllers
{
    public class HomeController : Controller
    {
		private readonly IServices services;

		public HomeController(IServices services)
		{
			this.services = services;
		}

		public IActionResult Index()
		{
			var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, "Home");
			return View(model);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
