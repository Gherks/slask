using Slask.Persistance.Services;
using Slask.Domain;
using Slask.Persistance;

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

        public User WhenUserCreated()
        {
            User user = UserService.CreateUser("Stålberto");
            SlaskContext.SaveChanges();

            return user;
        }

        public static UserServiceContext GivenServices(SlaskContextCreatorInterface slaskContextCreator)
        {
            return new UserServiceContext(slaskContextCreator.CreateContext());
        }
    }
}
