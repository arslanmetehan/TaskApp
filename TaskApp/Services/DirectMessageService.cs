using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;
using TaskApp.Persistence;
using TaskApp.Services;
using TaskApp.Helpers;

namespace TaskApp.Services
{
    public class DirectMessageService : IDirectMessageService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogRepository _logRepository;
        private readonly IDirectMessageRepository _directMessageRepository;

        public DirectMessageService(IUserRepository userRepository, ILogRepository logRepository,IDirectMessageRepository directMessageRepository)
        {
            this._userRepository = userRepository;
            this._logRepository = logRepository;
            this._directMessageRepository = directMessageRepository;
        }

        public void AddNewMessage(DirectMessage message)
        {
            this._directMessageRepository.Insert(message);
            this._logRepository.Log(Enums.LogType.Info, $"Inserted New Message : {message.MessageContent}");
        }

        public void Delete(int id)
        {
            var message = this._directMessageRepository.GetById(id);
            this._directMessageRepository.Delete(id);
            this._logRepository.Log(Enums.LogType.Info, $"Deleted Message : {message.MessageContent}");
        }
        public void UpdateMessage(DirectMessage message)
        {

            this._directMessageRepository.UpdateMessage(message);
        }

        public DirectMessageModel GetById(int id)
        {
            return this._directMessageRepository.GetById(id);
        }
        List<DirectMessageModel> IDirectMessageService.GetMessagesBySenderId(int senderId)
        {
            return this._directMessageRepository.GetBySenderId(senderId).ToList();
        }
        List<DirectMessageModel> IDirectMessageService.GetMessagesByReceiverId(int receiverId)
        {
            return this._directMessageRepository.GetByReceiverId(receiverId).ToList();
        }
        List<DirectMessageModel> IDirectMessageService.GetMessagesBySenderAndReceiverId(int senderId,int receiverId)
        {
            return this._directMessageRepository.GetBySenderAndReceiverId(senderId,receiverId).ToList();
        }
        DirectMessageModel IDirectMessageService.GetLastMessageBySenderAndReceiverId(int senderId, int receiverId)
        {
            return this._directMessageRepository.GetLastMessage(senderId, receiverId);
        }
        List<DirectMessageModel> IDirectMessageService.GetNewMessages(int lastMessageId, int senderId, int receiverId)
        {
            return this._directMessageRepository.GetNewMessages(lastMessageId,senderId, receiverId).ToList();
        }
    }
}
