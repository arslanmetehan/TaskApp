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
        IEnumerable<MissionModel> GetAll();

        IEnumerable<MissionModel> GetAllMissions();
        MissionModel GetById(int id);
    }
}
