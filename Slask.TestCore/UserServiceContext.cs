using Slask.Persistence.Services;
using Slask.Domain;
using Slask.Persistence;

namespace Slask.TestCore
{
    public class UserServiceContext : TestContextBase
    {
        public UserService UserService { get; }

        protected UserServiceContext(SlaskContext slaskContext)
            : base(slaskContext)
        {
            UserService = new UserService(SlaskContext);
        }

        public User WhenCreatedUser()
        {
            User user = UserService.CreateUser("Stålberto");
            SlaskContext.SaveChanges();

            return user;
        }

        public User WhenCreatedUsers()
        {
            User user = UserService.CreateUser("Stålberto");
            UserService.CreateUser("Bönis");
            UserService.CreateUser("Guggelito");
            SlaskContext.SaveChanges();

            return user;
        }

        public static UserServiceContext GivenServices(SlaskContextCreatorInterface slaskContextCreator)
        {
            return new UserServiceContext(slaskContextCreator.CreateContext());
        }
    }
}
