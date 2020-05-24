using System;
using System.Linq;
using Slask.Domain;

namespace Slask.Persistence.Services
{
    public class UserService
    {
        private SlaskContext _slaskContext;

        public UserService(SlaskContext slaskContext)
        {
            _slaskContext = slaskContext;
        }

        public User CreateUser(string name)
        {
            User user = Create(name);
            _slaskContext.SaveChanges();
            return user;
        }

        public User CreateUserAsync(string name)
        {
            User user = Create(name);
            _slaskContext.SaveChangesAsync();
            return user;
        }

        public bool RenameUser(Guid id, string name)
        {
            bool renameSuccessful = RenameUserTo(id, name);

            if (renameSuccessful)
            {
                _slaskContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool RenameUserAsync(Guid id, string name)
        {
            bool renameSuccessful = RenameUserTo(id, name);

            if (renameSuccessful)
            {
                _slaskContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public User GetUserByName(string name)
        {
            return _slaskContext.Users.FirstOrDefault(user => user.Name.ToLower() == name.ToLower());
        }

        public User GetUserById(Guid id)
        {
            return _slaskContext.Users.FirstOrDefault(user => user.Id == id);
        }

        private User Create(string name)
        {
            bool nameIsEmpty = name == "";
            bool userAlreadyExists = GetUserByName(name) != null;

            if (nameIsEmpty || userAlreadyExists)
            {
                return null;
            }

            User user = User.Create(name);
            _slaskContext.Add(user);

            return user;
        }

        private bool RenameUserTo(Guid id, string name)
        {
            name = name.Trim();

            User user = GetUserById(id);
            bool userFound = user != null;

            if (userFound)
            {
                User userWithName = GetUserByName(name);
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
    }
}
