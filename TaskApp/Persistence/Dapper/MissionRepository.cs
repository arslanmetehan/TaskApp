using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Persistence.Dapper
{
    public class MissionRepository : BaseSqliteRepository, IMissionRepository
    {
        public void Insert(Mission mission)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MissionModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public MissionModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MissionModel> GetAllMissions()
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.Query<MissionModel>("SELECT m.*, m.Username FROM Mission m, User u WHERE m.UserId = u.Id");
            }
        }
    }
}
