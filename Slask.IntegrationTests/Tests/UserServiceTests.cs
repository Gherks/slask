using Slask.TestCore;
using Xunit;

namespace Slask.IntegrationTests.Tests
{
    [Collection("Integration test collection")]
    public class UserServiceTests
    {
        private UserServiceContext GivenServices()
        {
            return UserServiceContext.GivenServices(new IntegrationTestSlaskContextCreator());
        }
    }
}
