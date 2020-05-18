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
				if (user == null)
				{
					return Json(ApiResponse<List<MissionModel>>.WithError("Not authorized !"));
				}
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
		[Route(nameof(GetAllMissions))]
		public ActionResult<ApiResponse<List<MissionModel>>> GetAllMissions()
		{
			try
			{
				// DONE: döngü yerine join yap
				var missions = this._missionService.GetAll().ToList();
				var response = ApiResponse<List<MissionModel>>.WithSuccess(missions);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<MissionModel>>.WithError(exp.ToString()));
			}
		}

		// DONE: bu method missionId parametresi almalı, adı düzeltilmeli, javascriptteki kısım da düzeltilmeli
		[HttpGet]
		[Route(nameof(GetOperationsByMissionId))]
		public ActionResult<ApiResponse<List<OperationModel>>> GetOperationsByMissionId(int missionId)
		{
			try
			{
				var operations = this._operationService.GetOperationsByMissionId(missionId);
				
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
				var user = this._userService.GetOnlineUser(this.HttpContext);
				if (user == null)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				if (model.MissionName == null || model.MissionName == "")
				{
					return Json(ApiResponse<UserModel>.WithError("Mission name is required !"));
				}
				MissionModel result = null;
				var newMission = new Mission();
				newMission.MissionName = model.MissionName;
				newMission.UserId = user.Id;
			

				this._missionService.AddNewMission(newMission);
				result = this._missionService.GetById(newMission.Id);

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
			// DONE: sadece kendi mission ıma ekleyebilmeliyim
			try
			{
				var user = this._userService.GetOnlineUser(this.HttpContext);
				if (user == null)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				if (model.OperationContent == null || model.OperationContent == "")
				{
					return Json(ApiResponse<UserModel>.WithError("Operation name is required !"));
				}
				var mission = _missionService.GetById(model.MissionId);
				if(user.Id != mission.UserId)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				// DONE: burada döngü olmamalı, get ops by mission id olmalı
				var operations = this._operationService.GetOperationsByMissionId(mission.Id);
				if(operations.Count >= 9)
				{
					return Json(ApiResponse<UserModel>.WithError("You cant add operations more then 9 !"));
				}
				else
				{
					var newOperation = new Operation();
					newOperation.OperationContent = model.OperationContent;
					newOperation.MissionId = model.MissionId;
					newOperation.OperationStatus = Enums.OperationStatus.NotDone;



					this._operationService.AddNewOperation(newOperation);
					OperationModel result = this._operationService.GetById(newOperation.Id);
					return Json(ApiResponse<OperationModel>.WithSuccess(result));
				}

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
				// DONE: Başkasının operationunu update edememeliyim.
				var user = this._userService.GetOnlineUser(this.HttpContext);
				if (user == null)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				var operation = this._operationService.GetById(model.Id);
				var mission = this._missionService.GetById(operation.MissionId);
				if(user.Id != mission.UserId)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				if(operation.OperationStatus == Enums.OperationStatus.Done)
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
				// DONE: önce operationsu siliyor sıkıntı yok.
				var user = this._userService.GetOnlineUser(this.HttpContext);
				if (user == null)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
				var mission = this._missionService.GetById(missionId);
				if(user.Id != mission.UserId)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}
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

				// DONE: komple toparlanmalı
				var user = this._userService.GetOnlineUser(this.HttpContext);
				
				if (user == null)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}

				var optToBeDeleted = this._operationService.GetById(operationId);
				var mission = this._missionService.GetById(optToBeDeleted.MissionId);
				
				if(user.Id != mission.UserId)
				{
					return Json(ApiResponse<List<ForumPostModel>>.WithError("Not authorized !"));
				}

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
