using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Services
{
    public interface IUserService
    {
		void AddNewUser(User user);
		void UpdateUser(User user);
		UserModel GetById(int id);
		void Logout(HttpContext httpContext);
		bool TryLogin(UserLoginModel loginData, HttpContext httpContext);
		UserModel GetOnlineUser(HttpContext httpContext);
		List<UserModel> GetAllUsers();
		int UsernameCounter(string username);
		List<UserModel> GetUsersExcept(int userId);
		void FollowAnotherUser(int followerId,int targerId);
		void UnFollowAnotherUser(int followerId, int targetId);
		List<UserModel> GetTargetUsersByOnlineUserId(int onlineUserId);
		List<UserModel> GetFollowerUsersByOnlineUserId(int onlineUserId);
		List<UserModel> GetTargetUsersById(int userId);
		List<UserModel> GetFollowerUsersById(int userId);
		List<UserModel> GetNotTargetUsersByOnlineUserId(int onlineUserId);
	}
}
