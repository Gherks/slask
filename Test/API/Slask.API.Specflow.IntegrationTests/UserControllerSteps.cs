using Slask.SpecFlow.IntegrationTests.PersistenceTests;
using TechTalk.SpecFlow;

namespace Slask.API.Specflow.IntegrationTests
{
    [Binding, Scope(Feature = "UserController")]
    public class UserControllerSteps : SpecflowCoreSteps
    {
        public UserControllerSteps()
        {
        }

        [When(@"API POST is called with address ""(.*)"" containing body")]
        public void WhenAPIPOSTIsCalledWithAddressContainingBody(string p0, Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the POST result should return status code ""(.*)""")]
        public void ThenThePOSTResultShouldReturnStatusCode(int p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
