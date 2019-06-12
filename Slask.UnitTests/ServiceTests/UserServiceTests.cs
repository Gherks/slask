using FluentAssertions;
using Slask.Domain;
using Slask.UnitTests.TestContexts;
using Xunit;

namespace Slask.UnitTests.ServiceTests
{
    public class UserServiceTests
    {
        [Fact]
        public void CanCreateUser()
        {
            UserServiceContext services = UserServiceContext.GivenServices();
            User user = services.WhenCreatedUser();

            user.Should().NotBeNull();
            user.Name.Should().Be("Stålberto");
        }

        [Fact]
        public void UserMustHaveAName()
        {
            UserServiceContext services = UserServiceContext.GivenServices();
            User user = services.UserService.CreateUser("");

            user.Should().BeNull();
        }

        [Fact]
        public void UserMustBeUnqiueByName()
        {
            UserServiceContext services = UserServiceContext.GivenServices();
            services.WhenCreatedUser();
            User duplicateUser = services.UserService.CreateUser("Stålberto");

            duplicateUser.Should().BeNull();
        }
    }
}
