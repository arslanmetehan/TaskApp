using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Helpers
{
	public static class Converter
	{
		public static User ToEntity(this UserModel userModel)
		{
			return new User()
			{
				Id = userModel.Id,
				Username = userModel.Username,
				Email = userModel.Email,
			};
		}
		public static DirectMessage ToEntity(this DirectMessageModel messageModel)
		{
			return new DirectMessage()
			{
				Id = messageModel.Id,
				MessageContent = messageModel.MessageContent,
				SenderId = messageModel.SenderId,
				ReceiverId = messageModel.ReceiverId,
			};
		}
		public static DirectMessageModel ToModel(this DirectMessage messageModel)
		{
			return new DirectMessageModel(messageModel);
		}
		public static UserModel ToModel(this User user)
		{
			return new UserModel(user);
		}
		public static OperationModel ToModel(this Operation operation)
		{
			return new OperationModel(operation);
		}
		public static MissionModel ToModel(this Mission mission)
		{
			return new MissionModel(mission);
		}

	}
}