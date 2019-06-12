using Slask.Data.Services;
using Slask.Domain;

namespace Slask.UnitTests.TestContexts
{
    public class UserServiceContext : TestContext
    {
        public UserService UserService { get; }

        protected UserServiceContext()
        {
            UserService = new UserService(SlaskContext);
        }

        public User WhenCreatedUser()
        {
            User user = UserService.CreateUser("Stålberto");
            SlaskContext.SaveChanges();

            return user;
        }

        public static UserServiceContext GivenServices()
        {
            return new UserServiceContext();
        }
    }
}
