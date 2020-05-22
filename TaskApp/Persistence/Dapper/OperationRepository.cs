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
    public class OperationRepository : BaseSqliteRepository, IOperationRepository
    {
        public void Insert(Operation operation)
        {
        
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("INSERT INTO Operation (OperationContent, OperationStatus, MissionId) VALUES(@OperationContent, @OperationStatus, @MissionId)", operation);
                operation.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
            }
            
        }
        public void Delete(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("DELETE FROM Operation WHERE Id = @Id", new { Id = id });
            }
        }
        public void DeleteOperationsByMissionId(int missionId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("DELETE FROM Operation WHERE MissionId = @MissionId", new { MissionId = missionId });
            }
        }
        public IEnumerable<OperationModel> GetAll()
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.Query<OperationModel>("SELECT * FROM Operation");
            }
        }

        public OperationModel GetById(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.QuerySingle<OperationModel>("SELECT * FROM Operation o WHERE o.Id = @Id", new { Id = id });
            }
        }
        public IEnumerable<Operation> GetByMissionId(int missionId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.Query<Operation>("SELECT * FROM Operation WHERE MissionId = @MissionId", new { MissionId = missionId });
            }
        }
        public void UpdateOperationById(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("UPDATE Operation SET OperationStatus = 1 WHERE Id = @Id", new { Id = id });
            }
        }


    }
}
