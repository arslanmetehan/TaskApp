function main() {
	tryGetMissions();
}
function tryGetMissions() {
	httpRequest("api/Mission/GetAllMissions", "GET", null, handleGetMissions, showError.bind(null, "System Error"));
}
function redirectOperationDetails(missionId) {
	redirect("Home/MissionDetail/" + missionId);
}
function handleGetMissions(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	
	page.missions = response.Data;
	for (let i = 0; i < page.missions.length; i++) {
		let mission = page.missions[i];
		appendMission(mission);
	}
}
function appendMission(mission) {
	let missionTemplate = '<div class="mission-box clearfix" id="mission-id-##mission.Id##">';
	missionTemplate += '<div class="mission-name">##mission.MissionName##</div>';
	missionTemplate += '<div class="user-name">##mission.MissionUsername##</div > ';
	missionTemplate += '<button class="mission-btn" onclick="redirectOperationDetails(##mission.Id##)" id="operation-details-btn-##mission.Id##">Operation Details</button>';
	missionTemplate += '</div>';

	let missionHtmlString = missionTemplate
		.split("##mission.Id##").join(mission.Id)//.replace("##user.Id##", userModel.Id)
		.split("##mission.MissionName##").join(mission.MissionName)//.replace("##user.Username##", userModel.Username)
		.split("##mission.MissionUsername##").join(mission.MissionUsername)//.replace("##user.Username##", userModel.Username)

	let missionHtml = toDom(missionHtmlString);

	let missionListDiv = document.getElementById("mission-list");
	missionListDiv.appendChild(missionHtml);
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