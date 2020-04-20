using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;
using TaskApp.Services;

namespace TaskApp.Controllers
{
    [ApiController]
    [Route("api/Mission")]
    public class MissionApiController : Controller
    {
		private readonly IUserService _userService;
		private readonly IMissionService _missionService;

		public MissionApiController(IUserService userService, IMissionService missionService)
		{
			_userService = userService;
			_missionService = missionService;
		}
		[HttpGet]
		[Route(nameof(GetMissions))]
		public ActionResult<ApiResponse<List<MissionModel>>> GetMissions()
		{
			try
			{
				var users = this._missionService.GetByMissionId().ToList();

				var response = ApiResponse<List<MissionModel>>.WithSuccess(users);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<MissionModel>>.WithError(exp.ToString()));
			}
		}
		[HttpPost]
		[Route(nameof(CreateMission))]
		public ActionResult<ApiResponse<MissionModel>> CreateMission([FromBody]CreateMissionModel model)
		{
			try
			{
				MissionModel result = null;

				var newMission = new Mission();
				newMission.MissionName = model.MissionName;
				newMission.UserId = model.UserId;

				this._missionService.AddNewMission(newMission);
				result = this._missionService.GetById(newMission.Id);

				return Json(ApiResponse<MissionModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<MissionModel>.WithError(exp.ToString()));
			}
		}
		[HttpDelete]
		[Route(nameof(DeleteMission))]
		public ActionResult<ApiResponse> DeleteMission([FromBody] int missionId)
		{
			try
			{
				this._missionService.Delete(missionId);

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}
	}
}
