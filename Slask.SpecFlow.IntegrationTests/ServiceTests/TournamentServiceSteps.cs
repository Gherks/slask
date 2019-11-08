using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Services;
using Slask.UnitTests;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.ServiceTests
{
    [Binding, Scope(Feature = "TournamentService")]
    public class TournamentServiceSteps
    {
        private TournamentService tournamentService;
        private Tournament tournament;

        [Given(@"a TournamentService has been created")]
        public void GivenATournamentServiceHasBeenCreated()
        {
            tournamentService = new TournamentService(InMemorySlaskContextCreator.Create());
        }
        
        [Given(@"a tournament named ""(.*)"" has been created")]
        public void GivenATournamentNamedHasBeenCreated(string name)
        {
            tournament = tournamentService.CreateTournament(name);
        }

        [Then(@"tournament should be valid with name ""(.*)""")]
        public void ThenTournamentShouldBeValidWithName(string name)
        {
            tournament.Should().NotBeNull();
            tournament.Should().NotBeNull();

            tournament.Id.Should().NotBeEmpty();
            tournament.Name.Should().Be(name);
            tournament.Rounds.Should().BeEmpty();
            tournament.PlayerReferences.Should().BeEmpty();
            tournament.Betters.Should().BeEmpty();
            tournament.Settings.Should().BeEmpty();
            tournament.MiscBetCatalogue.Should().BeEmpty();
        }

        [Then(@"tournament should be null")]
        public void ThenTournamentShouldBeNull()
        {
            tournament.Should().BeNull();
        }
    }
}
