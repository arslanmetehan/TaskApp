using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Persistence.Dapper
{
    public class UserRepository : BaseSqliteRepository, IUserRepository
    {
        public void Insert(User user)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("INSERT INTO User (Username, Password, Email, BirthYear) VALUES(@Username, @Password, @Email, @BirthYear)", user);
                user.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
            }
        }
        public void UpdateUser(User user)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Query("UPDATE User SET Username = @Username, Password = @Password, Email = @Email, BirthYear = @BirthYear WHERE Id = @Id" , user);
            }
        }
        public void FollowTargetUser(int followerId, int targetId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                // DONE: parametreler düzeltilmeli
                dbConnection.Execute("INSERT INTO Follow (FollowerUserId, TargetUserId) VALUES(@FollowerUserId, @TargetUserId)", new { FollowerUserId = followerId, TargetUserId = targetId });
               
            }
        }
        public void UnfollowTargetUser(int followerId, int targetId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                // DONE: parametreler düzeltilmeli
                dbConnection.Execute("DELETE From Follow WHERE FollowerUserId = @FollowerUserId AND TargetUserId = @TargetUserId", new { FollowerUserId = followerId, TargetUserId = targetId });
            }
        }

        public IEnumerable<UserModel> GetAll()
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.Query<UserModel>("SELECT * FROM User");
            }
        }
        public IEnumerable<UserModel> GetUsersExcept(int userId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.Query<UserModel>("SELECT * FROM User WHERE Id != @Id", new { Id = userId });
            }
        }
        public IEnumerable<UserModel> GetTargetUsersByOnlineUserId(int onlineUserId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                //return dbConnection.Query<UserModel>("SELECT * FROM User");
                return dbConnection.Query<UserModel>("SELECT u.* FROM User u, Follow f WHERE f.FollowerUserId = @OnlineUserId AND u.Id = f.TargetUserId", new { OnlineUserId = onlineUserId });
            }
        }
        public IEnumerable<UserModel> GetTargetUsersById(int userId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                //return dbConnection.Query<UserModel>("SELECT * FROM User");
                return dbConnection.Query<UserModel>("SELECT u.* FROM User u, Follow f WHERE f.FollowerUserId = @OnlineUserId AND u.Id = f.TargetUserId", new { OnlineUserId = userId });
            }
        }
        public IEnumerable<UserModel> GetNotTargetUsersByOnlineUserId(int onlineUserId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                //return dbConnection.Query<UserModel>("SELECT * FROM User");
                return dbConnection.Query<UserModel>("SELECT u.* FROM User u, Follow f WHERE f.FollowerUserId != @OnlineUserId AND u.Id = f.TargetUserId", new { OnlineUserId = onlineUserId });
            }
        }
        public IEnumerable<UserModel> GetFollowerUsersByOnlineUserId(int onlineUserId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                //return dbConnection.Query<UserModel>("SELECT * FROM User");
                return dbConnection.Query<UserModel>("SELECT u.* FROM User u, Follow f WHERE f.TargetUserId = @OnlineUserId AND u.Id = f.FollowerUserId", new { OnlineUserId = onlineUserId });
            }
        }
        public IEnumerable<UserModel> GetFollowerUsersById(int userId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                //return dbConnection.Query<UserModel>("SELECT * FROM User");
                return dbConnection.Query<UserModel>("SELECT u.* FROM User u, Follow f WHERE f.TargetUserId = @OnlineUserId AND u.Id = f.FollowerUserId", new { OnlineUserId = userId });
            }
        }

        public UserModel GetById(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.QuerySingle<UserModel>("SELECT * FROM User WHERE Id = @Id", new { Id = id });
            }
        }
        public int UsernameCounter(string username)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.QuerySingle<int>("SELECT COUNT(*) FROM User WHERE Username = @Username", new { Username = username });
            }
        }

        public IEnumerable<User> GetByMissionId(int missionUserId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.Query<User>("SELECT * FROM User WHERE Id = @Id", new { Id = missionUserId });
            }
        }

        public int GetUserIdByLogin(string username, string password)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                int userId = dbConnection.Query<int>("SELECT Id FROM User WHERE Username = @Username AND Password = @Password",
                                    new { Username = username, Password = password }).FirstOrDefault();

                return userId;
            }
        }
    }
}
