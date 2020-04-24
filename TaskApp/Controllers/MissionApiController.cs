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
using System.Threading;

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
				var operations = this._operationService.GetAll();
				
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
		public ActionResult<ApiResponse<OperationModel>> AddOperation([FromBody]CreateOperationModel model)
		{
			try
			{
				
				var operations = this._operationService.GetAll();
				int operationCount = 0;
				foreach (OperationModel operation in operations)
				{
					if (operation.MissionId == Convert.ToInt32(model.MissionId))
					{
						operationCount++;
					}
				}
				if (operationCount >= 9)
				{
					return Json(ApiResponse<UserModel>.WithError("You cant add operations more then 9 !"));
				}

				var newOperation = new Operation();
				newOperation.OperationContent = model.OperationContent;
				newOperation.MissionId = Convert.ToInt32(model.MissionId);
				newOperation.OperationStatus = 0;
				
				
		
				this._operationService.AddNewOperation(newOperation);
				OperationModel result = null;
				result = this._operationService.GetById(newOperation.Id);
				return Json(ApiResponse<OperationModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<OperationModel>.WithError(exp.ToString()));
			}
		}
		[HttpPost]
		[Route(nameof(UpdateOperation))]
		public ActionResult<ApiResponse<OperationModel>> UpdateOperation([FromBody]CreateOperationModel model)
		{
			try
			{
				var operation = this._operationService.GetByOptId(model.Id);
				
				if(operation.OperationStatus == 1)
				{
					return Json(ApiResponse<OperationModel>.WithSuccess());
				}
				this._operationService.UpdateOperationById(operation.Id);
				return Json(ApiResponse<OperationModel>.WithSuccess());
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
				
				var operations = this._operationService.GetAll();
				var OptToBeDeleted = this._operationService.GetByOptId(operationId);
				List<OperationModel> AllOperations = new List<OperationModel>();
				int operationCount = 0;
				foreach (OperationModel operation in operations)
				{
					if (operation.MissionId == OptToBeDeleted.MissionId)
					{
						operationCount++;
					}
				}
				this._operationService.Delete(operationId);
				if (operationCount <= 9)
				{
					return Json(ApiResponse.WithSuccess());
				}

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}
	}
}
