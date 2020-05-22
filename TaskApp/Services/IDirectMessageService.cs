using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Services
{
    public interface IDirectMessageService
    {
		void AddNewMessage(DirectMessage message);
		void UpdateMessage(DirectMessage message);
		DirectMessageModel GetById(int id);
		List<DirectMessageModel> GetMessagesBySenderId(int senderId);
		List<DirectMessageModel> GetMessagesByReceiverId(int receiverId);
		List<DirectMessageModel> GetMessagesBySenderAndReceiverId(int senderId, int receiverId);
		List<DirectMessageModel> GetNewMessages(int lastMessageId,int senderId, int receiverId);


	}
}
