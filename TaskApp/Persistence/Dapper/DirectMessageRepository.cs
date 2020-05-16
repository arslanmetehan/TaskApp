using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Persistence.Dapper
{
    public class DirectMessageRepository : BaseSqliteRepository, IDirectMessageRepository
    {
        public void Insert(DirectMessage message)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("INSERT INTO DirectMessage (SenderId, receiverId, MessageContent, IsDeleted) VALUES(@SenderId, @receiverId, @MessageContent, @IsDeleted)", message);
                message.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
            }
        }
        public void Delete(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Execute("DELETE FROM DirectMessage WHERE Id = @Id", new { Id = id });
                //dbConnection.Query("UPDATE DirectMessage SET MessageContent = @MessageContent  WHERE Id = @Id", user);
            }
        }
        public void UpdateMessage(DirectMessage message)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                dbConnection.Query("UPDATE DirectMessage SET MessageContent = @MessageContent, IsDeleted = 1 WHERE Id = @Id", message);
            }
        }
        public DirectMessageModel GetById(int id)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
                return dbConnection.QuerySingle<DirectMessageModel>("SELECT * FROM DirectMessage WHERE Id = @Id", new { Id = id });
            }
        }

        public IEnumerable<DirectMessageModel> GetBySenderId(int senderId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {
 
                return dbConnection.Query<DirectMessageModel>("SELECT * FROM DirectMessage WHERE SenderId = @SenderId", new { SenderId = senderId });
            }
        }
        public IEnumerable<DirectMessageModel> GetByReceiverId(int receiverId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {

                return dbConnection.Query<DirectMessageModel>("SELECT * FROM DirectMessage WHERE ReceiverId = @ReceiverId", new { SenderId = receiverId });
            }
        }
        public IEnumerable<DirectMessageModel> GetBySenderAndReceiverId(int senderId , int receiverId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {

                return dbConnection.Query<DirectMessageModel>("SELECT * FROM DirectMessage WHERE SenderId = @SenderId AND ReceiverId = @ReceiverId OR SenderId = @ReceiverId AND ReceiverId = @SenderId ORDER BY Id ASC", new { SenderId = senderId, ReceiverId = receiverId });
            }
        }
        public DirectMessageModel GetLastMessage(int senderId, int receiverId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {

                return dbConnection.QuerySingle<DirectMessageModel>("SELECT * FROM DirectMessage WHERE SenderId = @ReceiverId AND ReceiverId = @SenderId ORDER BY Id DESC LIMIT 1", new { SenderId = senderId, ReceiverId = receiverId });
            }
        }
        public IEnumerable<DirectMessageModel> GetNewMessages(int lastMessageId,int senderId, int receiverId)
        {
            using (IDbConnection dbConnection = this.OpenConnection())
            {

                return dbConnection.Query<DirectMessageModel>("SELECT * FROM DirectMessage WHERE SenderId = @SenderId AND ReceiverId = @ReceiverId AND Id > @LastMessageId ORDER BY Id ASC", new { SenderId = senderId, ReceiverId = receiverId, LastMessageId = lastMessageId });
            }
        }
    }
}
