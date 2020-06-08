using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Services;
using Slask.TestCore;
using System;
using Xunit;

namespace Slask.Xunit.IntegrationTests.PersistenceTests.ServiceTests
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
        public void CanRenameUser()
        {
            string username1 = "Stålberto";
            string username2 = "Guggelito";

            User user = userService.CreateUser(username1);
            userService.RenameUser(user.Id, username2);

            user.Name.Should().Be(username2);
        }

        [Fact]
        public void CannotRenameUserWithNameAlreadyInUseNotMatterLetterCasing()
        {
            string username1 = "Stålberto";
            string username2 = "Guggelito";

            User user1 = userService.CreateUser(username1);
            User user2 = userService.CreateUser(username2);

            userService.RenameUser(user2.Id, username1.ToUpper());

            user1.Name.Should().Be(username1);
            user2.Name.Should().Be(username2);
        }

        [Fact]
        public void CannotRenameUserToNameThatOnlyDiffersInWhitespaceToNameAlreadyInUse()
        {
            string username1 = "Stålberto";
            string username2 = "Guggelito";

            User user1 = userService.CreateUser(username1);
            User user2 = userService.CreateUser(username2);

            userService.RenameUser(user2.Id, username1 + " ");

            user1.Name.Should().Be(username1);
            user2.Name.Should().Be(username2);
        }

        [Fact]
        public void CanFetchUserById()
        {
            User createdUser = userService.CreateUser("Stålberto");
            User fetchedUser = userService.GetUserById(createdUser.Id);

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
        public void CanFetchUserByNameNoMatterLetterCasing()
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
