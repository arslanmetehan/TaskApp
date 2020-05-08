function main() {
	let onlineUser = document.getElementById("onlineUserId").value;
	if (onlineUser == null || onlineUser == "") {
		tryGetUsers();
	}
	else {
		tryGetNotTargetUsers();
		tryGetTargetUsers();
		//tryGetFollowerUsers();
		
    }
	
}
function tryGetUsers() {
	httpRequest("api/User/GetActiveUsers", "GET", null, handleGetUsers, showError.bind(null, "System Error"));
}
function redirectProfileDetails(userId) {
	redirect("User/Profile/" + userId);
}
function tryGetTargetUsers() {
	httpRequest("api/User/GetTargetUsers", "GET", null, handleGetTargetUsers, showError.bind(null, "System Error"));
}
function tryGetNotTargetUsers() {
	httpRequest("api/User/GetNotTargetUsers", "GET", null, handleGetNotTargetUsers, showError.bind(null, "System Error"));
}
function tryGetFollowerUsers() {
	httpRequest("api/User/GetFollowerUsers", "GET", null, handleGetFollowerUsers, showError.bind(null, "System Error"));
}
function handleGetTargetUsers(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	page.users = response.Data;
	for (let i = 0; i < page.users.length; i++) {
		let user = page.users[i];

		if (user.Id.toString() != document.getElementById("onlineUserId").value) {
			appendFollowingUser(user);
		}
		
	}
}
function handleGetNotTargetUsers(response) {
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
function handleGetFollowerUsers(response) {
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
function handleGetUsers(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	
	page.users = response.Data;

	for (let i = 0; i < page.users.length; i++) {
		let user = page.users[i];


		appendOfflineUser(user);
	}
}
function appendUser(user) {
	let userTemplate = '<div class="user-box clearfix" id="user-id-##user.Id##">';
	userTemplate += '<div class="username">##user.Username##</div>';
	userTemplate += '<div style="margin-bottom:20px;"><button class="profile-btn"  id="user-profile-btn-##user.Id##" onclick="redirectProfileDetails(##user.Id##)">User Profile</button></div>';
	userTemplate += '<div style="margin-bottom:20px;"><button class="follow-btn" id="follow-btn-##user.Id##">Follow</button></div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("user-list");
	userListDiv.appendChild(userHtml);

	/*let profileBtn = document.getElementById("user-profile-btn-" + user.Id);
	profileBtn.onclick = tryDeleteUser.bind(null, user.Id);*/

	let followBtn = document.getElementById("follow-btn-" + user.Id);
	followBtn.onclick = tryFollowUser.bind(null, user.Id);

	let profileBtn = document.getElementById("user-profil-btn" + user.Id);
	
}
function appendFollowingUser(user) {
	let userTemplate = '<div class="user-box clearfix" id="user-id-##user.Id##">';
	userTemplate += '<div class="username">##user.Username##</div>';
	userTemplate += '<div style="margin-bottom:20px;"><button class="profile-btn" id="user-profile-btn-##user.Id##" onclick="redirectProfileDetails(##user.Id##)">User Profile</button></div>';
	userTemplate += '<div style="margin-bottom:20px;"><button class="follow-btn" id="unfollow-btn-##user.Id##">Unfollow</button></div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("user-list");
	userListDiv.appendChild(userHtml);

	/*let profileBtn = document.getElementById("user-profile-btn-" + user.Id);
	profileBtn.onclick = tryDeleteUser.bind(null, user.Id);*/

	let unfollowBtn = document.getElementById("unfollow-btn-" + user.Id);
	unfollowBtn.onclick = tryUnfollowUser.bind(null, user.Id);
}
function appendFollowerUser(user) {
	let userTemplate = '<div class="user-box clearfix" id="user-id-##user.Id##">';
	userTemplate += '<div class="username">##user.Username##</div>';
	userTemplate += '<div style="margin-bottom:20px;"><button class="profile-btn" id="user-profile-btn-##user.Id##" onclick="redirectProfileDetails(##user.Id##)">User Profile</button></div>';
	userTemplate += '<div style="margin-bottom:20px;"><button class="follow-btn" id="unfollow-btn-##user.Id##">Unfollow</button></div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("user-list");
	userListDiv.appendChild(userHtml);

	/*let profileBtn = document.getElementById("user-profile-btn-" + user.Id);
	profileBtn.onclick = tryDeleteUser.bind(null, user.Id);*/

	let unfollowBtn = document.getElementById("unfollow-btn-" + user.Id);
	unfollowBtn.onclick = tryUnfollowUser.bind(null, user.Id);
}
function tryFollowUser(userId) {
	httpRequest("api/User/Follow", "POST", userId.toString(), handleFollowUser.bind(null, userId), showError.bind(null, "System Error"));
}
function handleFollowUser(userId, response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let followCheckBtn = document.getElementById("follow-btn-" + userId);
	followCheckBtn.id = "unfollow-btn-" + userId;
	followCheckBtn.onclick = tryUnfollowUser.bind(null, userId);
	followCheckBtn.innerHTML = "Unfollow";
}
function tryUnfollowUser(userId) {
	httpRequest("api/User/Unfollow", "DELETE", userId.toString(), handleUnfollowUser.bind(null, userId), showError.bind(null, "System Error"));
}
function handleUnfollowUser(userId, response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let unfollowCheckBtn = document.getElementById("unfollow-btn-" + userId);
	unfollowCheckBtn.id = "follow-btn-" + userId;
	unfollowCheckBtn.onclick = tryFollowUser.bind(null, userId);
	unfollowCheckBtn.innerHTML = "Follow";
}
function appendOfflineUser(user) {
	let userTemplate = '<div class="user-box clearfix" id="user-id-##user.Id##">';
	userTemplate += '<div class="username">##user.Username##</div>';
	userTemplate += '<div style="margin-bottom:20px;"><button  class="profile-btn" id="user-profile-btn-##user.Id##" onclick="redirectProfileDetails(##user.Id##)">User Profile</button></div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("user-list");
	userListDiv.appendChild(userHtml);
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