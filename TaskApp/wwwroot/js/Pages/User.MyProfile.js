function main() {
	let userCreateBtn = document.getElementById("user-create-btn");
	userCreateBtn.onclick = tryUpdateUser;

	tryGetOnlineUser();

}

function tryGetOnlineUser() {
	httpRequest("api/User/GetOnlineUser", "GET", null, handleGetOnlineUser, showError.bind(null, "System Error"));
}

function handleGetOnlineUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.OnlineUser = response.Data;

	let username = document.getElementById("username");
	username.innerHTML = page.OnlineUser.Username;
	let email = document.getElementById("email");
	email.innerHTML = page.OnlineUser.Email;
	let birth = document.getElementById("birth");
	birth.innerHTML = page.OnlineUser.BirthYear;

}
function appendUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div>##user.Username##';
	userTemplate += '<div>##user.Email##';
	userTemplate += '<div>##user.BirthYear##';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)
		.split("##user.Email##").join(user.Email)//.replace("##user.Email##", userModel.Email)
		.split("##user.BirthYear##").join(user.BirthYear)//.replace("##user.Email##", userModel.Email)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("user-list");
	userListDiv.appendChild(userHtml);

	
}
/*function createUser(user) {
	let username = document.getElementById("username");
	username.innerHTML = user.Username;
	let email = document.getElementById("email");
	email.innerHTML = user.Email;
	let birth = document.getElementById("birth");
	birth.innerHTML = user.BirthYear;
}*/
function tryUpdateUser() {
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

	httpRequest("api/User/UpdateUser", "PUT", data, handleUpdateUser, showError.bind(null, "System Error"));
}
function redirectMyProfileDetails() {
	redirect("User/MyProfile");
}
function handleUpdateUser(response) {
	
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	redirectMyProfileDetails();

	
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
