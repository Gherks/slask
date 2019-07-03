using System;
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

        public User CreateUser(string username)
        {
            throw new NotImplementedException();
        }

        public User GetUserByName(string v)
        {
            throw new NotImplementedException();
        }

        public User GetUserById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
