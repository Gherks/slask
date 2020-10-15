using TechTalk.SpecFlow;

namespace Slask.API.Specflow.IntegrationTests
{
    [Binding, Scope(Feature = "UserController")]
    public class UserControllerSteps : APIControllerSteps
    {
        public UserControllerSteps(InMemoryDatabaseWebApplicationFactory<Startup> factory)
            : base(factory)
        {

        }
    }
}
