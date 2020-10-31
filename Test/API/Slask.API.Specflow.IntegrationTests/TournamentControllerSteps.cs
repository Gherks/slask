using Slask.API.Specflow.IntegrationTests.Utilities;
using Slask.Dto;
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

        [Given(@"bare tournament DTOs are extracted from response")]
        [When(@"bare tournament DTOs are extracted from response")]
        public async Task WhenBareTournamentDTOsAreExtractedFromResponse()
        {
            string responseContent = await _response.Content.ReadAsStringAsync();

            _bareTournamentDtos.AddRange(JsonResponseToObjectList<BareTournamentDto>(responseContent));
        }

        [Given(@"tournament DTOs are extracted from response")]
        [When(@"tournament DTOs are extracted from response")]
        public async Task WhenTournamentDTOsAreExtractedFromResponse()
        {
            string responseContent = await _response.Content.ReadAsStringAsync();

            _tournamentDtos.AddRange(JsonResponseToObjectList<TournamentDto>(responseContent));
        }
    }
}
