using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Entities;
using TaskApp.Models;

namespace TaskApp.Persistence
{
    public interface IUserRepository
    {
        void Insert(User user);
        void Delete(int id);
        IEnumerable<UserModel> GetAll();
        UserModel GetById(int id);
        int GetUserIdByLogin(string username, string password);
    }
}
