using FluentAssertions;
using Newtonsoft.Json;
using Slask.API.Specflow.IntegrationTests.Utilities;
using Slask.Common;
using Slask.Dto;
using System.Collections.Generic;
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

        [Given(@"POST request is sent to create a tournament named ""(.*)""")]
        [When(@"POST request is sent to create a tournament named ""(.*)""")]
        public async Task GivenPOSTRequestIsSentToCreateATournamentNamed(string name)
        {
            string jsonContent = JsonConvert.SerializeObject(new { tournamentName = name });
            HttpContent content = CreateHttpContent(jsonContent);

            _response = await _client.PostAsync("api/tournaments", content);
        }

        [Then(@"response should contain tournaments ""(.*)""")]
        public async Task ThenResponseShouldContainTournaments(string commaSeparatedTournamentNames)
        {
            List<string> tournamentNames = commaSeparatedTournamentNames.ToStringList(",");

            List<BareTournamentDto> bareTournamentDtos = await JsonResponseToObjectList<BareTournamentDto>();

            bareTournamentDtos.Should().HaveCount(tournamentNames.Count);

            for (int index = 0; index < bareTournamentDtos.Count; ++index)
            {
                BareTournamentDto bareTournamentDto = bareTournamentDtos[index];

                bareTournamentDto.Id.Should().NotBeEmpty();
                bareTournamentDto.Name.Should().Be(tournamentNames[index]);
                bareTournamentDto.Created.Should().BeCloseTo(SystemTime.Now, _acceptableInaccuracy);
            }
        }
    }
}
