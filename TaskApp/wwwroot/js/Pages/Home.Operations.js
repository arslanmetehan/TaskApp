
function main() {
	
	let missionIdInput = document.getElementById("missionid").value;
	let missionId = parseInt(missionIdInput);
	

	tryGetOperations();
}

function tryGetOperations() {
	httpRequest("api/Mission/GetCurrentMissionOperations", "GET", null, handleGetOperations, showError.bind(null, "System Error"));
}

function handleGetOperations(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	let missionIdInput = parseInt(document.getElementById("missionid").value);
	page.operations = response.Data;
	for (let i = 0; i < page.operations.length; i++) {
		let operation = page.operations[i];
	
		if (operation.MissionId == missionIdInput)
		{
			appendOperation(operation);
			if (operation.OperationStatus == 1) {
				OperationDone(operation.Id);
			}
		}
		
	}

}

function OperationDone(operationId) {
	let doneOperation = document.getElementById("operation-content-" + operationId);
	doneOperation.style = "text-decoration: line-through;" +
		"text-decoration-color: #ff5733;";
	//let optStatus = document.getElementById("operation-status-" + operationId);
	//optStatus.value = 1;
	tryUpdateOperation(operationId);
}
function tryUpdateOperation(operationId) {
	let optId = operationId;
	let optIdtoInt = parseInt(optId);
	//let optStatus = document.getElementById("operation-status-" + operationId).value;
	//let optStatusToInt = parseInt(optStatus);
	let data = {
		Id: optIdtoInt,
		//OperationStatus: optStatusToInt,


	};
	httpRequest("api/Mission/UpdateOperation", "POST", data, handleUpdateOperation, showError.bind(null, "System Error"));
}
function handleUpdateOperation(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
}
function appendOperation(operation) {
	let operationTemplate = '<div class="operation-box clearfix" id="operation-id-##operation.Id##">';
	operationTemplate += '<div id="operation-content-##operation.Id##" class="operation-content">##operation.OperationContent##</div>';
	//operationTemplate += '<div><button onclick="OperationDone(##operation.Id##)" class="createBtn" value="##operation.Id##" id="operation-done-btn-##operation.Id##">DONE</button></div>';
	operationTemplate += '<input id="operation-status-##operation.Id##" type="hidden" value="##operation.OperationStatus##"/>';
	operationTemplate += '</div>';

	let operationHtmlString = operationTemplate
		.split("##operation.Id##").join(operation.Id)//.replace("##user.Id##", userModel.Id)
		.split("##operation.OperationStatus##").join(operation.OperationStatus)//.replace("##user.Id##", userModel.Id)
		.split("##operation.OperationContent##").join(operation.OperationContent)//.replace("##user.Username##", userModel.Username)

	let operationHtml = toDom(operationHtmlString);

	let operationListDiv = document.getElementById("operation-list");
	operationListDiv.appendChild(operationHtml);
	if (operation.OperationStatus == 1) {
		OperationDone(operation.Id);
    }
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