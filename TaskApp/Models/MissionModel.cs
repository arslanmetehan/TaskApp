using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;

namespace TaskApp.Models
{
    public class MissionModel
    {
        public int Id { get; set; }
        public string MissionName { get; set; }
        public string MissionUsername { get; set; }
        public int UserId { get; set; }

        public MissionModel() { }

        public MissionModel(Mission mission)
        {
            this.Id = mission.Id;
            this.MissionName = mission.MissionName;
            this.UserId = mission.UserId;
        }
    }
}
