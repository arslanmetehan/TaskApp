using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;
using TaskApp.Persistence;
using TaskApp.Services;
using TaskApp.Helpers;

namespace TaskApp.Services
{
    public class ForumPostService : IForumPostService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogRepository _logRepository;
        private readonly IForumPostRepository _forumPostRepository;

        public ForumPostService(IUserRepository userRepository, ILogRepository logRepository, IForumPostRepository forumPostRepository)
        {
            this._userRepository = userRepository;
            this._logRepository = logRepository;
            this._forumPostRepository = forumPostRepository;
        }

        public void AddNewPost(ForumPost post)
        {
            this._forumPostRepository.Insert(post);
            this._logRepository.Log(Enums.LogType.Info, $"Inserted New Post : {post.PostContent}");
        }

        public void Delete(int id)
        {
            var post = this._forumPostRepository.GetById(id);
            this._forumPostRepository.Delete(id);
            this._logRepository.Log(Enums.LogType.Info, $"Deleted User : {post.PostContent}");
        }

        public ForumPostModel GetById(int id)
        {
            return this._forumPostRepository.GetById(id);
        }
        List<ForumPostModel> IForumPostService.GetPostsByUserId(int userId)
        {
            return this._forumPostRepository.GetByUserId(userId).Select(post => new ForumPostModel(post)).ToList();
        }
        public IEnumerable<ForumPostModel> GetAll()
        {
            return this._forumPostRepository.GetAllPosts().ToList();
        }
        List<ForumPostModel> IForumPostService.GetAllMyPostsByUserId(int userId)
        {
           
            
            var posts = this._forumPostRepository.GetPostsByUserId(userId).ToList();

            return posts;
        }
    }
}
