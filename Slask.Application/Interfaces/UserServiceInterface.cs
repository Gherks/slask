using Slask.Domain;
using System;
using System.Collections.Generic;

namespace Slask.Persistence.Services
{
    public interface UserServiceInterface
    {
        User CreateUser(string name);
        bool RenameUser(Guid id, string name);
        IEnumerable<User> GetUsers();
        User GetUserByName(string name);
        User GetUserById(Guid id);
        void Save();
    }
}