function main()
{

	tryGetOnlineUser();
	let onlineUser = document.getElementById("onlineUser").value;
	if (onlineUser == null || onlineUser == "" || onlineUser == undefined) {
		let postCreateBtn = document.getElementById("post-create-btn");
		let postCreateInput = document.getElementById("post-create-postcontent");
		postCreateBtn.parentNode.removeChild(postCreateBtn);
		postCreateInput.parentNode.removeChild(postCreateInput);

	}
	else
	{
		let postCreateBtn = document.getElementById("post-create-btn");
		postCreateBtn.onclick = tryInsertPost;
	}
	
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

	tryGetPosts();
}
function tryGetPosts() {
	httpRequest("api/Forum/GetPosts", "GET", null, handleGetPosts, showError.bind(null, "System Error"));
}

function handleGetPosts(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}
	let onlineUser = document.getElementById("onlineUser").value;
	page.posts = response.Data;
	for (let i = page.posts.length-1; i >= 0; i--) {
		let post = page.posts[i];
		if (post.UserId == OnlineUser.Id) {
			appendPost(post);
		}
		else
		{
			appendAnotherUserPost(post);
		}
		
	}
}
function tryInsertPost() {
	let postContent = document.getElementById("post-create-postcontent").value;

	let data = {
		PostContent: postContent,
		
	};

	httpRequest("api/Forum/CreatePost", "POST", data, handleInsertPost, showError.bind(null, "System Error"));
}

function handleInsertPost(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let post = response.Data;
	appendPost(post);
}

function appendPost(post) {
	let postTemplate = '<div class="post-box clearfix" id="post-id-##post.Id##">';
	postTemplate += '<div class="username">##post.Username##</div>';
	postTemplate += '<div class="post">##post.PostContent##</div>';
	postTemplate += '<button class="delete-btn" id="post-delete-btn-##post.Id##">Delete Post</button>';
	postTemplate += '</div>';

	let postHtmlString = postTemplate
		.split("##post.Id##").join(post.Id)//.replace("##user.Id##", userModel.Id)
		.split("##post.Username##").join(post.Username)//.replace("##user.Username##", userModel.Username)
		.split("##post.PostContent##").join(post.PostContent)//.replace("##user.Email##", userModel.Email)

	let postHtml = toDom(postHtmlString);

	let postListDiv = document.getElementById("post-list");
	postListDiv.appendChild(postHtml);

	let deleteBtn = document.getElementById("post-delete-btn-" + post.Id);
	deleteBtn.onclick = tryDeletePost.bind(null, post.Id);
}
function appendAnotherUserPost(post) {
	let postTemplate = '<div class="post-box clearfix" id="post-id-##post.Id##">';
	postTemplate += '<div class="username">##post.Username##';
	postTemplate += '<div class="post">##post.PostContent##';
	postTemplate += '</div>';

	let postHtmlString = postTemplate
		.split("##post.Id##").join(post.Id)//.replace("##user.Id##", userModel.Id)
		.split("##post.Username##").join(post.Username)//.replace("##user.Username##", userModel.Username)
		.split("##post.PostContent##").join(post.PostContent)//.replace("##user.Email##", userModel.Email)

	let postHtml = toDom(postHtmlString);

	let postListDiv = document.getElementById("post-list");
	postListDiv.appendChild(postHtml);
}

function tryDeletePost(postId) {
	httpRequest("api/Forum/DeletePost", "DELETE", postId.toString(), handleDeletePost.bind(null, postId), showError.bind(null, "System Error"));
}

function handleDeletePost(postId, response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let postDiv = document.getElementById("post-id-" + postId);
	postDiv.parentNode.removeChild(postDiv);
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