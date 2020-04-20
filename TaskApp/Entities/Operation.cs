using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Entities
{
    public class Operation
    {
        [Key]
        public int Id { get; set; }
        public string OperationContent { get; set; }
        public int MissionId { get; set; }
    }
}
