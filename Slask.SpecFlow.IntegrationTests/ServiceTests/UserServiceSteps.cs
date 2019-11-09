using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Services;
using Slask.TestCore.SlaskContexts;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.ServiceTests
{
    [Binding, Scope(Feature = "UserService")]
    public class UserServiceSteps : UserServiceStepDefinitions
    {

    }

    public class UserServiceStepDefinitions : InMemoryTestContext
    {
        protected readonly UserService userService;
        protected readonly List<User> createdUsers;
        protected readonly List<User> fetchedUsers;

        public UserServiceStepDefinitions()
        {
            userService = new UserService(SlaskContext);
            createdUsers = new List<User>();
            fetchedUsers = new List<User>();
        }

        [Given(@"a user named ""(.*)"" has been created")]
        [When(@"a user named ""(.*)"" has been created")]
        public void GivenAUserNamedHasBeenCreated(string name)
        {
            createdUsers.Add(userService.CreateUser(name));
        }

        [Given(@"fetching user with user id: (.*)")]
        [When(@"fetching user with user id: (.*)")]
        public void GivenFetchingUserWithUserId(Guid userId)
        {
            fetchedUsers.Add(userService.GetUserById(userId));
        }

        [Given(@"fetching created user (.*) by user id")]
        [When(@"fetching created user (.*) by user id")]
        public void GivenFetchingCreatedUserByUserId(int userIndex)
        {
            Guid userId = createdUsers[userIndex].Id;
            fetchedUsers.Add(userService.GetUserById(userId));
        }

        [When(@"fetching user by user name: ""(.*)""")]
        public void WhenFetchingUserByUserName(string name)
        {
            fetchedUsers.Add(userService.GetUserByName(name));
        }

        [Then(@"created user (.*) should be valid with name: ""(.*)""")]
        public void ThenCreatedUserShouldBeValidWithName(int userIndex, string name)
        {
            CheckUserValidity(createdUsers[userIndex], name);
        }

        [Then(@"fetched user (.*) should be valid with name: ""(.*)""")]
        public void ThenFetchedUserShouldBeValidWithName(int userIndex, string name)
        {
            CheckUserValidity(createdUsers[userIndex], name);
        }

        [Then(@"created user (.*) should be invalid")]
        public void ThenCreatedUserShouldBeInvalid(int userIndex)
        {
            createdUsers[userIndex].Should().BeNull();
        }

        [Then(@"fetched user (.*) should be invalid")]
        public void ThenFetchedUserShouldBeInvalid(int userIndex)
        {
            fetchedUsers[userIndex].Should().BeNull();
        }

        protected void CheckUserValidity(User user, string correctName)
        {
            user.Should().NotBeNull();
            user.Id.Should().NotBeEmpty();
            user.Name.Should().Be(correctName);
        }
    }
}
