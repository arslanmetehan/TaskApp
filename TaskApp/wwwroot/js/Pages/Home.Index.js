function main() {
	let userCreateBtn = document.getElementById("user-create-btn");
	userCreateBtn.onclick = tryInsertUser;

	tryGetUserGroups();
	tryGetUsers();
}