
function main() {
	let operationCreateBtn = document.getElementById("operation-create-btn");
	let missionIdInput = document.getElementById("missionid").value;
	let missionId = parseInt(missionIdInput);
	operationCreateBtn.onclick = tryInsertOperation;

	tryGetOperations(missionId);
}

function tryGetOperations(missionId) {
	httpRequest("api/Mission/GetOperationsByMissionId/?missionId=" + missionId, "GET", null, handleGetOperations, showError.bind(null, "System Error"));
}

function handleGetOperations(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	page.operations = response.Data;
	for (let i = 0; i < page.operations.length; i++) {
		let operation = page.operations[i];
		if (operation.OperationStatus == 1) {
			appendDoneOperation(operation);
		}
		else {
			appendOperation(operation);
		}
	}

}

function tryInsertOperation() {

	let operationcontent = document.getElementById("operation-create-operationcontent").value;
	let missionIdInput = document.getElementById("missionid").value;
	let missionIdToInt = parseInt(missionIdInput);

	let data = {
		OperationContent: operationcontent,
		MissionId: missionIdToInt,
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
function OperationDone(operationId) {
	let doneOperation = document.getElementById("operation-content-" + operationId);
	doneOperation.style = "text-decoration: line-through;" +
		"text-decoration-color: #ff5733;";
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
	page.updatedOptId = operationId;
	OperationDone(operationId);
	httpRequest("api/Mission/UpdateOperation", "POST", data, handleUpdateOperation, showError.bind(null, "System Error"));
}
function handleUpdateOperation(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	
	let doneBtn = document.getElementById("operation-done-btn-" + page.updatedOptId);
	doneBtn.parentNode.removeChild(doneBtn);
	
}
function appendOperation(operation) {
	let operationTemplate = '<div class="operation-box clearfix" id="operation-id-##operation.Id##">';
	operationTemplate += '<div id="operation-content-##operation.Id##" class="operation-content">##operation.OperationContent##</div>';
	operationTemplate += '<div><button onclick="tryUpdateOperation(##operation.Id##)" class="createBtn" value="##operation.Id##" id="operation-done-btn-##operation.Id##">DONE</button></div>';
	operationTemplate += '<input id="operation-status-##operation.Id##" type="hidden" value="##operation.OperationStatus##"/>';
	operationTemplate += '<div style="margin-bottom:10px;"><button class="createBtn deleteBtn" id="operation-delete-btn-##operation.Id##">Delete Operation</button></div>';
	operationTemplate += '</div>';

	let operationHtmlString = operationTemplate
		.split("##operation.Id##").join(operation.Id)//.replace("##user.Id##", userModel.Id)
		.split("##operation.OperationStatus##").join(operation.OperationStatus)//.replace("##user.Id##", userModel.Id)
		.split("##operation.OperationContent##").join(operation.OperationContent)//.replace("##user.Username##", userModel.Username)

	let operationHtml = toDom(operationHtmlString);

	let operationListDiv = document.getElementById("operation-list");
	operationListDiv.appendChild(operationHtml);

	let deleteBtn = document.getElementById("operation-delete-btn-" + operation.Id);
	deleteBtn.onclick = tryDeleteOperation.bind(null, operation.Id);
}
function appendDoneOperation(operation) {
	let operationTemplate = '<div class="operation-box clearfix" id="operation-id-##operation.Id##">';
	operationTemplate += '<div id="operation-content-##operation.Id##" class="operation-content">##operation.OperationContent##</div>';
	operationTemplate += '<input id="operation-status-##operation.Id##" type="hidden" value="##operation.OperationStatus##"/>';
	operationTemplate += '<div style="margin-bottom:10px;"><button class="createBtn deleteBtn" id="operation-delete-btn-##operation.Id##">Delete Operation</button></div>';
	operationTemplate += '</div>';

	let operationHtmlString = operationTemplate
		.split("##operation.Id##").join(operation.Id)//.replace("##user.Id##", userModel.Id)
		.split("##operation.OperationStatus##").join(operation.OperationStatus)//.replace("##user.Id##", userModel.Id)
		.split("##operation.OperationContent##").join(operation.OperationContent)//.replace("##user.Username##", userModel.Username)

	let operationHtml = toDom(operationHtmlString);

	let operationListDiv = document.getElementById("operation-list");
	operationListDiv.appendChild(operationHtml);
	OperationDone(operation.Id);
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
	hideError();
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