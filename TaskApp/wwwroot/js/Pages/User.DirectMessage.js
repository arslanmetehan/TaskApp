function main()
{

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
	page.onlineUser = response.Data;
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
		if (user.Id == page.onlineUser.Id) {
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
		SenderId: page.onlineUser.Id,
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
	if (message.SenderId == page.onlineUser.Id) {
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

	let currentlyMessageList = document.createElement("div");
	currentlyMessageList.id = "currently-message-list";
	currentlyMessageList.className = "currently-message-box clearfix";
	activeMessageBox.appendChild(currentlyMessageList);

	let activeChattingUser = document.createElement("div");
	activeChattingUser.id = "active-chatting-user";
	activeChattingUser.className = "chatting-username";
	let username = document.getElementById("username-" + receiverId);
	activeChattingUser.innerHTML = "Talking with " + username.innerHTML;
	currentlyMessageList.appendChild(activeChattingUser);

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
function tryGetNewMessages() {
	let lastMessage = page.lastMessage;
	if (lastMessage) {
		httpRequest("api/User/GetNewMessages/?lastMessageId=" + lastMessage.Id, "GET", null, handleGetNewMessages, showError.bind(null, "System Error"));
    }
}

function handleGetMessages(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	page.messages = response.Data;
	let lastMessage = page.messages[page.messages.length - 1];
	for (let i = 0; i < page.messages.length; i++) {
		let message = page.messages[i];
		if (message.SenderId == page.onlineUser.Id)
		{
			if (message.IsDeleted == 1) {
				appendOnlineUserDeletedMessage(message);
			}
			else {
				appendOnlineUserMessage(message);
            }
			
		}
		else
		{
			if (message.IsDeleted == 1) {
				appendDeletedMessage(message);
			}
			else {
				appendMessage(message);
            }
			
			lastMessage = message;
		}
	}
	
	if (lastMessage) {
		page.lastMessage = lastMessage;
    }

	setTimeout(tryGetNewMessages, 500);
}
function handleGetNewMessages(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	
	let newMessages = response.Data;
	for (let i = 0; i < newMessages.length; i++) {
		let message = newMessages[i];
		if (message.IsDeleted == 1) {
			appendDeletedMessage(message);
		}
		else {
			appendMessage(message);
        }
		
	
	}
	let lastMessage = newMessages[newMessages.length - 1];
	if (lastMessage) {
		page.lastMessage = lastMessage;
	}
	setTimeout(tryGetNewMessages, 500);
}
function appendUser(user) {
	let userTemplate = '<div class="user-box clearfix" id="user-id-##user.Id##">';
	userTemplate += '<div id="username-##user.Id##" class="username">##user.Username##</div>';
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
	messageTemplate += '<div id="message-text-##message.Id##" class="message-text">##message.MessageContent##</div>';
	messageTemplate += '<div style="margin-bottom:20px;"><button class="delete-btn" id="message-delete-btn-##message.Id##">Delete</button></div>';
	messageTemplate += '</div>';

	let messageHtmlString = messageTemplate
		.split("##message.Id##").join(message.Id)//.replace("##user.Id##", userModel.Id)
		.split("##message.MessageContent##").join(message.MessageContent)//.replace("##user.Username##", userModel.Username)

	let messageHtml = toDom(messageHtmlString);

	let messageListDiv = document.getElementById("currently-message-list");
	messageListDiv.appendChild(messageHtml);

	let deleteBtn = document.getElementById("message-delete-btn-" + message.Id);
	deleteBtn.onclick = tryDeleteMessage.bind(null, message.Id);
	
}
function appendOnlineUserDeletedMessage(message) {
	let messageTemplate = '<div class="message-box onlineUserMessage clearfix" id="message-id-##message.Id##">';
	messageTemplate += '<div id="message-text-##message.Id##" class="message-text deleted-message">##message.MessageContent##</div>';
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
	messageTemplate += '<div id="message-text-##message.Id##" class="message-text">##message.MessageContent##</div>';
	messageTemplate += '</div>';

	let messageHtmlString = messageTemplate
		.split("##message.Id##").join(message.Id)//.replace("##user.Id##", userModel.Id)
		.split("##message.MessageContent##").join(message.MessageContent)//.replace("##user.Username##", userModel.Username)

	let messageHtml = toDom(messageHtmlString);

	let messageListDiv = document.getElementById("currently-message-list");
	messageListDiv.appendChild(messageHtml);
}
function appendDeletedMessage(message) {
	let messageTemplate = '<div class="message-box otherUserMessage clearfix" id="message-id-##message.Id##">';
	messageTemplate += '<div id="message-text-##message.Id##" class="message-text deleted-message">##message.MessageContent##</div>';
	messageTemplate += '</div>';

	let messageHtmlString = messageTemplate
		.split("##message.Id##").join(message.Id)//.replace("##user.Id##", userModel.Id)
		.split("##message.MessageContent##").join(message.MessageContent)//.replace("##user.Username##", userModel.Username)

	let messageHtml = toDom(messageHtmlString);

	let messageListDiv = document.getElementById("currently-message-list");
	messageListDiv.appendChild(messageHtml);


}
function tryDeleteMessage(messageId) {
	httpRequest("api/User/DeleteMessage", "DELETE", messageId.toString(), handleDeleteMessage.bind(null, messageId), showError.bind(null, "System Error"));
}
function handleDeleteMessage(messageId, response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let messageDiv = document.getElementById("message-text-" + messageId);
	messageDiv.innerHTML = "Bu mesaj silindi !";
	messageDiv.className = "deleted-message";
	let deleteBtn = document.getElementById("message-delete-btn-" + messageId);
	deleteBtn.parentNode.removeChild(deleteBtn);
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