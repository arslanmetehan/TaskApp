function main() {
	let operationCreateBtn = document.getElementById("operation-create-btn");
	operationCreateBtn.onclick = tryInsertOperation;

	tryGetOperations();
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
function showError(message) {
	let errorDiv = document.getElementById("error");
	errorDiv.innerHTML = message;
	errorDiv.style.display = "block";
}

function hideError() {
	let errorDiv = document.getElementById("error");
	errorDiv.style.display = "none";
}