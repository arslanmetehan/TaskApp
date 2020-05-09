using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;

namespace TaskApp.Models
{
    public class DirectMessageModel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageContent { get; set; }
       
       
        

        public DirectMessageModel() { }

        public DirectMessageModel(DirectMessage message)
        {
            this.Id = message.Id;
            this.SenderId = message.SenderId;
            this.ReceiverId = message.ReceiverId;
            this.MessageContent = message.MessageContent;
        }
    }
}
