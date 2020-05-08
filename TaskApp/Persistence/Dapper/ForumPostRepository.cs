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
    public class ForumPostRepository : BaseSqliteRepository, IForumPostRepository
    {
        public void Insert(ForumPost post)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("INSERT INTO ForumPost (PostContent, UserId) VALUES(@PostContent, @UserId)", post);
                post.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
            }
        }
        public void Delete(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("DELETE FROM ForumPost WHERE Id = @Id", new { Id = id });
            }
        }

        public IEnumerable<ForumPostModel> GetPostsByUserId(int userId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
 
                return dbConnection.Query<ForumPostModel>("SELECT f.*, u.Username as Username FROM ForumPost f, User u WHERE u.Id = f.UserId AND UserId = @UserId", new { UserId = userId });
            }
        }

        public ForumPostModel GetById(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.QuerySingle<ForumPostModel>("SELECT f.*, u.Username as Username FROM ForumPost f, User u WHERE u.Id = f.UserId AND f.Id = @Id", new { Id = id });
            }
        }
        public IEnumerable<ForumPostModel> GetAllPosts()
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.Query<ForumPostModel>("SELECT f.*, u.Username as Username FROM ForumPost f, User u WHERE u.Id = f.UserId");
            }
        }
        public IEnumerable<ForumPost> GetByUserId(int postUserId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.Query<ForumPost>("SELECT * FROM ForumPost WHERE userId = @userId", new { userId = postUserId });
            }
        }
    }
}
