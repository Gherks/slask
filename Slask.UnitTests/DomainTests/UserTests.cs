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
            User user = services.WhenUserCreated();

            user.Should().NotBeNull();
            user.Id.Should().NotBeEmpty();
            user.Name.Should().Be("Stålberto");
        }

        [Fact]
        public void UserCanBeRenamed()
        {
            UserServiceContext services = GivenServices();
            User user = services.WhenUserCreated();

            user.RenameTo("Bönis");

            user.Name.Should().Be("Bönis");
        }

        [Fact]
        public void UserCannotBeRenamedToEmptyName()
        {
            UserServiceContext services = GivenServices();
            User user = services.WhenUserCreated();

            user.RenameTo("");

            user.Name.Should().Be("Bönis");
        }

        [Fact]
        public void CannotCreateUserWithNameAlreadyInUseNoMatterLetterCasing()
        {
            UserServiceContext services = GivenServices();
            User createdUser = services.WhenUserCreated();
            User duplicateUser = services.UserService.CreateUser(createdUser.Name.ToUpper());

            duplicateUser.Should().BeNull();
        }

        [Fact]
        public void UserCannotBeRenamedToNameAlreadyInUseNoMatterLetterCasing()
        {
            UserServiceContext services = GivenServices();
            User firstUser = services.WhenUserCreated();
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
