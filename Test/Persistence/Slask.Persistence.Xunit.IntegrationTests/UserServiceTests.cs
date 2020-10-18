using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Repositories;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Persistence.Xunit.IntegrationTests
{
    public class UserRepositoryTests
    {
        private readonly string testDatabaseName = "InMemoryTestDatabase_" + Guid.NewGuid().ToString();

        [Fact]
        public void CanCreateUser()
        {
            string userName = "Guggelito";

            using (UserRepository userRepository = CreateuserRepository())
            {
                User user = userRepository.CreateUser(userName);
                userRepository.Save();

                user.Should().NotBeNull();
                user.Id.Should().NotBeEmpty();
                user.Name.Should().Be(userName);
            }
        }

        [Fact]
        public void CannotCreateUserWithEmptyName()
        {
            using (UserRepository userRepository = CreateuserRepository())
            {
                User user = userRepository.CreateUser("");
                userRepository.Save();

                user.Should().BeNull();
            }
        }

        [Fact]
        public void CannotCreateUserWithNameAlreadyInUseNoMatterLetterCasing()
        {
            using (UserRepository userRepository = CreateuserRepository())
            {
                User createdUser = userRepository.CreateUser("Stålberto");
                userRepository.Save();

                User duplicateUser = userRepository.CreateUser(createdUser.Name.ToUpper());
                userRepository.Save();

                duplicateUser.Should().BeNull();
            }
        }

        [Fact]
        public void CanRenameUser()
        {
            string username1 = "Stålberto";
            string username2 = "Guggelito";

            using (UserRepository userRepository = CreateuserRepository())
            {
                User user = userRepository.CreateUser(username1);
                userRepository.Save();

                userRepository.RenameUser(user.Id, username2);
                user.Name.Should().Be(username2);
            }
        }

        [Fact]
        public void CannotRenameUserWithNameAlreadyInUseNotMatterLetterCasing()
        {
            string username1 = "Stålberto";
            string username2 = "Guggelito";

            using (UserRepository userRepository = CreateuserRepository())
            {
                User user1 = userRepository.CreateUser(username1);
                User user2 = userRepository.CreateUser(username2);
                userRepository.Save();

                userRepository.RenameUser(user2.Id, username1.ToUpper());
                userRepository.Save();

                User after_renamed_user1 = userRepository.GetUserById(user1.Id);
                User after_renamed_user2 = userRepository.GetUserById(user2.Id);

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

            using (UserRepository userRepository = CreateuserRepository())
            {
                User user1 = userRepository.CreateUser(username1);
                User user2 = userRepository.CreateUser(username2);
                userRepository.Save();

                userRepository.RenameUser(user2.Id, username1 + " ");
                userRepository.Save();

                User after_renamed_user1 = userRepository.GetUserById(user1.Id);
                User after_renamed_user2 = userRepository.GetUserById(user2.Id);

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

            using (UserRepository userRepository = CreateuserRepository())
            {
                foreach (string userName in userNames)
                {
                    userRepository.CreateUser(userName);
                }
                userRepository.Save();
            }

            using (UserRepository userRepository = CreateuserRepository())
            {
                IEnumerable<User> users = userRepository.GetUsers();

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
            using (UserRepository userRepository = CreateuserRepository())
            {
                User createdUser = userRepository.CreateUser("Stålberto");
                userRepository.Save();

                User fetchedUser = userRepository.GetUserById(createdUser.Id);
                fetchedUser.Id.Should().Be(createdUser.Id);
                fetchedUser.Name.Should().Be(createdUser.Name);
            }
        }

        [Fact]
        public void CanFetchUserByNameNoMatterLetterCasing()
        {
            using (UserRepository userRepository = CreateuserRepository())
            {
                User createdUser = userRepository.CreateUser("Stålberto");
                userRepository.Save();

                User fetchedUser = userRepository.GetUserByName(createdUser.Name.ToUpper());

                fetchedUser.Should().NotBeNull();
                fetchedUser.Id.Should().Be(createdUser.Id);
                fetchedUser.Name.Should().Be(createdUser.Name);
            }
        }

        [Fact]
        public void ReturnsNullWhenFetchingNonexistentUserById()
        {
            using (UserRepository userRepository = CreateuserRepository())
            {
                User fetchedUser = userRepository.GetUserById(Guid.NewGuid());

                fetchedUser.Should().BeNull();
            }
        }

        [Fact]
        public void ReturnsNullWhenFetchingNonexistentUserByName()
        {
            using (UserRepository userRepository = CreateuserRepository())
            {
                User fetchedUser = userRepository.GetUserByName("my-god-thats-jason-bourne");

                fetchedUser.Should().BeNull();
            }
        }

        private UserRepository CreateuserRepository()
        {
            return new UserRepository(InMemoryContextCreator.Create(testDatabaseName));
        }
    }
}
