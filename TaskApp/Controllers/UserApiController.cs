using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Helpers;
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
				var user = this._userService.GetOnlineUser(this.HttpContext);
				users.Remove(user);
				var response = ApiResponse<List<UserModel>>.WithSuccess(users);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<UserModel>>.WithError(exp.ToString()));
			}
		}
		[HttpGet]
		[Route(nameof(GetTargetUsers))]
		public ActionResult<ApiResponse<List<UserModel>>> GetTargetUsers()
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);
				
				var onlineUserId = onlineUser.Id;
				var users = this._userService.GetTargetUsersByOnlineUserId(onlineUserId);
	
				for(int i=0; i<users.Count;i++)
				{
					if (users[i].Id == onlineUserId)
					{
						users.RemoveAt(i);
					}
				}
		
				var response = ApiResponse<List<UserModel>>.WithSuccess(users);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<UserModel>>.WithError(exp.ToString()));
			}
		}
		[HttpGet]
		[Route(nameof(GetTargetUserMissions))]
		public ActionResult<ApiResponse<List<MissionModel>>> GetTargetUserMissions()
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				var onlineUserId = onlineUser.Id;
				var users = this._userService.GetTargetUsersByOnlineUserId(onlineUserId);

				for (int i = 0; i < users.Count; i++)
				{
					if (users[i].Id == onlineUserId)
					{
						users.RemoveAt(i);
					}
				}
				List<MissionModel> targetUserMissions = new List<MissionModel>();
				var missions = this._missionService.GetAll().ToList();
				for(int i=0;i<users.Count;i++)
				{
					for(int k=0;k<missions.Count;k++)
					{
						if(users[i].Id == missions[k].UserId)
						{
							missions[k].MissionUsername = users[i].Username;
							targetUserMissions.Add(missions[k]);
						}
					}
				}

				var response = ApiResponse<List<MissionModel>>.WithSuccess(targetUserMissions);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<UserModel>>.WithError(exp.ToString()));
			}
		}
		[HttpGet]
		[Route(nameof(GetTargetUsersById))]
		public ActionResult<ApiResponse<List<UserModel>>> GetTargetUsersById(int userId)
		{
			try
			{
				
				
				//var users = this._userService.GetTargetUsersByOnlineUserId(onlineUserId);
				var users = this._userService.GetTargetUsersById(userId);


				
				var response = ApiResponse<List<UserModel>>.WithSuccess(users);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<UserModel>>.WithError(exp.ToString()));
			}
		}
		[HttpGet]
		[Route(nameof(GetFollowerUsersById))]
		public ActionResult<ApiResponse<List<UserModel>>> GetFollowerUsersById(int userId)
		{
			try
			{
				var users = this._userService.GetFollowerUsersById(userId);
				var response = ApiResponse<List<UserModel>>.WithSuccess(users);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<UserModel>>.WithError(exp.ToString()));
			}
		}
		[HttpGet]
		[Route(nameof(GetNotTargetUsers))]
		public ActionResult<ApiResponse<List<UserModel>>> GetNotTargetUsers()
		{
			try
			{
				
				var user = this._userService.GetOnlineUser(this.HttpContext);
				var onlineUserId = user.Id;
				var targetUsers = this._userService.GetTargetUsersByOnlineUserId(onlineUserId);
				var allUsers = this._userService.GetAllUsers();


	
				for (int i = 0; i < allUsers.Count; i++)
				{
					if (allUsers[i].Id == onlineUserId)
					{
						allUsers.RemoveAt(i);
						break;
					}
				}
				bool targetCheck = false;
				List<UserModel> notTargetUsers = new List<UserModel>();
				for(int i=0;i< allUsers.Count;i++)
				{
					for(int b=0;b<targetUsers.Count;b++)
					{
						
						if (allUsers[i].Id == targetUsers[b].Id)
						{
							targetCheck = true;
							continue;
							
						}

					}
					if(targetCheck==false)
					{
						notTargetUsers.Add(allUsers[i]);
						
					}
					targetCheck = false;
				}
				var response = ApiResponse<List<UserModel>>.WithSuccess(notTargetUsers);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<UserModel>>.WithError(exp.ToString()));
			}
		}
		[HttpGet]
		[Route(nameof(GetFollowerUsers))]
		public ActionResult<ApiResponse<List<UserModel>>> GetFollowerUsers()
		{
			try
			{
				var user = this._userService.GetOnlineUser(this.HttpContext);
				var onlineUserId = user.Id;
				var users = this._userService.GetFollowerUsersByOnlineUserId(onlineUserId);
				users.Remove(user);
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
				if (model.Username == null || model.Username == "")
				{
					return Json(ApiResponse<UserModel>.WithError("Username is required !"));
				}
				if(model.Email == null || model.Email == "")
				{
					return Json(ApiResponse<UserModel>.WithError("Email is required !"));
				}
				if (model.Password == null || model.Password == "")
				{
					return Json(ApiResponse<UserModel>.WithError("Password is required !"));
				}
				UserModel result = null;

				var newUser = new User();
				newUser.Username = model.Username;
				newUser.Email = model.Email;
				newUser.Password = model.Password;
				newUser.BirthYear = Convert.ToInt32(model.BirthYear);
				
				var users = _userService.GetAllUsers();
				foreach (var user in users)
				{
					if (user.Username == newUser.Username)
					{
						
						return Json(ApiResponse<UserModel>.WithError("This Username has already exist !"));
					}
				}
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
		[Route(nameof(Follow))]
		public ActionResult<ApiResponse> Follow([FromBody] int targetId)
		{
			try
			{
				var user = this._userService.GetOnlineUser(this.HttpContext);
				var onlineUserId = user.Id;
				this._userService.FollowAnotherUser(onlineUserId, targetId);

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}
		[HttpDelete]
		[Route(nameof(Unfollow))]
		public ActionResult<ApiResponse> Unfollow([FromBody] int targetId)
		{
			try
			{
				var user = this._userService.GetOnlineUser(this.HttpContext);
				var onlineUserId = user.Id;
				this._userService.UnFollowAnotherUser(onlineUserId, targetId);

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}
		[HttpPut]
		[Route(nameof(UpdateUser))]
		public ActionResult<ApiResponse<UserModel>> UpdateUser([FromBody]UpdateUserModel model)
		{
			try
			{
				var user = this._userService.GetOnlineUser(this.HttpContext).ToEntity();
				user.Email = model.Email;
				user.Username = model.Username;
				user.Password = model.Password;
				user.BirthYear = model.BirthYear;
				
				this._userService.UpdateUser(user);
				
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
