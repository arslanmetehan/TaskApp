using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Persistence
{
    public interface IOperationRepository
    {
        void Insert(Operation operation);
        void Delete(int id);
        void DeleteOperationsByMissionId(int missionId);
        IEnumerable<OperationModel> GetAll();
        OperationModel GetById(int id);
        IEnumerable<Operation> GetByMissionId(int missionId);
        void UpdateOperationById(int id);
    }
}
