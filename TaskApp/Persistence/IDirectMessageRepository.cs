using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Persistence
{
    public interface IDirectMessageRepository
    {
        void Insert(DirectMessage message);
        void Delete(int id);
        void UpdateMessage(DirectMessage message);
        DirectMessageModel GetById(int id);
        IEnumerable<DirectMessageModel> GetBySenderId(int senderId);
        IEnumerable<DirectMessageModel> GetByReceiverId(int receiverId);
        IEnumerable<DirectMessageModel> GetBySenderAndReceiverId(int senderId, int receiverId);
        DirectMessageModel GetLastMessage(int senderId, int receiverId);
        IEnumerable<DirectMessageModel> GetNewMessages(int lastMessageId,int senderId, int receiverId);




    }
}
