using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Services
{
    public interface IForumPostService
    {
		void AddNewPost(ForumPost post);
		void Delete(int id);
		ForumPostModel GetById(int id);
		List<ForumPostModel> GetAllMyPostsByUserId(int userId);
		List<ForumPostModel> GetPostsByUserId(int userId);
		IEnumerable<ForumPostModel> GetAll();
	}
}
