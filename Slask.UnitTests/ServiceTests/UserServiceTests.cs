using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using System;
using Xunit;

namespace Slask.UnitTests.ServiceTests
{
    public class UserServiceTests
    {
        private readonly UserServiceContext services;

        public UserServiceTests()
        {
            services = UserServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }

        [Fact]
        public void CanCreateUser()
        {
            string userName = "Guggelito";

            User user = services.UserService.CreateUser(userName);

            user.Should().NotBeNull();
            user.Id.Should().NotBeEmpty();
            user.Name.Should().Be(userName);
        }

        [Fact]
        public void CannotCreateUserWithEmptyName()
        {
            User user = services.UserService.CreateUser("");

            user.Should().BeNull();
        }

        [Fact]
        public void CannotCreateUserWithNameAlreadyInUseNoMatterLetterCasing()
        {
            User createdUser = services.UserService.CreateUser("Stålberto");
            User duplicateUser = services.UserService.CreateUser(createdUser.Name.ToUpper());

            duplicateUser.Should().BeNull();
        }

        [Fact]
        public void CanGetUserById()
        {
            User createdUser = services.UserService.CreateUser("Stålberto");
            User fetchedUser = services.UserService.GetUserById(createdUser.Id);

            fetchedUser.Should().NotBeNull();
            fetchedUser.Id.Should().Be(createdUser.Id);
            fetchedUser.Name.Should().Be(createdUser.Name);
        }

        [Fact]
        public void ReturnsNullWhenFetchingNonexistentUserById()
        {
            User fetchedUser = services.UserService.GetUserById(Guid.NewGuid());

            fetchedUser.Should().BeNull();
        }

        [Fact]
        public void CanGetUserByNameNoMatterLetterCasing()
        {
            User createdUser = services.UserService.CreateUser("Stålberto");
            User fetchedUser = services.UserService.GetUserByName(createdUser.Name.ToUpper());

            fetchedUser.Should().NotBeNull();
            fetchedUser.Id.Should().Be(createdUser.Id);
            fetchedUser.Name.Should().Be(createdUser.Name);
        }

        [Fact]
        public void ReturnsNullWhenFetchingNonexistentUserByName()
        {
            User fetchedUser = services.UserService.GetUserByName("my-god-thats-jason-bourne");

            fetchedUser.Should().BeNull();
        }
    }
}
