using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;
using TaskApp.Persistence;
using TaskApp.Services;
using TaskApp.Helpers;

namespace TaskApp.Services
{
    public class MissionService : IMissionService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogRepository _logRepository;
        private readonly IMissionRepository _missionRepository;
        private readonly IOperationService _operationService;

        public MissionService(IUserRepository userRepository, ILogRepository logRepository, IMissionRepository missionRepository, IOperationService operationService)
        {
            this._userRepository = userRepository;
            this._logRepository = logRepository;
            this._missionRepository = missionRepository;
            this._operationService = operationService;
        }

        public void AddNewMission(Mission mission)
        {
            this._missionRepository.Insert(mission);
            this._logRepository.Log(Enums.LogType.Info, $"Inserted New User : {mission.MissionName}");
        }

        public void Delete(int id)
        {
            this._operationService.DeleteOperationsByMissionId(id);
            var mission = this._missionRepository.GetById(id);
            this._missionRepository.Delete(id);
            this._logRepository.Log(Enums.LogType.Info, $"Deleted User : {mission.MissionName}");
        }

        public MissionModel GetById(int id)
        {
            return this._missionRepository.GetById(id);
        }

        public List<MissionModel> GetMissionsByUserId(int userId)
        {
            return this._missionRepository.GetByUserId(userId).Select(article => new MissionModel(article)).ToList();
        }

        public IEnumerable<MissionModel> GetAll()
        {
            return this._missionRepository.GetAllMissions().ToList();
        }

        public List<MissionModel> GetAllMyMissionsByUserId(int userId)
        {
            var missions = this._missionRepository.GetMissionsByUserId(userId).ToList();

            return missions;
        }
    }
}
