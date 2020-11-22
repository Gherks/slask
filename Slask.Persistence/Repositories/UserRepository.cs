using Microsoft.EntityFrameworkCore;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Persistence.Repositories
{
    public class UserRepository : UserRepositoryInterface, IDisposable
    {
        private SlaskContext _slaskContext;

        public UserRepository(SlaskContext slaskContext)
        {
            _slaskContext = slaskContext;
        }

        public void Dispose()
        {
        }

        public User CreateUser(string name)
        {
            bool nameIsEmpty = name == "";
            bool userAlreadyExists = GetUser(name) != null;

            if (nameIsEmpty || userAlreadyExists)
            {
                return null;
            }

            User user = User.Create(name);
            _slaskContext.Add(user);

            return user;
        }

        public bool RenameUser(Guid id, string name)
        {
            name = name.Trim();

            User user = GetUser(id);
            bool userFound = user != null;

            if (userFound)
            {
                User userWithName = GetUser(name);
                bool noUserWithNameExist = userWithName == null;

                if (noUserWithNameExist)
                {
                    user.RenameTo(name);
                    return true;
                }

                // LOG Error: Could not rename user - user with given name already exist.
                return false;
            }

            // LOG Error: Could not rename user - user not found.
            return false;
        }

        public IEnumerable<User> GetUsers()
        {
            return _slaskContext.Users.AsNoTracking();
        }

        public User GetUser(string name)
        {
            return _slaskContext.Users.FirstOrDefault(user => user.Name.ToLower() == name.ToLower());
        }

        public User GetUser(Guid id)
        {
            return _slaskContext.Users.FirstOrDefault(user => user.Id == id);
        }

        public void Save()
        {
            _slaskContext.SaveChanges();
        }
    }
}
