using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Services
{
    public interface IOperationService
    {
        void AddNewOperation(Operation operation);
        void Delete(int id);
        OperationModel GetById(int id);
        OperationModel GetByOptId(int id);
        List<OperationModel> GetOperationsByMissionId(int missionId);
        void UpdateOperationById(int id);
        List<OperationModel> GetAll();
    }
}
