using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
        protected List<Better> fetchedBetters;

        public TournamentServiceStepDefinitions()
        {
            tournamentService = new TournamentService(SlaskContext);
            createdTournaments = new List<Tournament>();
            fetchedTournaments = new List<Tournament>();
            fetchedBetters = new List<Better>();
        }

        [Given(@"a tournament named ""(.*)"" has been created")]
        [When(@"a tournament named ""(.*)"" has been created")]
        public Tournament GivenATournamentNamedHasBeenCreated(string name)
        {
            createdTournaments.Add(tournamentService.CreateTournament(name));
            return createdTournaments.Last();
        }

        [Given(@"users ""(.*)"" has been added to tournament with name: ""(.*)""")]
        [When(@"users ""(.*)"" has been added to tournament with name: ""(.*)""")]
        public void GivenATournamentNamedWithBettersHasBeenCreated(string commaSeparatedUserNames, string tournamentName)
        {
            List<string> userNames = StringUtility.ToStringList(commaSeparatedUserNames, ",");
            foreach (string userName in userNames)
            {
                createdUsers.Add(userService.CreateUser(userName));
            }

            Tournament tournament = GivenATournamentNamedHasBeenCreated(tournamentName);

            foreach (User user in createdUsers)
            {
                tournament.AddBetter(user);
            }
        }

        [Given(@"fetching tournament with tournament id: (.*)")]
        [When(@"fetching tournament with tournament id: (.*)")]
        public Tournament GivenFetchingTournamentWithTournamentId(Guid tournamentId)
        {
            fetchedTournaments.Add(tournamentService.GetTournamentById(tournamentId));
            return fetchedTournaments.Last();
        }

        [Given(@"fetching created tournament (.*) by tournament id")]
        [When(@"fetching created tournament (.*) by tournament id")]
        public void GivenFetchingTournamentWithTournamentId(int tournamentIndex)
        {
            Guid tournamentId = createdTournaments[tournamentIndex].Id;
            fetchedTournaments.Add(GivenFetchingTournamentWithTournamentId(tournamentId));
        }

        [Given(@"user ""(.*)"" is added to created tournament ""(.*)""")]
        [When(@"user ""(.*)"" is added to created tournament ""(.*)""")]
        public void GivenUserIsAddedToCreatedTournament(string userName, string tournamentName)
        {
            Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
            tournament.AddBetter(userService.GetUserByName(userName));
        }

        [Given(@"a tournament with player references added with name ""(.*)"" has been created")]
        public void GivenATournamentWithPlayerReferencesAddedWithNameHasBeenCreated(string name)
        {
            Tournament tournament = GivenATournamentNamedHasBeenCreated(name);

            RoundBase bracketRound = tournament.AddBracketRound("BracketRound", 3);
            BracketGroup bracketGroup = (BracketGroup)bracketRound.AddGroup();

            bracketGroup.AddPlayerReference("Maru");
            bracketGroup.AddPlayerReference("Stork");
            bracketGroup.AddPlayerReference("Taeja");
            bracketGroup.AddPlayerReference("Rain");
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

        [When(@"fetching betters from created tournament (.*) by tournament id")]
        public void WhenFetchingUsersFromCreatedTournament(int tournamentIndex)
        {
            Guid tournamentId = createdTournaments[tournamentIndex].Id;
            fetchedBetters = tournamentService.GetBettersByTournamentId(tournamentId);
        }

        [When(@"fetching betters from tournament by tournament name: ""(.*)""")]
        public void FetchingBettersFromTournamentByTournamentName(string tournamentName)
        {
            fetchedBetters = tournamentService.GetBettersByTournamentName(tournamentName);
        }

        [When(@"added players ""(.*)"" to tournament with name ""(.*)""")]
        public void WhenAddedPlayersToTournamentWithName(string commaSeparatedPlayerNames, string tournamentName)
        {
            Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");
            foreach (string playerName in playerNames)
            {
                tournament.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }

        [When(@"fetching player references ""(.*)"" from created tournament (.*) by tournament id")]
        public void WhenFetchingPlayerReferencesFromCreatedTournamentByTournamentId(string commaSeparatedPlayerNames, int tournamentIndex)
        {
            Tournament tournament = createdTournaments[tournamentIndex];
            List<PlayerReference> playerReferences = tournamentService.GetPlayerReferencesByTournamentId(tournament.Id);

            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");
            foreach (string playerName in playerNames)
            {
                playerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }

        [When(@"fetching player references ""(.*)"" from tournament by tournament name: ""(.*)""")]
        public void WhenFetchingPlayerReferencesFromTournamentByTournamentName(string commaSeparatedPlayerNames, string tournamentName)
        {
            List<PlayerReference> playerReferences = tournamentService.GetPlayerReferencesByTournamentName(tournamentName);

            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");
            foreach (string playerName in playerNames)
            {
                playerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
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

        [Then(@"created tournament (.*), better (.*), should be valid with name: ""(.*)""")]
        public void CreatedTournamentBetterShouldBeValidWithName(int tournamentIndex, int betterIndex, string correctName)
        {
            Tournament tournament = createdTournaments[tournamentIndex];
            tournament.Betters[betterIndex].Should().NotBeNull();
            CheckUserValidity(tournament.Betters[betterIndex].User, correctName);
        }
        
        [Then(@"fetched tournament (.*), better (.*), should be valid with name: ""(.*)""")]
        public void FetchedTournamentBetterShouldBeValidWithname(int tournamentIndex, int betterIndex, string correctName)
        {
            Tournament tournament = fetchedTournaments[tournamentIndex];
            tournament.Betters[betterIndex].Should().NotBeNull();
            CheckUserValidity(tournament.Betters[betterIndex].User, correctName);
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

        [Then(@"created tournament (.*), better (.*), should be invalid")]
        public void CreatedTournamentBetterShouldBeInvalid(int tournamentIndex, int betterIndex)
        {
            Tournament tournament = createdTournaments[tournamentIndex];
            tournament.Betters[betterIndex].Should().BeNull();
        }

        [Then(@"created tournament (.*) should have (.*) betters")]
        public void ThenCreatedTournamentShouldHaveBetter(int tournamentIndex, int betterAmount)
        {
            Tournament tournament = createdTournaments[tournamentIndex];
            tournament.Betters.Should().HaveCount(betterAmount);
        }

        [Then(@"created tournament (.*) should have (.*) player references with names: ""(.*)""")]
        public void ThenTournamentShouldHavePlayerReferencesWithNames(int tournamentIndex, int playerAmount, string commaSeparetedPlayerNames)
        {
            Tournament tournament = createdTournaments[tournamentIndex];
            tournament.PlayerReferences.Should().NotBeNull();
            tournament.PlayerReferences.Should().HaveCount(playerAmount);

            List<string> playerNames = StringUtility.ToStringList(commaSeparetedPlayerNames, ",");
            foreach (string playerName in playerNames)
            {
                tournament.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }

        protected static void CheckTournamentValidity(Tournament tournament, string correctName)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException(nameof(tournament));
            }

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
