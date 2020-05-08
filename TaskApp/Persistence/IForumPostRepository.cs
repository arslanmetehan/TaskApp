using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Persistence
{
    public interface IForumPostRepository
    {
        void Insert(ForumPost post);
        void Delete(int id);
        IEnumerable<ForumPostModel> GetPostsByUserId(int userId);
        IEnumerable<ForumPostModel> GetAllPosts();
        ForumPostModel GetById(int id);
        IEnumerable<ForumPost> GetByUserId(int id);
    }
}
