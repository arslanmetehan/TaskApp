let allOnlineUserTargets;
function main() {
	tryGetOnlineUser();
	let onlineUser = document.getElementById("onlineUserId").value;
	
	if (onlineUser == null || onlineUser == "" || onlineUser == undefined) {
	
		let userId = document.getElementById("userid").value;

		tryGetTargetUsersById(userId);
		tryGetFollowerUsersbyId(userId);
	}
	else {
		let userId = document.getElementById("userid").value;
		
		tryGetTargetUsers();
		//tryGetTargetUsersById(userId);
		//tryGetFollowerUsersbyId(userId);
		
		
    }
	
		



	
}
function tryGetOnlineUser() {
	httpRequest("api/User/GetOnlineUser", "GET", null, handleGetOnlineUser, showError.bind(null, "System Error"));
}
function handleGetOnlineUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	page.user = response.Data;
	document.getElementById("onlineUserId").value = page.user.Id;
	
}
function redirectProfileDetails(userId)
{

	redirect("User/Profile/" + userId);
	/*let userTitle = document.getElementById("users-title");
	userTitle.parentNode.removeChild(userTitle);*/

	
}
function tryGetTargetUsers() {
	httpRequest("api/User/GetTargetUsers", "GET", null, handleGetTargetUsers, showError.bind(null, "System Error"));
}
function tryGetTargetUsersById(userId) {
	httpRequest("api/User/GetTargetUsersById/?userId="+userId, "GET", null, handleGetTargetUsersById, showError.bind(null, "System Error"));
}
function tryGetFollowerUsersbyId(userId) {
	httpRequest("api/User/GetFollowerUsersById/?userId="+userId, "GET", null, handleGetFollowerUsersById, showError.bind(null, "System Error"));
}
function handleGetTargetUsers(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	page.users = response.Data;
	allOnlineUserTargets = page.users;
	let userId = document.getElementById("userid").value;
	tryGetTargetUsersById(userId);
	tryGetFollowerUsersbyId(userId);
	
	
	
}

function handleGetTargetUsersById(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	let onlineUser = document.getElementById("onlineUserId").value;
	//let onlineUserIdToInt = parseInt(onlineUser);
	if (onlineUser == null || onlineUser == "" || onlineUser == undefined)
	{
		
		page.users = response.Data;
		for (let i = 0; i < page.users.length; i++) {
			let user = page.users[i];
			appendOfflineUserTarget(user);
            
			
		}

	}
	else
	{
		
		page.users = response.Data;
		let userCheck = false;
		for (let i = 0; i < page.users.length; i++) {
			let user = page.users[i];
			for (let b = 0; b < allOnlineUserTargets.length; b++)
			{
				
					
				if(user.Id == allOnlineUserTargets[b].Id)
				{
					appendOnlineTargetUser(user);
					userCheck = true;
				}
			}
			if (userCheck == false)
			{
				if (user.Id == onlineUser)
				{
					appendOfflineUserTarget(user);
				}
				else {
					appendOnlineNotTargetUser(user);
                }
				
			}
			userCheck = false;

		}
	}
	
	
}
function handleGetFollowerUsersById(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	let onlineUser = document.getElementById("onlineUserId").value;
	//let onlineUserIdToInt = parseInt(onlineUser);
	
	if (onlineUser == null || onlineUser == "" || onlineUser == undefined) {
		page.users = response.Data;
		for (let i = 0; i < page.users.length; i++) {
			let user = page.users[i];
			appendOfflineUserFollower(user);
		}
	}
	else {

		
		page.users = response.Data;
		let userCheck = false;
		for (let i = 0; i < page.users.length; i++) {
			let user = page.users[i];
			for (let b = 0; b < allOnlineUserTargets.length; b++) {
			
				if(user.Id == allOnlineUserTargets[b].Id)
				{
					appendOnlineFollowerUser(user);
					userCheck = true;
				}
			}
			if (userCheck == false)
			{
				if (user.Id == onlineUser) {
					appendOfflineUserFollower(user);
				}
				else
				{
					appendOnlineNotFollowerUser(user);
                }
				
			}
			userCheck = false;

		}
	}
}
function appendOnlineNotTargetUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div>##user.Username##';
	userTemplate += '<div style="margin-bottom:20px;"><button id="user-profile-btn-##user.Id##" onclick="redirectProfileDetails(##user.Id##)">User Profile</button></div>';
	userTemplate += '<div style="margin-bottom:20px;"><button id="follow-btn-##user.Id##">Follow</button></div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("target-users");
	userListDiv.appendChild(userHtml);

	/*let profileBtn = document.getElementById("user-profile-btn-" + user.Id);
	profileBtn.onclick = tryDeleteUser.bind(null, user.Id);*/

	let followBtn = document.getElementById("follow-btn-" + user.Id);
	followBtn.onclick = tryFollowUser.bind(null, user.Id);
}
function appendOnlineNotFollowerUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div>##user.Username##';
	userTemplate += '<div style="margin-bottom:20px;"><button id="user-profile-btn-##user.Id##" onclick="redirectProfileDetails(##user.Id##)">User Profile</button></div>';
	userTemplate += '<div style="margin-bottom:20px;"><button id="follow-btn-##user.Id##">Follow</button></div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("follower-users");
	userListDiv.appendChild(userHtml);

	/*let profileBtn = document.getElementById("user-profile-btn-" + user.Id);
	profileBtn.onclick = tryDeleteUser.bind(null, user.Id);*/

	let followBtn = document.getElementById("follow-btn-" + user.Id);
	followBtn.onclick = tryFollowUser.bind(null, user.Id);
}
function appendOnlineTargetUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div>##user.Username##';
	userTemplate += '<div style="margin-bottom:20px;"><button id="user-profile-btn-##user.Id##" onclick="redirectProfileDetails(##user.Id##)">User Profile</button></div>';
	userTemplate += '<div style="margin-bottom:20px;"><button id="unfollow-btn-##user.Id##">Unfollow</button></div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("target-users");
	userListDiv.appendChild(userHtml);

	/*let profileBtn = document.getElementById("user-profile-btn-" + user.Id);
	profileBtn.onclick = tryDeleteUser.bind(null, user.Id);*/

	let unfollowBtn = document.getElementById("unfollow-btn-" + user.Id);
	unfollowBtn.onclick = tryUnfollowUser.bind(null, user.Id);
}
function appendOnlineFollowerUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div>##user.Username##';
	userTemplate += '<div style="margin-bottom:20px;"><button id="user-profile-btn-##user.Id##" onclick="redirectProfileDetails(##user.Id##)">User Profile</button></div>';
	userTemplate += '<div style="margin-bottom:20px;"><button id="unfollow-btn-##user.Id##">Unfollow</button></div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("follower-users");
	userListDiv.appendChild(userHtml);

	/*let profileBtn = document.getElementById("user-profile-btn-" + user.Id);
	profileBtn.onclick = tryDeleteUser.bind(null, user.Id);*/

	let unfollowBtn = document.getElementById("unfollow-btn-" + user.Id);
	unfollowBtn.onclick = tryUnfollowUser.bind(null, user.Id);
}
function appendFollowerUser(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div>##user.Username##';
	userTemplate += '<div style="margin-bottom:20px;"><button id="user-profile-btn-##user.Id##" onclick="redirectProfileDetails(##user.Id##)">User Profile</button></div>';
	userTemplate += '<div style="margin-bottom:20px;"><button id="unfollow-btn-##user.Id##">Unfollow</button></div>';
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
function appendOfflineUserFollower(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div>##user.Username##';
	userTemplate += '<div style="margin-bottom:20px;"><button id="user-profile-btn-##user.Id##" onclick="redirectProfileDetails(##user.Id##)">User Profile</button></div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("follower-users");
	userListDiv.appendChild(userHtml);
}
function appendOfflineUserTarget(user) {
	let userTemplate = '<div id="user-id-##user.Id##">';
	userTemplate += '<div>##user.Username##';
	userTemplate += '<div style="margin-bottom:20px;"><button id="user-profile-btn-##user.Id##" onclick="redirectProfileDetails(##user.Id##)">User Profile</button></div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("target-users");
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