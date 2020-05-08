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

        public MissionService(IUserRepository userRepository, ILogRepository logRepository, IMissionRepository missionRepository)
        {
            this._userRepository = userRepository;
            this._logRepository = logRepository;
            this._missionRepository = missionRepository;
        }

        public void AddNewMission(Mission mission)
        {
            this._missionRepository.Insert(mission);
            this._logRepository.Log(Enums.LogType.Info, $"Inserted New User : {mission.MissionName}");
        }

        public void Delete(int id)
        {
            var mission = this._missionRepository.GetById(id);
            this._missionRepository.Delete(id);
            this._logRepository.Log(Enums.LogType.Info, $"Deleted User : {mission.MissionName}");
        }

        public MissionModel GetById(int id)
        {
            return this._missionRepository.GetById(id);
        }
        /*List<MissionModel> IMissionService.GetByMissionId()
        {
            var users = this._userRepository.GetAll().ToList();
            var missions = this._missionRepository.GetAllMissions().ToList();

            /*users.ForEach(u =>
            {
                u.Username = missions.FirstOrDefault(ug => ug.Id == u.GroupId)?.Name;
                
            });
           /* missions.ForEach(m =>
            {
                m.MissionUsername = users.FirstOrDefault(u => u.Id == m.UserId)?.Username;

            });

            return missions;
        }*/
        List<MissionModel> IMissionService.GetMissionsByUserId(int userId)
        {
            return this._missionRepository.GetByUserId(userId).Select(article => new MissionModel(article)).ToList();
        }
        public IEnumerable<MissionModel> GetAll()
        {
            return this._missionRepository.GetAllMissions().ToList();
        }
        List<MissionModel> IMissionService.GetAllMyMissionsByUserId(int userId)
        {
           
            
            var missions = this._missionRepository.GetMissionsByUserId(userId).ToList();


            /*foreach (var mission in missions)
            {
                
                mission.MissionUsername = user.Username;
            }*/

            return missions;
        }
    }
}
