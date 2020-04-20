using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TaskApp.Entities;
using TaskApp.Models;
using TaskApp.Persistence;

namespace TaskApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogRepository _logRepository;
        private readonly IMissionRepository _missionRepository;

        public UserService(IUserRepository userRepository, ILogRepository logRepository, IMissionRepository missionRepository)
        {
            this._userRepository = userRepository;
            this._logRepository = logRepository;
            this._missionRepository = missionRepository;
        }
        public void AddNewUser(User user)
        {
            this._userRepository.Insert(user);
            this._logRepository.Log(Enums.LogType.Info, $"Inserted New User : {user.Username}");
            // this._emailService.SendEmail("fsdf", "dsfsdfsdfdsf");
        }

        public void Delete(int id)
        {
            var user = this._userRepository.GetById(id);
            this._userRepository.Delete(id);
            this._logRepository.Log(Enums.LogType.Info, $"Deleted User : {user.Username}");
        }
        public UserModel GetById(int id)
        {
            return this._userRepository.GetById(id);
        }
        public List<UserModel> GetAllUsers()
        {
            var users = this._userRepository.GetAll().ToList();
           /* var missions = this._missionRepository.GetAll().Select(mission => new MissionModel(mission)).ToList();

            users.ForEach(u =>
            {
                u.GroupName = missions.FirstOrDefault(ug => ug.Id == u.GroupId)?.Name;
            });*/

            return users;
        }
        public UserModel GetOnlineUser(HttpContext httpContext)
        {
            int? onlineUserId = httpContext.Session.GetInt32("onlineUserId");
            if (!onlineUserId.HasValue)
            {
                return null;
            }

            return this._userRepository.GetById(onlineUserId.Value);
        }

        public void Logout(HttpContext httpContext)
        {
            httpContext.Session.Remove("onlineUserId");
        }

        public bool TryLogin(UserLoginModel loginData, HttpContext httpContext)
        {
            int userId = this._userRepository.GetUserIdByLogin(loginData.Username, loginData.Password);

            if (userId > 0)
            {
                httpContext.Session.SetInt32("onlineUserId", userId);
                return true;
            }

            return false;
        }
    }
}
