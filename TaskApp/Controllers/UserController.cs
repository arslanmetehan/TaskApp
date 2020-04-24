using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Models;
using TaskApp.Services;

namespace TaskApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IServices services;

        public UserController(IServices services)
        {
            this.services = services;
            
        }

        [HttpGet]
        public ActionResult Register()
        {
            var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.Register));
            return View(model);
        }
        public ActionResult MyMissions()
        {
            var user = this.services.UserService.GetOnlineUser(this.HttpContext);
            if(user == null)
            {
                return RedirectToAction("Login", "User");
            }
            var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.MyMissions));
            return View(model);
        }
        public ActionResult MyMissionDetail(int id)
        {
            var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.MyMissionDetail));
            return View(model);
        }
        public IActionResult Login()
        {
            var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.Login));
            return View(model);
        }
        public IActionResult Logout()
        {
            this.services.UserService.Logout(this.HttpContext);

            return RedirectToAction("Index", "Home");
        }
    }
}
