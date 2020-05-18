function main() {
	let missionCreateBtn = document.getElementById("mission-create-btn");
	missionCreateBtn.onclick = tryInsertMission;

	tryGetMissions();
	
	
}
function tryGetMissions() {
	httpRequest("api/Mission/GetMyMissions", "GET", null, handleGetMissions, showError.bind(null, "System Error"));
}
function redirectOperationDetails(missionId) {
	redirect("User/MissionDetail/" + missionId);
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
	let missionTemplate = '<div class="mission-box clearfix" id="mission-id-##mission.Id##">';
	missionTemplate += '<div class="mission-name" >##mission.MissionName##</div>';
	missionTemplate += '<div class="user-name">##mission.MissionUsername##</div > ';
	missionTemplate += '<button class="mission-btn" onclick="redirectOperationDetails(##mission.Id##)" id="operation-details-btn-##mission.Id##">Operation Details</button>';
	missionTemplate += '<div style="margin-bottom:20px;"><button class="mission-btn" id="mission-delete-btn-##mission.Id##">Delete Mission</button></div>';
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
