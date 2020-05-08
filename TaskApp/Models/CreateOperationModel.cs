using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models
{
    public class CreateOperationModel
    {
        public int Id { get; set; }
        public string OperationContent { get; set; }
        public int MissionId { get; set; }
        public int OperationStatus { get; set; }
    }
}
