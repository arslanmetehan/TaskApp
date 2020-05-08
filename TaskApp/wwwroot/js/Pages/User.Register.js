function main() {
	let userCreateBtn = document.getElementById("user-create-btn");
	userCreateBtn.onclick = tryInsertUser;

	tryGetUsers();
}

function tryGetUsers() {
	httpRequest("api/User/GetActiveUsers", "GET", null, handleGetUsers, showError.bind(null, "System Error"));
}

function handleGetUsers(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.users = response.Data;

	for (let i = 0; i < page.users.length; i++) {
		let user = page.users[i];
		appendUser(user);
	}
}
function tryInsertUser() {
	let username = document.getElementById("user-create-username").value;
	let email = document.getElementById("user-create-email").value;
	let password = document.getElementById("user-create-password").value;
	let birthyear = document.getElementById("user-create-birthyear").value;
	let birthYearToInt = parseInt(birthyear);

	let data = {
		Username: username,
		Email: email,
		Password: password,
		BirthYear: birthYearToInt,
	};

	httpRequest("api/User/CreateUser", "POST", data, handleInsertUser, showError.bind(null, "System Error"));
}

function handleInsertUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let user = response.Data;
	appendUser(user);
}

function appendUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div>##user.Username##';
	userTemplate += '<div>##user.Email##';
	userTemplate += '<div style="margin-bottom:20px;"><button id="user-delete-btn-##user.Id##">Delete User</button></div>';
	userTemplate += '<div>Welcome';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)
		.split("##user.Email##").join(user.Email)//.replace("##user.Email##", userModel.Email)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("user-list");
	userListDiv.appendChild(userHtml);

	let deleteBtn = document.getElementById("user-delete-btn-" + user.Id);
	deleteBtn.onclick = tryDeleteUser.bind(null, user.Id);
}

function tryDeleteUser(userId) {
	httpRequest("api/User/DeleteUser", "DELETE", userId.toString(), handleDeleteUser.bind(null, userId), showError.bind(null, "System Error"));
}

function handleDeleteUser(userId, response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let userDiv = document.getElementById("user-id-" + userId);
	userDiv.parentNode.removeChild(userDiv);
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