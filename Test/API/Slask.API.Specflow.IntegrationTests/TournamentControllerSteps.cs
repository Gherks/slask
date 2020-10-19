using Newtonsoft.Json;
using Slask.API.Specflow.IntegrationTests.Utilities;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Slask.API.Specflow.IntegrationTests
{
    [Binding, Scope(Feature = "TournamentController")]
    public class TournamentControllerSteps : ControllerStepsBase
    {
        public TournamentControllerSteps(InMemoryDatabaseWebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Given(@"POST request is sent to create a tournament named ""(.*)""")]
        [When(@"POST request is sent to create a tournament named ""(.*)""")]
        public async Task GivenPOSTRequestIsSentToCreateATournamentNamed(string name)
        {
            string jsonContent = JsonConvert.SerializeObject(new { tournamentName = name });
            HttpContent content = CreateHttpContent(jsonContent);

            _response = await _client.PostAsync("api/tournaments", content);
        }
    }
}
