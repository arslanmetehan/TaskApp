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
            var missionId = id;
           
            var model = this.services.ViewService.CreateViewModel<MissionDetailViewModel>(this.HttpContext, nameof(this.MyMissionDetail));
            model.MissionId = id.ToString();
        
            return View(model);
        }
        public ActionResult Profile(int id)
        {
            var userId = id;

            var model = this.services.ViewService.CreateViewModel<UserViewModel>(this.HttpContext, nameof(this.Profile));
            model.UserId = id;
            var user = this.services.UserService.GetById(id);
           
            model.Username = user.Username;

            return View(model);
        }
        public IActionResult Login()
        {
            var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.Login));
            return View(model);
        }
        public IActionResult UserList()
        {
            var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.UserList));
            return View(model);
        }
        public IActionResult MyProfile()
        {
            var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.UserList));
            return View(model);
        }
        public ActionResult NewsFeed()
        {
            var model = this.services.ViewService.CreateViewModel<MissionDetailViewModel>(this.HttpContext, nameof(this.NewsFeed));
            return View(model);
        }
        public ActionResult DirectMessage()
        {
            var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.DirectMessage));
            return View(model);
        }

        public IActionResult Logout()
        {
            this.services.UserService.Logout(this.HttpContext);

            return RedirectToAction("Index", "Home");
        }
    }
}
