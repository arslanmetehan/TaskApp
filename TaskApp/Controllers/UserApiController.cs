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
		private readonly IDirectMessageService _directMessageService;

        public UserApiController(IUserService userService, IMissionService missionService, IDirectMessageService directMessageService)
        {
            _userService = userService;
            _missionService = missionService;
			_directMessageService = directMessageService;

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

		[HttpGet]
		[Route(nameof(GetTargetUsers))]
		public ActionResult<ApiResponse<List<UserModel>>> GetTargetUsers()
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);
				if (onlineUser == null)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				
				
				var onlineUserId = onlineUser.Id;
				var users = this._userService.GetTargetUsersByOnlineUserId(onlineUserId);

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
				if (onlineUser == null)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}

				var onlineUserId = onlineUser.Id;
				var users = this._userService.GetTargetUsersByOnlineUserId(onlineUserId);
				var userIds = users.Select(u => u.Id);
				var userIdSet = new HashSet<int>(userIds);
				
				// DONE: targetUsers Id lerini HashSet e koy
				// DONE: sadecee mission dönmeli, mission ın userId si targetUsers HashSet inde varsa ekle, yoksa ekleme
				
				List<MissionModel> targetUserMissions = new List<MissionModel>();
				
				var missions = this._missionService.GetAll().ToList();
				for(int i=0;i< missions.Count;i++)
				{
					if(userIdSet.Contains(missions[i].UserId))
					{
						targetUserMissions.Add(missions[i]);
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
				// DONE _userService.GetUsersExcept i kullanacak şekilde düzelt
				var user = this._userService.GetOnlineUser(this.HttpContext);
				var onlineUserId = user.Id;
				var targetUsers = this._userService.GetTargetUsersByOnlineUserId(onlineUserId);
				var allUsers = this._userService.GetUsersExcept(onlineUserId);
	
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
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);
				if (onlineUser == null)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				var onlineUserId = onlineUser.Id;
				var users = this._userService.GetFollowerUsersByOnlineUserId(onlineUserId);

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
				var usernameControl = _userService.UsernameCounter(model.Username);
				if(usernameControl >= 1)
				{
					return Json(ApiResponse<UserModel>.WithError("This Username has already exist !"));
				}
				newUser.Email = model.Email;
				newUser.Password = model.Password;
				newUser.BirthYear = model.BirthYear;

				//DONE Username ile aynı user var mı diye repository bakmalı. SELECT COUNT(*) FROM User WHERE Username = @Username


				this._userService.AddNewUser(newUser);
				result = this._userService.GetById(newUser.Id);

				return Json(ApiResponse<UserModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<UserModel>.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(Follow))]
		public ActionResult<ApiResponse> Follow([FromBody] int targetId)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);
				if (onlineUser == null)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				var onlineUserId = onlineUser.Id;
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
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);
				if (onlineUser == null)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				var onlineUserId = onlineUser.Id;
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
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineUser == null)
				{
					return Json(ApiResponse.WithError("Not Authorized"));
				}

				var user = onlineUser.ToEntity();
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

		// DONE: isimlendirmelerde düzeltme gerekli
		[HttpGet]
		[Route(nameof(GetMessages))]
		public ActionResult<ApiResponse<List<DirectMessageModel>>> GetMessages(int otherUserId)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);
				if (onlineUser == null)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				
				int senderId = onlineUser.Id;
				var messages = this._directMessageService.GetMessagesBySenderAndReceiverId(senderId, otherUserId);



				var response = ApiResponse<List<DirectMessageModel>>.WithSuccess(messages);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<UserModel>>.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(CreateMessage))]
		public ActionResult<ApiResponse<DirectMessageModel>> CreateMessage([FromBody]CreateDirectMessageModel model)
		{
			// DONE: onlineUserId ile senderId aynı olmak zorunda
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);
				if (onlineUser == null)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				DirectMessageModel result = null;
				if(model.SenderId != onlineUser.Id)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				var newMessage = new DirectMessage();
				newMessage.SenderId = model.SenderId;
				newMessage.ReceiverId = model.ReceiverId;
				newMessage.MessageContent = model.MessageContent;
				newMessage.IsDeleted = 0;
				
				this._directMessageService.AddNewMessage(newMessage);
				result = this._directMessageService.GetById(newMessage.Id);

				return Json(ApiResponse<DirectMessageModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<UserModel>.WithError(exp.ToString()));
			}
		}

		[HttpDelete]
		[Route(nameof(DeleteMessage))]
		public ActionResult<ApiResponse> DeleteMessage([FromBody] int messageId)
		{
			try
			{
				var user = this._userService.GetOnlineUser(this.HttpContext);
				if(user == null)
				{
					return Json(ApiResponse.WithError("You cant delete this message !"));
				}

				// DONE: sadece kendi mesajımı silebilirim

				var message = this._directMessageService.GetById(messageId).ToEntity();
				if(message.SenderId != user.Id)
				{
					return Json(ApiResponse.WithError("You cant delete this message !"));
				}
				message.MessageContent = "Bu Mesaj Silindi !";
				message.IsDeleted = 1;
				this._directMessageService.UpdateMessage(message);

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetNewMessages))]
		public ActionResult<ApiResponse<List<DirectMessageModel>>> GetNewMessages(int lastMessageId)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);
				if (onlineUser == null)
				{
					return Json(ApiResponse.WithError("You cant delete this message !"));
				}
				
				int receiverId = onlineUser.Id;
				var message = this._directMessageService.GetById(lastMessageId);

				var messages = this._directMessageService.GetNewMessages(lastMessageId, message.SenderId, receiverId);

				var response = ApiResponse<List<DirectMessageModel>>.WithSuccess(messages);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<UserModel>>.WithError(exp.ToString()));
			}
		}
	}
}
