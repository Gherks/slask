using FluentAssertions;
using Slask.Domain;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class UserTests
    {
        [Fact]
        public void EnsureUserIsValidWhenCreated()
        {
            User user = User.Create("Stålberto");

            user.Should().NotBeNull();
            user.Id.Should().NotBeEmpty();
            user.Name.Should().Be("Stålberto");
        }

        [Fact]
        public void CannotCreateUserWithEmptyName()
        {
            User user = User.Create("");

            user.Should().BeNull();
        }

        //[Fact]
        //public void CannotCreateUserWithNameAlreadyInUseNoMatterLetterCasing()
        //{
        //    UserServiceContext services = GivenServices();
        //    User createdUser = services.WhenCreatedUser();
        //    User duplicateUser = services.UserService.CreateUser(createdUser.Name.ToUpper());

        //    duplicateUser.Should().BeNull();
        //}

        //[Fact]
        //public void CannotCreateUserWithEmptyName()
        //{
        //    UserServiceContext services = GivenServices();
        //    User user = services.UserService.CreateUser("");

        //    user.Should().BeNull();
        //}

        //private UserServiceContext GivenServices()
        //{
        //    return UserServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        //}
    }
}
