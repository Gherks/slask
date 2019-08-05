using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class UserTests
    {
        [Fact]
        public void EnsureUserIsValidWhenCreated()
        {
            UserServiceContext services = GivenServices();
            User user = services.WhenCreatedUser();

            user.Should().NotBeNull();
            user.Id.Should().NotBeEmpty();
            user.Name.Should().Be("Stålberto");
        }

        [Fact]
        public void CannotCreateUserWithNameAlreadyInUseNoMatterLetterCasing()
        {
            UserServiceContext services = GivenServices();
            User createdUser = services.WhenCreatedUser();
            User duplicateUser = services.UserService.CreateUser(createdUser.Name.ToUpper());

            duplicateUser.Should().BeNull();
        }

        [Fact]
        public void CannotBeRenameUserToNameAlreadyInUseNoMatterLetterCasing()
        {
            UserServiceContext services = GivenServices();
            User firstUser = services.WhenCreatedUser();
            User secondUser = services.UserService.CreateUser("Bönis");

            secondUser.RenameTo(firstUser.Name.ToUpper());

            secondUser.Name.Should().Be("Bönis");
        }

        private UserServiceContext GivenServices()
        {
            return UserServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
