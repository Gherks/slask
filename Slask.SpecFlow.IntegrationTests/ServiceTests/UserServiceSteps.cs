using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Services;
using Slask.UnitTests;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.ServiceTests
{
    [Binding]
    public class UserServiceSteps
    {
        private UserService userService;
        private User user;

        [Given(@"a UserService has been created")]
        public void GivenAUserServiceHasBeenCreated()
        {
            userService = new UserService(InMemorySlaskContextCreator.Create());
        }
        
        [Given(@"a user named ""(.*)"" has been created")]
        public void GivenAUserNamedHasBeenCreated(string name)
        {
            user = userService.CreateUser(name);
        }
        
        [Then(@"user should be valid and named ""(.*)""")]
        public void ThenUserShouldBeValidAndNamed(string name)
        {
            user.Should().NotBeNull();
            user.Id.Should().NotBeEmpty();
            user.Name.Should().Be(name);
        }
        
        [Then(@"user should be null")]
        public void ThenUserShouldBeNull()
        {
            user.Should().BeNull();
        }
    }
}
