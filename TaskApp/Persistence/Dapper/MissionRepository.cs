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
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("INSERT INTO Mission (MissionName, UserId) VALUES(@MissionName, @UserId)", mission);
                mission.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
            }
        }
        public void Delete(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("DELETE FROM Mission WHERE Id = @Id", new { Id = id });
            }
        }

        public IEnumerable<MissionModel> GetMissionsByUserId(int userId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.Query<MissionModel>("SELECT m.*, u.Username as MissionUsername FROM Mission m, User u WHERE u.Id = m.UserId AND UserId = @UserId", new { UserId = userId });
            }
        }

        public MissionModel GetById(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.QuerySingle<MissionModel>("SELECT m.*, u.Username as MissionUsername FROM Mission m, User u WHERE u.Id = m.UserId AND m.Id = @Id", new { Id = id });
            }
        }

        public IEnumerable<MissionModel> GetAllMissions()
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                // DONE: User ile joinle
                return dbConnection.Query<MissionModel>("SELECT m.*, u.Username as MissionUsername FROM Mission m, User u WHERE u.Id = m.UserId");
            }
        }

        public IEnumerable<Mission> GetByUserId(int missionUserId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                // DONE: User ile joinle
                return dbConnection.Query<Mission>("SELECT m.*, u.Username as MissionUsername FROM Mission m, User u WHERE u.Id = m.UserId AND m.UserId = @UserId", new { UserId = missionUserId });
            }
        }
    }
}
