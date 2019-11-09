using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Services;
using Slask.UnitTests;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.ServiceTests
{
    [Binding, Scope(Feature = "TournamentService")]
    public class TournamentServiceSteps : TournamentServiceStepDefinitions
    {

    }

    public class TournamentServiceStepDefinitions : UserServiceStepDefinitions
    {
        protected readonly TournamentService tournamentService;
        protected readonly List<Tournament> createdTournaments;
        protected readonly List<Tournament> fetchedTournaments;

        public TournamentServiceStepDefinitions()
        {
            tournamentService = new TournamentService(SlaskContext);
            createdTournaments = new List<Tournament>();
            fetchedTournaments = new List<Tournament>();
        }

        [Given(@"a tournament named ""(.*)"" has been created")]
        [When(@"a tournament named ""(.*)"" has been created")]
        public void GivenATournamentNamedHasBeenCreated(string name)
        {
            createdTournaments.Add(tournamentService.CreateTournament(name));
        }

        [Given(@"fetching tournament with tournament id: (.*)")]
        [When(@"fetching tournament with tournament id: (.*)")]
        public void GivenFetchingTournamentWithTournamentId(Guid tournamentId)
        {
            fetchedTournaments.Add(tournamentService.GetTournamentById(tournamentId));
        }

        [Given(@"fetching created tournament (.*) by tournament id")]
        [When(@"fetching created tournament (.*) by tournament id")]
        public void GivenFetchingTournamentWithTournamentId(int tournamentIndex)
        {
            Guid tournamentId = createdTournaments[tournamentIndex].Id;
            fetchedTournaments.Add(tournamentService.GetTournamentById(tournamentId));
        }

        [When(@"fetching tournament by tournament name: ""(.*)""")]
        public void WhenFetchingTournamentByTournamentName(string name)
        {
            fetchedTournaments.Add(tournamentService.GetTournamentByName(name));
        }

        [When(@"created tournament (.*) is renamed to: ""(.*)""")]
        public void WhenCreatedTournamentIsRenamedTo(int tournamentIndex, string newName)
        {
            Guid tournamentId = createdTournaments[tournamentIndex].Id;
            tournamentService.RenameTournament(tournamentId, newName);
        }

        [When(@"user ""(.*)"" is added to created tournament ""(.*)""")]
        public void WhenUserIsAddedToCreatedTournament(string userName, string tournamentName)
        {
            Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
            tournament.AddBetter(userService.GetUserByName(userName));
        }

        [Then(@"created tournament (.*) should be valid with name: ""(.*)""")]
        public void ThenCreatedTournamentShouldBeValidWithName(int tournamentIndex, string name)
        {
            CheckTournamentValidity(createdTournaments[tournamentIndex], name);
        }

        [Then(@"fetched tournament (.*) should be valid with name: ""(.*)""")]
        public void ThenFetchedTournamentShouldBeValidWithName(int tournamentIndex, string name)
        {
            CheckTournamentValidity(fetchedTournaments[tournamentIndex], name);
        }

        [Then(@"created tournament (.*) should be invalid")]
        public void ThenCreatedTournamentShouldBeNull(int tournamentIndex)
        {
            createdTournaments[tournamentIndex].Should().BeNull();
        }

        [Then(@"fetched tournament (.*) should be invalid")]
        public void ThenFetchedTournamentShouldBeNull(int tournamentIndex)
        {
            fetchedTournaments[tournamentIndex].Should().BeNull();
        }

        [Then(@"created tournament (.*) better (.*) should be valid with name: ""(.*)""")]
        public void CreatedTournamentBetterShouldBeValidWithName(int tournamentIndex, int betterIndex, string correctName)
        {
            Tournament tournament = createdTournaments[tournamentIndex];
            tournament.Betters[betterIndex].Should().NotBeNull();
            CheckUserValidity(tournament.Betters[betterIndex].User, correctName);
        }

        protected void CheckTournamentValidity(Tournament tournament, string correctName)
        {
            tournament.Should().NotBeNull();
            tournament.Id.Should().NotBeEmpty();
            tournament.Name.Should().Be(correctName);
            tournament.Rounds.Should().BeEmpty();
            tournament.PlayerReferences.Should().BeEmpty();
            tournament.Betters.Should().BeEmpty();
            tournament.Settings.Should().BeEmpty();
            tournament.MiscBetCatalogue.Should().BeEmpty();
        }
    }
}
