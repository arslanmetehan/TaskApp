function main() {
	let missionCreateBtn = document.getElementById("mission-create-btn");
	missionCreateBtn.onclick = tryInsertMission;

	tryGetMissions();
	
	
}
function tryGetMissions() {
	httpRequest("api/Mission/GetMyMissions", "GET", null, handleGetMissions, showError.bind(null, "System Error"));
}
function redirectAddOperation(missionId) {
	redirect("User/MyMissionDetail/" + missionId);
}
function handleGetMissions(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.missions = response.Data;
	/*if (missions.length == 0) {
		showError(response.ErrorMessage);
		return;
    }*/

	for (let i = 0; i < page.missions.length; i++) {
		let mission = page.missions[i];
		appendMission(mission);
	}
}

function tryInsertMission() {
	let missionname = document.getElementById("mission-create-missionname").value;

	let data = {
		MissionName: missionname,

		
	};

	httpRequest("api/Mission/CreateMission", "POST", data, handleInsertMission, showError.bind(null, "System Error"));
}

function handleInsertMission(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let mission = response.Data;
	appendMission(mission);
}

function appendMission(mission) {
	let missionTemplate = '<div id="mission-id-##mission.Id##">';
	missionTemplate += '<div>##mission.MissionName## [User: ##mission.MissionUsername##]</div>';
	missionTemplate += '<button onclick="redirectAddOperation(##mission.Id##)" id="operation-create-btn-##mission.Id##">Add Operation</button>';
	missionTemplate += '<div style="margin-bottom:20px;"><button id="mission-delete-btn-##mission.Id##">Delete Mission</button></div>';
	missionTemplate += '</div>';

	let missionHtmlString = missionTemplate
		.split("##mission.Id##").join(mission.Id)//.replace("##user.Id##", userModel.Id)
		.split("##mission.MissionName##").join(mission.MissionName)//.replace("##user.Username##", userModel.Username)
		.split("##mission.MissionUsername##").join(mission.MissionUsername)//.replace("##user.Username##", userModel.Username)

	let missionHtml = toDom(missionHtmlString);

	let missionListDiv = document.getElementById("mission-list");
	missionListDiv.appendChild(missionHtml);

	let deleteBtn = document.getElementById("mission-delete-btn-" + mission.Id);
	deleteBtn.onclick = tryDeleteMission.bind(null, mission.Id);
	
}

function tryDeleteMission(missionId) {
	httpRequest("api/Mission/DeleteMission", "DELETE", missionId.toString(), handleDeleteMission.bind(null, missionId), showError.bind(null, "System Error"));
}

function handleDeleteMission(missionId, response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let missionDiv = document.getElementById("mission-id-" + missionId);
	missionDiv.parentNode.removeChild(missionDiv);
}
function showError(message) {
	let errorDiv = document.getElementById("error");
	errorDiv.innerHTML = message;
	errorDiv.style.display = "block";
}

function hideError() {
	let errorDiv = document.getElementById("error");
	errorDiv.style.display = "none";
}
function tryGetOperations() {
	httpRequest("api/Mission/GetCurrentMissionOperations()", "GET", null, handleGetOperations, showError.bind(null, "System Error"));
}

function handleGetOperations(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.operations = response.Data;
	for (let i = 0; i < page.operations.length; i++) {
		let operation = page.operations[i];
		appendOperation(operation);
	}
}

function tryInsertOperation() {
	let operationcontent = document.getElementById("operation-create-operationcontent").value;

	let data = {
		OperationContent: operationcontent,


	};

	httpRequest("api/Mission/AddOperation", "POST", data, handleInsertOperation, showError.bind(null, "System Error"));
}

function handleInsertOperation(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let operation = response.Data;
	appendOperation(operation);
}
function appendOperation(operation) {
	let operationTemplate = '<div id="operation-id-##operation.Id##">';
	operationTemplate += '<div>##operation.OperationContent##</div>';
	operationTemplate += '<div style="margin-bottom:20px;"><button id="operation-delete-btn-##operation.Id##">Delete Operation</button></div>';
	operationTemplate += '<div>button id="operation-create-btn">ADD OPERATION</button></div>';
	operationTemplate += '</div>';

	let operationHtmlString = operationTemplate
		.split("##operation.Id##").join(operation.Id)//.replace("##user.Id##", userModel.Id)
		.split("##operation.OperationContent##").join(operation.OperationContent)//.replace("##user.Username##", userModel.Username)

	let operationHtml = toDom(operationHtmlString);

	let operationListDiv = document.getElementById("operation-list");
	operationListDiv.appendChild(operationHtml);

	let deleteBtn = document.getElementById("operation-delete-btn-" + operation.Id);
	deleteBtn.onclick = tryDeleteOperation.bind(null, operation.Id);
}

function tryDeleteOperation(operationId) {
	httpRequest("api/Mission/DeleteOperation", "DELETE", operationId.toString(), handleDeleteOperation.bind(null, operationId), showError.bind(null, "System Error"));
}

function handleDeleteOperation(operationId, response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let operationDiv = document.getElementById("operation-id-" + operationId);
	operationDiv.parentNode.removeChild(operationDiv);
}