using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Persistence
{
    public interface IUserRepository
    {
        void Insert(User user);
        void Delete(int id);
        IEnumerable<UserModel> GetAll();
        UserModel GetById(int id);
        int GetUserIdByLogin(string username, string password);
        void FollowTargetUser(int followerId, int targetId);
        void UnfollowTargetUser(int followerId, int targetId);
        IEnumerable<UserModel> GetTargetUsersByOnlineUserId(int onlineUserId);
        IEnumerable<UserModel> GetFollowerUsersByOnlineUserId(int onlineUserId);
        IEnumerable<UserModel> GetNotTargetUsersByOnlineUserId(int onlineUserId);
        void UpdateUser(User user);
        IEnumerable<UserModel> GetTargetUsersById(int userId);
        IEnumerable<UserModel> GetFollowerUsersById(int userId);
    }
}
