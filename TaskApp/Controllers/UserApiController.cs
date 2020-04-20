using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;
using TaskApp.Services;

namespace TaskApp.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserApiController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMissionService _missionService;

        public UserApiController(IUserService userService, IMissionService missionService)
        {
            _userService = userService;
            _missionService = missionService;
        }
        [HttpGet]
		[Route(nameof(GetActiveUsers))]
		public ActionResult<ApiResponse<List<UserModel>>> GetActiveUsers()
		{
			try
			{
				var users = this._userService.GetAllUsers();

				var response = ApiResponse<List<UserModel>>.WithSuccess(users);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<UserModel>>.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(CreateUser))]
		public ActionResult<ApiResponse<UserModel>> CreateUser([FromBody]CreateUserModel model)
		{
			try
			{
				UserModel result = null;

				var newUser = new User();
				newUser.Username = model.Username;
				newUser.Email = model.Email;
				newUser.Password = model.Password;
				
				this._userService.AddNewUser(newUser);
				result = this._userService.GetById(newUser.Id);

				return Json(ApiResponse<UserModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<UserModel>.WithError(exp.ToString()));
			}
		}
		[HttpDelete]
		[Route(nameof(DeleteUser))]
		public ActionResult<ApiResponse> DeleteUser([FromBody] int userId)
		{
			try
			{
				this._userService.Delete(userId);

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(Login))]
		public ActionResult<ApiResponse> Login([FromBody]UserLoginModel model)
		{
			try
			{
				if (!this._userService.TryLogin(model, this.HttpContext))
				{
					return Json(ApiResponse.WithError("Invalid Username or Password!"));
				}

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetOnlineUser))]
		public ActionResult<ApiResponse<UserModel>> GetOnlineUser()
		{
			try
			{
				var user = this._userService.GetOnlineUser(this.HttpContext);

				return Json(ApiResponse<UserModel>.WithSuccess(user));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<UserModel>.WithError(exp.ToString()));
			}
		}
	}
}
