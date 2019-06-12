using System;
using Slask.Domain;

namespace Slask.Persistance.Services
{
    public class UserService
    {
        private SlaskContext _slaskContext;

        public UserService(SlaskContext slaskContext)
        {
            _slaskContext = slaskContext;
        }

        public User CreateUser(string v1)
        {
            throw new NotImplementedException();
        }
    }
}
