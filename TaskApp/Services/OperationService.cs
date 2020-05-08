using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;
using TaskApp.Persistence;
using TaskApp.Helpers;

namespace TaskApp.Services
{
    public class OperationService : IOperationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogRepository _logRepository;
        private readonly IMissionRepository _missionRepository;
        private readonly IOperationRepository _operationRepository;


        public OperationService(IOperationRepository operationRepository, IUserRepository userRepository, ILogRepository logRepository, IMissionRepository missionRepository)
        {
            this._userRepository = userRepository;
            this._logRepository = logRepository;
            this._missionRepository = missionRepository;
            this._operationRepository = operationRepository;
        }
        public void AddNewOperation(Operation operation)
        {
            this._operationRepository.Insert(operation);
            this._logRepository.Log(Enums.LogType.Info, $"Inserted New User : {operation.OperationContent}");
        }

        public void Delete(int id)
        {
            var operation = this._operationRepository.GetById(id);
            this._operationRepository.Delete(id);
            this._logRepository.Log(Enums.LogType.Info, $"Deleted User : {operation.OperationContent}");
        }

        public OperationModel GetById(int id)
        {
            return this._operationRepository.GetById(id);
        }
        public void UpdateOperationById(int id)
        {
            this._operationRepository.UpdateOperationById(id);
        }
        public OperationModel GetByOptId(int id)
        {
            return this._operationRepository.GetByOptId(id);
        }
        public List<OperationModel> GetAll()
        {
            return this._operationRepository.GetAll().ToList();
        }
        public List<OperationModel> GetOperationsByMissionId(int missionId)
        {
           
            return this._operationRepository.GetByMissionId(missionId).Select(operation => new OperationModel(operation)).ToList();
        }
    }
}
