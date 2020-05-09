function main()
{
	tryGetOnlineUser();
}
class OnlineUser {
	Id;
	Username;

};

function tryGetOnlineUser() {
	httpRequest("api/User/GetOnlineUser", "GET", null, handleGetOnlineUser, showError.bind(null, "System Error"));
}
function handleGetOnlineUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	page.user = response.Data;
	if (response.Data != null || response.Data != undefined) {
		OnlineUser.Id = page.user.Id;
		OnlineUser.Username = page.user.Username;
    }

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
		if (user.Id == OnlineUser.Id) {
			continue;
		}
		else
		{
			appendUser(user);
		}

		
	}
}
function tryInsertMessage(receiverId) {
	let messageContent = document.getElementById("message-create-messagecontent").value;

	let data = {
		MessageContent: messageContent,
		SenderId: OnlineUser.Id,
		ReceiverId: receiverId,
		
	};

	httpRequest("api/User/CreateMessage", "POST", data, handleInsertMessage, showError.bind(null, "System Error"));
}

function handleInsertMessage(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let message = response.Data;
	if (message.SenderId == OnlineUser.Id) {
		appendOnlineUserMessage(message);
	}
	else
	{
		appendMessage(message);
	}
	
}
function OpenMessageList(receiverId)
{
	if (document.getElementById("active-chat-box") != null || document.getElementById("active-chat-box") != undefined)
	{
		let activeChatBox = document.getElementById("active-chat-box");
		activeChatBox.parentNode.removeChild(activeChatBox);
	}
	let activeMessageBox = document.createElement("div");
	activeMessageBox.id = "active-chat-box";
	activeMessageBox.className = "clearfix";
	let chatArea = document.getElementById("chat-area");
	chatArea.appendChild(activeMessageBox);

	let activeChattingUser = document.createElement("div");
	activeChattingUser.id = "active-chatting-user";
	activeChattingUser.innerHTML = "Şuanki User";
	activeMessageBox.appendChild(activeChattingUser);

	let currentlyMessageList = document.createElement("div");
	currentlyMessageList.id = "currently-message-list";
	currentlyMessageList.className = "clearfix";
	activeMessageBox.appendChild(currentlyMessageList);

	let messageInput = document.createElement("input");
	messageInput.id = "message-create-messagecontent";
	activeMessageBox.appendChild(messageInput);

	let sendBtn = document.createElement("button");
	sendBtn.innerHTML = "Send";
	sendBtn.id = "message-send-btn";
	sendBtn.onclick = tryInsertMessage.bind(this, receiverId);
	activeMessageBox.appendChild(sendBtn);
	tryGetMessages(receiverId);
}
function tryGetMessages(receiverId) {
	httpRequest("api/User/GetMessages/?receiverId=" + receiverId, "GET", null, handleGetMessages, showError.bind(null, "System Error"));
}
function handleGetMessages(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.messages = response.Data;
	for (let i = 0; i < page.messages.length; i++) {
		let message = page.messages[i];
		if (message.SenderId == OnlineUser.Id) {
			appendOnlineUserMessage(message);
		}
		else
		{
			appendMessage(message)
		}
		
	}
	
}
function appendUser(user) {
	let userTemplate = '<div class="user-box clearfix" id="user-id-##user.Id##">';
	userTemplate += '<div class="username">##user.Username##</div>';
	userTemplate += '<div style="margin-bottom:20px;"><button class="message-btn" onclick="OpenMessageList(##user.Id##)" id="message-btn-##user.Id##">Message</button></div>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)
	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("user-list");
	userListDiv.appendChild(userHtml);
}
function appendOnlineUserMessage(message) {
	let messageTemplate = '<div class="message-box onlineUserMessage clearfix" id="message-id-##message.Id##">';
	messageTemplate += '<div class="message-text">##message.MessageContent##</div>';
	messageTemplate += '</div>';

	let messageHtmlString = messageTemplate
		.split("##message.Id##").join(message.Id)//.replace("##user.Id##", userModel.Id)
		.split("##message.MessageContent##").join(message.MessageContent)//.replace("##user.Username##", userModel.Username)

	let messageHtml = toDom(messageHtmlString);

	let messageListDiv = document.getElementById("currently-message-list");
	messageListDiv.appendChild(messageHtml);
}
function appendMessage(message) {
	let messageTemplate = '<div class="message-box otherUserMessage clearfix" id="message-id-##message.Id##">';
	messageTemplate += '<div class="message-text">##message.MessageContent##</div>';
	messageTemplate += '</div>';

	let messageHtmlString = messageTemplate
		.split("##message.Id##").join(message.Id)//.replace("##user.Id##", userModel.Id)
		.split("##message.MessageContent##").join(message.MessageContent)//.replace("##user.Username##", userModel.Username)

	let messageHtml = toDom(messageHtmlString);

	let messageListDiv = document.getElementById("currently-message-list");
	messageListDiv.appendChild(messageHtml);
}
function tryDeleteMessage(messageId) {
	httpRequest("api/User/DeleteMessage", "DELETE", messageId.toString(), handleDeleteMessage.bind(null, postId), showError.bind(null, "System Error"));
}

function handleDeleteMessage(messageId, response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let messageDiv = document.getElementById("message-id-" + messageId);
	messageDiv.parentNode.removeChild(messageDiv);
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