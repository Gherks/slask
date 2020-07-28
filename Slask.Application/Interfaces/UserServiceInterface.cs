using Slask.Domain;
using System;

namespace Slask.Persistence.Services
{
    public interface UserServiceInterface
    {
        User CreateUser(string name);
        User GetUserById(Guid id);
        User GetUserByName(string name);
        bool RenameUser(Guid id, string name);
        void Save();
    }
}