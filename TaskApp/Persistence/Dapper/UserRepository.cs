using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                dbConnection.Execute("INSERT INTO User (Username, Password, Email) VALUES(@Username, @Password, @Email)", user);
                user.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
            }
        }
        public void Delete(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("DELETE FROM User WHERE Id = @Id", new { Id = id });
            }
        }

        public IEnumerable<UserModel> GetAll()
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.Query<UserModel>("SELECT u.*, m.UserId as Id FROM User u, Mission m WHERE u.Id = m.UserId");
            }
        }

        public UserModel GetById(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.QuerySingle<UserModel>("SELECT * FROM User WHERE Id = @Id", new { Id = id });
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
