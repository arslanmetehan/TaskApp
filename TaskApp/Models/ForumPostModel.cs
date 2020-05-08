using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;

namespace TaskApp.Models
{
    public class ForumPostModel
    {
        public int Id { get; set; }
        public string PostContent { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
       
        

        public ForumPostModel() { }

        public ForumPostModel(ForumPost post)
        {
            this.Id = post.Id;
            this.PostContent = post.PostContent;
            this.UserId = post.UserId;
        }
    }
}
