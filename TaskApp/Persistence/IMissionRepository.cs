using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Persistence
{
    public interface IMissionRepository
    {
        void Insert(Mission mission);
        void Delete(int id);
        IEnumerable<MissionModel> GetMissionsByUserId(int userId);

        IEnumerable<MissionModel> GetAllMissions();
        MissionModel GetById(int id);
        IEnumerable<Mission> GetByUserId(int id);
    }
}
