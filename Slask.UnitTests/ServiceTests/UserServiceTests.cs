using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using Xunit;

namespace Slask.UnitTests.ServiceTests
{
    public class UserServiceTests
    {
        [Fact]
        public void CanGetUserByName()
        {
            UserServiceContext services = GivenServices();
            User createdUser = services.WhenUserCreated();
            User fetchedUser = services.UserService.GetUserByName(createdUser.Name);

            fetchedUser.Should().NotBeNull();
            fetchedUser.Id.Should().Be(createdUser.Id);
            fetchedUser.Name.Should().Be(createdUser.Name);
        }

        [Fact]
        public void CanGetUserById()
        {
            UserServiceContext services = GivenServices();
            User createdUser = services.WhenUserCreated();
            User fetchedUser = services.UserService.GetUserById(createdUser.Id);

            fetchedUser.Should().NotBeNull();
            fetchedUser.Id.Should().Be(createdUser.Id);
            fetchedUser.Name.Should().Be(createdUser.Name);
        }

        private UserServiceContext GivenServices()
        {
            return UserServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
