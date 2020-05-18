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
    [Route("api/Forum")]
    public class ForumApiController : Controller
    {
		private readonly IUserService _userService;
		private readonly IForumPostService _forumPostService;



		public ForumApiController(IUserService userService, IForumPostService forumPostService)
		{
			_userService = userService;
			_forumPostService = forumPostService;
		}
		[HttpGet]
		[Route(nameof(GetPosts))]
		public ActionResult<ApiResponse<List<ForumPostModel>>> GetPosts()
		{
			try
			{
				
				var posts = this._forumPostService.GetAll().ToList();

				var response = ApiResponse<List<ForumPostModel>>.WithSuccess(posts);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<ForumPostModel>>.WithError(exp.ToString()));
			}
		}
		[HttpPost]
		[Route(nameof(CreatePost))]
		public ActionResult<ApiResponse<ForumPostModel>> CreatePost([FromBody]CreateForumPostModel model)
		{
			try
			{
				var user = this._userService.GetOnlineUser(this.HttpContext);
				if(user == null)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				ForumPostModel result = null;
				var newPost = new ForumPost();
				newPost.PostContent = model.PostContent;
				newPost.UserId = user.Id;
			

				this._forumPostService.AddNewPost(newPost);
				result = this._forumPostService.GetById(newPost.Id);
				result.Username = user.Username;
				return Json(ApiResponse<ForumPostModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<ForumPostModel>.WithError(exp.ToString()));
			}
		}
		[HttpDelete]
		[Route(nameof(DeletePost))]
		public ActionResult<ApiResponse> DeletePost([FromBody] int postId)
		{
			try
			{
				var user = this._userService.GetOnlineUser(this.HttpContext);
				if (user == null)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				var post = this._forumPostService.GetById(postId);
				if(user.Id != post.UserId)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				this._forumPostService.Delete(postId);

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}
	}
}
