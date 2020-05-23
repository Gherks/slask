using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Services;
using Slask.TestCore;
using System;
using Xunit;

namespace Slask.UnitTests.ServiceTests
{
    public class UserServiceTests
    {
        private readonly UserService userService;

        public UserServiceTests()
        {
            userService = new UserService(InMemoryContextCreator.Create());
        }

        [Fact]
        public void CanCreateUser()
        {
            string userName = "Guggelito";

            User user = userService.CreateUser(userName);

            user.Should().NotBeNull();
            user.Id.Should().NotBeEmpty();
            user.Name.Should().Be(userName);
        }

        [Fact]
        public void CannotCreateUserWithEmptyName()
        {
            User user = userService.CreateUser("");

            user.Should().BeNull();
        }

        [Fact]
        public void CannotCreateUserWithNameAlreadyInUseNoMatterLetterCasing()
        {
            User createdUser = userService.CreateUser("Stålberto");
            User duplicateUser = userService.CreateUser(createdUser.Name.ToUpper());

            duplicateUser.Should().BeNull();
        }

        [Fact]
        public void CanGetUserById()
        {
            User createdUser = userService.CreateUser("Stålberto");
            User fetchedUser = userService.GetUserById(createdUser.Id);

            fetchedUser.Should().NotBeNull();
            fetchedUser.Id.Should().Be(createdUser.Id);
            fetchedUser.Name.Should().Be(createdUser.Name);
        }

        [Fact]
        public void ReturnsNullWhenFetchingNonexistentUserById()
        {
            User fetchedUser = userService.GetUserById(Guid.NewGuid());

            fetchedUser.Should().BeNull();
        }

        [Fact]
        public void CanGetUserByNameNoMatterLetterCasing()
        {
            User createdUser = userService.CreateUser("Stålberto");
            User fetchedUser = userService.GetUserByName(createdUser.Name.ToUpper());

            fetchedUser.Should().NotBeNull();
            fetchedUser.Id.Should().Be(createdUser.Id);
            fetchedUser.Name.Should().Be(createdUser.Name);
        }

        [Fact]
        public void ReturnsNullWhenFetchingNonexistentUserByName()
        {
            User fetchedUser = userService.GetUserByName("my-god-thats-jason-bourne");

            fetchedUser.Should().BeNull();
        }
    }
}
