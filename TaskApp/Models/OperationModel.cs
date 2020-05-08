using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;

namespace TaskApp.Models
{
    public class OperationModel
    {
        public int Id { get; set; }
        public string OperationContent { get; set; }
        public int MissionId { get; set; }
        public int OperationStatus { get; set; }
        public OperationModel() { }

        public OperationModel(Operation operation)
        {
            this.Id = operation.Id;
            this.OperationContent = operation.OperationContent;
            this.MissionId = operation.MissionId;
            this.OperationStatus = operation.OperationStatus;
        }
    }
}
