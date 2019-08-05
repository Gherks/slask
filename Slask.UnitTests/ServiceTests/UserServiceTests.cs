using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using System;
using Xunit;

namespace Slask.UnitTests.ServiceTests
{
    public class UserServiceTests
    {
        [Fact]
        public void CanCreateUser()
        {
            UserServiceContext services = GivenServices();
            User user = services.UserService.CreateUser("Stålberto");

            user.Should().NotBeNull();
            user.Id.Should().NotBeEmpty();
            user.Name.Should().Be("Stålberto");
        }

        [Fact]
        public void CannotCreateUserWithEmptyName()
        {
            UserServiceContext services = GivenServices();
            User user = services.UserService.CreateUser("");

            user.Should().BeNull();
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
        public void CanGetUserById()
        {
            UserServiceContext services = GivenServices();
            User createdUser = services.WhenCreatedUser();
            User fetchedUser = services.UserService.GetUserById(createdUser.Id);

            fetchedUser.Should().NotBeNull();
            fetchedUser.Id.Should().Be(createdUser.Id);
            fetchedUser.Name.Should().Be(createdUser.Name);
        }

        [Fact]
        public void ReturnsNullWhenFetchingNonexistentUserById()
        {
            UserServiceContext services = GivenServices();
            User fetchedUser = services.UserService.GetUserById(Guid.NewGuid());

            fetchedUser.Should().BeNull();
        }

        [Fact]
        public void CanGetUserByName()
        {
            UserServiceContext services = GivenServices();
            User createdUser = services.WhenCreatedUser();
            User fetchedUser = services.UserService.GetUserByName(createdUser.Name);

            fetchedUser.Should().NotBeNull();
            fetchedUser.Id.Should().Be(createdUser.Id);
            fetchedUser.Name.Should().Be(createdUser.Name);
        }

        [Fact]
        public void ReturnsNullWhenFetchingNonexistentUserByName()
        {
            UserServiceContext services = GivenServices();
            User fetchedUser = services.UserService.GetUserByName("my-god-thats-jason-bourne");

            fetchedUser.Should().BeNull();
        }

        private UserServiceContext GivenServices()
        {
            return UserServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
