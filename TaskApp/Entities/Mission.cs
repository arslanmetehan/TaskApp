using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Entities
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }
        public string MissionName { get; set; }
        public int UserId { get; set; }
    
    }   
}
