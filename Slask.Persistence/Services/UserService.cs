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
            bool nameIsEmpty = name == "";
            bool userAlreadyExists = GetUserByName(name) != null;

            if (nameIsEmpty || userAlreadyExists)
            {
                return null;
            }

            User user = User.Create(name);

            _slaskContext.Add(user);
            _slaskContext.SaveChanges();
            
            return user;
        }

        public User GetUserByName(string name)
        {
            return _slaskContext.Users.FirstOrDefault(user => user.Name.ToLower() == name.ToLower());
        }

        public User GetUserById(Guid id)
        {
            return _slaskContext.Users.FirstOrDefault(user => user.Id == id);
        }
    }
}
