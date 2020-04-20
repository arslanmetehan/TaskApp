using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;

namespace TaskApp.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

		public UserModel() { }

		public UserModel(User user)
		{
			this.Id = user.Id;
			this.Username = user.Username;
			this.Email = user.Email;
		}
	}

}
