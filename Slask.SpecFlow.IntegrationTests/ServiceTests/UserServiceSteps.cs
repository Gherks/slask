using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Services;
using Slask.UnitTests;
using System;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.ServiceTests
{
    [Binding]
    public class UserServiceSteps
    {
        private UserService userService;
        private User user;
        private Guid userId;
        public UserServiceSteps()
        {
            userService = null;
            user = null;
            userId = Guid.Empty;
        }

        [Given(@"a UserService has been created")]
        public void GivenAUserServiceHasBeenCreated()
        {
            userService = new UserService(InMemorySlaskContextCreator.Create());
        }

        [Given(@"a user named ""(.*)"" has been created")]
        [When(@"a user named ""(.*)"" has been created")]
        public void GivenAUserNamedHasBeenCreated(string name)
        {
            user = userService.CreateUser(name);

            if (user != null)
            {
                userId = user.Id;
            }
        }

        [Given(@"getting user by user id")]
        [When(@"getting user by user id")]
        public void GivenGettingUserByUserId()
        {
            user = userService.GetUserById(userId);
        }

        [When(@"getting user by user name ""(.*)""")]
        public void WhenGettingUserByUserName(string name)
        {
            user = userService.GetUserByName(name);
        }

        [Then(@"user should be valid and named ""(.*)""")]
        public void ThenUserShouldBeValidAndNamed(string name)
        {
            user.Should().NotBeNull();
            user.Id.Should().NotBeEmpty();
            user.Name.Should().Be(name);
        }

        [Then(@"user should be invalid")]
        public void ThenUserShouldBeInvalid()
        {
            user.Should().BeNull();
        }
    }
}
