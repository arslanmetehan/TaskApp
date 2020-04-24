using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;
using TaskApp.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace TaskApp.Controllers
{
    [ApiController]
    [Route("api/Mission")]
    public class MissionApiController : Controller
    {
		private readonly IUserService _userService;
		private readonly IMissionService _missionService;
		private readonly IOperationService _operationService;



		public MissionApiController(IUserService userService, IMissionService missionService, IOperationService operationService)
		{
			_userService = userService;
			_missionService = missionService;
			_operationService = operationService;
		}
		[HttpGet]
		[Route(nameof(GetMyMissions))]
		public ActionResult<ApiResponse<List<MissionModel>>> GetMyMissions()
		{
			try
			{
				var user = this._userService.GetOnlineUser(this.HttpContext);
				var missions = this._missionService.GetAllMyMissionsByUserId(user.Id);

				var response = ApiResponse<List<MissionModel>>.WithSuccess(missions);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<MissionModel>>.WithError(exp.ToString()));
			}
		}
		[HttpGet]
		[Route(nameof(GetCurrentMissionOperations))]
		public ActionResult<ApiResponse<List<OperationModel>>> GetCurrentMissionOperations()
		{
			try
			{
				var user = this._userService.GetOnlineUser(this.HttpContext);
				var missions = this._missionService.GetAllMyMissionsByUserId(user.Id);
				var operations = this._operationService.GetAll();
				
				
				/*foreach (var mission in missions)
				{
					mission.MissionUsername = user.Username;
				}*/


				var response = ApiResponse<List<OperationModel>>.WithSuccess(operations);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<OperationModel>>.WithError(exp.ToString()));
			}
		}
		[HttpPost]
		[Route(nameof(CreateMission))]
		public ActionResult<ApiResponse<MissionModel>> CreateMission([FromBody]CreateMissionModel model)
		{
			try
			{
				MissionModel result = null;
				var user = this._userService.GetOnlineUser(this.HttpContext);
				var newMission = new Mission();
				newMission.MissionName = model.MissionName;
				newMission.UserId = user.Id;
			

				this._missionService.AddNewMission(newMission);
				result = this._missionService.GetById(newMission.Id);
				result.MissionUsername = user.Username;
				return Json(ApiResponse<MissionModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<MissionModel>.WithError(exp.ToString()));
			}
		}
		[HttpPost]
		[Route(nameof(AddOperation))]
		public ActionResult<ApiResponse<MissionModel>> AddOperation([FromBody]CreateOperationModel model)
		{
			try
			{
				OperationModel result = null;
				var user = this._userService.GetOnlineUser(this.HttpContext);
				var newOperation = new Operation();
				newOperation.OperationContent = model.OperationContent;
				newOperation.MissionId = model.MissionId;


				this._operationService.AddNewOperation(newOperation);
				result = this._operationService.GetById(newOperation.Id);
				
				return Json(ApiResponse<OperationModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<OperationModel>.WithError(exp.ToString()));
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
		[HttpDelete]
		[Route(nameof(DeleteOperation))]
		public ActionResult<ApiResponse> DeleteOperation([FromBody] int operationId)
		{
			try
			{
				this._operationService.Delete(operationId);

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}
	}
}
