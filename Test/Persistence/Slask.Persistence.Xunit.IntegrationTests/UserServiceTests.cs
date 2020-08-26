using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Services;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Persistence.Xunit.IntegrationTests
{
    public class UserServiceTests
    {
        private readonly string testDatabaseName;

        public UserServiceTests()
        {
            testDatabaseName = Guid.NewGuid().ToString();
        }

        [Fact]
        public void CanCreateUser()
        {
            string userName = "Guggelito";

            using (UserService userService = CreateUserService())
            {
                User user = userService.CreateUser(userName);
                userService.Save();

                user.Should().NotBeNull();
                user.Id.Should().NotBeEmpty();
                user.Name.Should().Be(userName);
            }
        }

        [Fact]
        public void CannotCreateUserWithEmptyName()
        {
            using (UserService userService = CreateUserService())
            {
                User user = userService.CreateUser("");
                userService.Save();

                user.Should().BeNull();
            }
        }

        [Fact]
        public void CannotCreateUserWithNameAlreadyInUseNoMatterLetterCasing()
        {
            using (UserService userService = CreateUserService())
            {
                User createdUser = userService.CreateUser("Stålberto");
                userService.Save();

                User duplicateUser = userService.CreateUser(createdUser.Name.ToUpper());
                userService.Save();

                duplicateUser.Should().BeNull();
            }
        }

        [Fact]
        public void CanRenameUser()
        {
            string username1 = "Stålberto";
            string username2 = "Guggelito";

            using (UserService userService = CreateUserService())
            {
                User user = userService.CreateUser(username1);
                userService.Save();

                userService.RenameUser(user.Id, username2);
                user.Name.Should().Be(username2);
            }
        }

        [Fact]
        public void CannotRenameUserWithNameAlreadyInUseNotMatterLetterCasing()
        {
            string username1 = "Stålberto";
            string username2 = "Guggelito";

            using (UserService userService = CreateUserService())
            {
                User user1 = userService.CreateUser(username1);
                User user2 = userService.CreateUser(username2);
                userService.Save();

                userService.RenameUser(user2.Id, username1.ToUpper());
                userService.Save();

                User after_renamed_user1 = userService.GetUserById(user1.Id);
                User after_renamed_user2 = userService.GetUserById(user2.Id);

                after_renamed_user1.Id.Should().Be(user1.Id);
                after_renamed_user1.Name.Should().Be(username1);
                after_renamed_user2.Id.Should().Be(user2.Id);
                after_renamed_user2.Name.Should().Be(username2);
            }
        }

        [Fact]
        public void CannotRenameUserToNameThatOnlyDiffersInWhitespaceToNameAlreadyInUse()
        {
            string username1 = "Stålberto";
            string username2 = "Guggelito";

            using (UserService userService = CreateUserService())
            {
                User user1 = userService.CreateUser(username1);
                User user2 = userService.CreateUser(username2);
                userService.Save();

                userService.RenameUser(user2.Id, username1 + " ");
                userService.Save();

                User after_renamed_user1 = userService.GetUserById(user1.Id);
                User after_renamed_user2 = userService.GetUserById(user2.Id);

                after_renamed_user1.Id.Should().Be(user1.Id);
                after_renamed_user1.Name.Should().Be(username1);
                after_renamed_user2.Id.Should().Be(user2.Id);
                after_renamed_user2.Name.Should().Be(username2);
            }
        }

        [Fact]
        public void CanGetListOfAllUsers()
        {
            List<string> userNames = new List<string>() { "Stålberto", "Guggelito", "Bönis", "Kimmieboi" };

            using (UserService userService = CreateUserService())
            {
                foreach (string userName in userNames)
                {
                    userService.CreateUser(userName);
                }
                userService.Save();
            }

            using (UserService userService = CreateUserService())
            {
                IEnumerable<User> users = userService.GetUsers();

                users.Should().HaveCount(userNames.Count);
                foreach (string userName in userNames)
                {
                    users.FirstOrDefault(user => user.Name == userName).Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void CanFetchUserById()
        {
            using (UserService userService = CreateUserService())
            {
                User createdUser = userService.CreateUser("Stålberto");
                userService.Save();

                User fetchedUser = userService.GetUserById(createdUser.Id);
                fetchedUser.Id.Should().Be(createdUser.Id);
                fetchedUser.Name.Should().Be(createdUser.Name);
            }
        }

        [Fact]
        public void CanFetchUserByNameNoMatterLetterCasing()
        {
            using (UserService userService = CreateUserService())
            {
                User createdUser = userService.CreateUser("Stålberto");
                userService.Save();

                User fetchedUser = userService.GetUserByName(createdUser.Name.ToUpper());

                fetchedUser.Should().NotBeNull();
                fetchedUser.Id.Should().Be(createdUser.Id);
                fetchedUser.Name.Should().Be(createdUser.Name);
            }
        }

        [Fact]
        public void ReturnsNullWhenFetchingNonexistentUserById()
        {
            using (UserService userService = CreateUserService())
            {
                User fetchedUser = userService.GetUserById(Guid.NewGuid());

                fetchedUser.Should().BeNull();
            }
        }

        [Fact]
        public void ReturnsNullWhenFetchingNonexistentUserByName()
        {
            using (UserService userService = CreateUserService())
            {
                User fetchedUser = userService.GetUserByName("my-god-thats-jason-bourne");

                fetchedUser.Should().BeNull();
            }
        }

        private UserService CreateUserService()
        {
            return new UserService(InMemoryContextCreator.Create(testDatabaseName));
        }
    }
}
