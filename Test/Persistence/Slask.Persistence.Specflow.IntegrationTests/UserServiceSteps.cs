using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Services;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.PersistenceTests
{
    [Binding, Scope(Feature = "UserService")]
    public class UserServiceSteps : UserServiceStepDefinitions
    {

    }

    public class UserServiceStepDefinitions
    {
        protected readonly UserService _userService;
        protected readonly List<User> _createdUsers;
        protected readonly List<User> _fetchedUsers;

        public UserServiceStepDefinitions()
        {
            _userService = new UserService(InMemoryContextCreator.Create());
            _createdUsers = new List<User>();
            _fetchedUsers = new List<User>();
        }

        [Given(@"a user named ""(.*)"" has been created")]
        [When(@"a user named ""(.*)"" has been created")]
        public void GivenAUserNamedHasBeenCreated(string name)
        {
            _createdUsers.Add(_userService.CreateUser(name));
        }

        [Given(@"fetching user with user id: (.*)")]
        [When(@"fetching user with user id: (.*)")]
        public void GivenFetchingUserWithUserId(Guid userId)
        {
            _fetchedUsers.Add(_userService.GetUserById(userId));
        }

        [Given(@"fetching created user (.*) by user id")]
        [When(@"fetching created user (.*) by user id")]
        public void GivenFetchingCreatedUserByUserId(int userIndex)
        {
            Guid userId = _createdUsers[userIndex].Id;
            _fetchedUsers.Add(_userService.GetUserById(userId));
        }

        [When(@"fetching user by user name: ""(.*)""")]
        public void WhenFetchingUserByUserName(string name)
        {
            _fetchedUsers.Add(_userService.GetUserByName(name));
        }

        [Then(@"created user (.*) should be valid with name: ""(.*)""")]
        public void ThenCreatedUserShouldBeValidWithName(int userIndex, string name)
        {
            CheckUserValidity(_createdUsers[userIndex], name);
        }

        [Then(@"fetched user (.*) should be valid with name: ""(.*)""")]
        public void ThenFetchedUserShouldBeValidWithName(int userIndex, string name)
        {
            CheckUserValidity(_createdUsers[userIndex], name);
        }

        [Then(@"created user (.*) should be invalid")]
        public void ThenCreatedUserShouldBeInvalid(int userIndex)
        {
            _createdUsers[userIndex].Should().BeNull();
        }

        [Then(@"fetched user (.*) should be invalid")]
        public void ThenFetchedUserShouldBeInvalid(int userIndex)
        {
            _fetchedUsers[userIndex].Should().BeNull();
        }

        protected static void CheckUserValidity(User user, string correctName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Should().NotBeNull();
            user.Id.Should().NotBeEmpty();
            user.Name.Should().Be(correctName);
        }
    }
}
