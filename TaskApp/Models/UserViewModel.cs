using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models
{
    public class UserViewModel : BaseViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
    }
}
