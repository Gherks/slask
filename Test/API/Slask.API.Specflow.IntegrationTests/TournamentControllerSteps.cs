using Newtonsoft.Json;
using Slask.API.Specflow.IntegrationTests.Utilities;
using Slask.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Slask.API.Specflow.IntegrationTests
{
    [Binding, Scope(Feature = "TournamentController")]
    public class TournamentControllerSteps : ControllerStepsBase
    {
        private const int _acceptableInaccuracy = 2000;

        public TournamentControllerSteps(InMemoryDatabaseWebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Given(@"GET request is sent to fetch tournaments")]
        [When(@"GET request is sent to fetch tournaments")]
        public async Task GivenGETRequestIsSentToFetchTournaments()
        {
            _response = await _client.GetAsync("api/tournaments");
        }

        [Given(@"GET request is sent to fetch tournament named ""(.*)""")]
        [When(@"GET request is sent to fetch tournament named ""(.*)""")]
        public async Task GivenGETRequestIsSentToFetchTournamentNamed(string name)
        {
            _response = await _client.GetAsync("api/tournaments/" + name);
        }

        [Given(@"GET request is sent to fetch tournament named ""(.*)"" by user id")]
        [When(@"GET request is sent to fetch tournament named ""(.*)"" by user id")]
        public async Task GivenGETRequestIsSentToFetchTournamentNamedByUserId(string name)
        {
            await GivenGETRequestIsSentToFetchTournamentNamed(name);

            List<TournamentDto> tournamentDtos = await JsonResponseToObjectList<TournamentDto>();
            TournamentDto tournamentDto = tournamentDtos.FirstOrDefault(dto => dto.Name == name);

            if (tournamentDto != null)
            {
                _response = await _client.GetAsync("api/tournaments/" + tournamentDto.Id);
            }
        }

        [Given(@"POST request is sent to create a tournament named ""(.*)""")]
        [When(@"POST request is sent to create a tournament named ""(.*)""")]
        public async Task GivenPOSTRequestIsSentToCreateATournamentNamed(string name)
        {
            string jsonContent = JsonConvert.SerializeObject(new { tournamentName = name });
            HttpContent content = CreateHttpContent(jsonContent);

            _response = await _client.PostAsync("api/tournaments", content);
        }

        [Given(@"bare tournament DTOs are extracted from response")]
        [When(@"bare tournament DTOs are extracted from response")]
        public async Task WhenBareTournamentDTOsAreExtractedFromResponse()
        {
            _bareTournamentDtos.AddRange(await JsonResponseToObjectList<BareTournamentDto>());
        }

        [Given(@"tournament DTOs are extracted from response")]
        [When(@"tournament DTOs are extracted from response")]
        public async Task WhenTournamentDTOsAreExtractedFromResponse()
        {
            _tournamentDtos.AddRange(await JsonResponseToObjectList<TournamentDto>());
        }
    }
}
