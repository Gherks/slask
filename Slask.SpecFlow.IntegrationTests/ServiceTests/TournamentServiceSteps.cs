using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Persistence.Services;
using Slask.TestCore;
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
        protected List<Better> createdBetters;
        protected List<Better> fetchedBetters;
        protected readonly List<RoundBase> createdRounds;
        protected readonly List<RoundBase> fetchedRounds;
        protected readonly List<GroupBase> createdGroups;
        protected readonly List<PlayerReference> createdPlayerReferences;
        protected readonly List<PlayerReference> fetchedPlayerReferences;

        public TournamentServiceStepDefinitions()
        {
            tournamentService = new TournamentService(InMemoryContextCreator.Create());
            createdTournaments = new List<Tournament>();
            fetchedTournaments = new List<Tournament>();
            createdBetters = new List<Better>();
            fetchedBetters = new List<Better>();
            createdRounds = new List<RoundBase>();
            fetchedRounds = new List<RoundBase>();
            createdGroups = new List<GroupBase>();
            createdPlayerReferences = new List<PlayerReference>();
            fetchedPlayerReferences = new List<PlayerReference>();
        }

        [Given(@"a tournament named ""(.*)"" has been created")]
        [When(@"a tournament named ""(.*)"" has been created")]
        public Tournament GivenATournamentNamedHasBeenCreated(string name)
        {
            createdTournaments.Add(tournamentService.CreateTournamentAsync(name));
            return createdTournaments.Last();
        }

        [Given(@"a tournament named ""(.*)"" with users ""(.*)"" added to it")]
        [When(@"a tournament named ""(.*)"" with users ""(.*)"" added to it")]
        public Tournament GivenATournamentNamedWithUsersAddedToIt(string tournamentName, string commaSeparatedUserNames)
        {
            List<string> userNames = StringUtility.ToStringList(commaSeparatedUserNames, ",");
            foreach (string userName in userNames)
            {
                createdUsers.Add(userService.CreateUserAsync(userName));
            }

            Tournament tournament = GivenATournamentNamedHasBeenCreated(tournamentName);

            foreach (User user in createdUsers)
            {
                createdBetters.Add(tournament.AddBetter(user));
            }

            return tournament;
        }

        [Given(@"a tournament named ""(.*)"" with player references ""(.*)"" added to it")]
        [When(@"a tournament named ""(.*)"" with player references ""(.*)"" added to it")]
        public Tournament GivenATournamentNamedWithPlayerReferencesAddedToIt(string tournamentName, string commaSeparatedPlayerNames)
        {
            Tournament tournament = GivenATournamentNamedHasBeenCreated(tournamentName);
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            RoundBase bracketRound = tournament.AddBracketRound("BracketRound", 3);
            BracketGroup bracketGroup = (BracketGroup)bracketRound.AddGroup();

            createdRounds.Add(bracketRound);
            createdGroups.Add(bracketGroup);

            foreach (string playerName in playerNames)
            {
                createdPlayerReferences.Add(bracketGroup.AddNewPlayerReference(playerName));
            }

            return tournament;
        }

        [Given(@"fetching created tournament by tournament id: (.*)")]
        [When(@"fetching created tournament by tournament id: (.*)")]
        public Tournament GivenFetchingCreatedTournamentByTournamentId(Guid tournamentId)
        {
            fetchedTournaments.Add(tournamentService.GetTournamentById(tournamentId));
            return fetchedTournaments.Last();
        }

        [Given(@"fetching created tournament (.*) by tournament id")]
        [When(@"fetching created tournament (.*) by tournament id")]
        public Tournament GivenFetchingCreatedTournamentByTournamentId(int tournamentIndex)
        {
            Guid tournamentId = createdTournaments[tournamentIndex].Id;
            fetchedTournaments.Add(GivenFetchingCreatedTournamentByTournamentId(tournamentId));
            return fetchedTournaments.Last();
        }

        [Given(@"fetching created tournament by tournament name: ""(.*)""")]
        [When(@"fetching created tournament by tournament name: ""(.*)""")]
        public Tournament GivenFetchingCreatedTournamentByTournamentName(string name)
        {
            fetchedTournaments.Add(tournamentService.GetTournamentByName(name));
            return fetchedTournaments.Last();
        }

        [Given(@"users ""(.*)"" is added to created tournament ""(.*)""")]
        [When(@"users ""(.*)"" is added to created tournament ""(.*)""")]
        public void GivenUsersIsAddedToCreatedTournament(string commaSeparatedUserNames, string tournamentName)
        {
            Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
            List<string> userNames = StringUtility.ToStringList(commaSeparatedUserNames, ",");

            foreach (string userName in userNames)
            {
                tournament.AddBetter(userService.GetUserByName(userName));
            }
        }

        [When(@"created tournament (.*) is renamed to: ""(.*)""")]
        public void WhenCreatedTournamentIsRenamedTo(int tournamentIndex, string newName)
        {
            Guid tournamentId = createdTournaments[tournamentIndex].Id;
            tournamentService.RenameTournament(tournamentId, newName);
        }

        [When(@"fetching betters from created tournament (.*) by tournament id")]
        public void WhenFetchingBettersFromCreatedTournamentByTournamentId(int tournamentIndex)
        {
            Guid tournamentId = createdTournaments[tournamentIndex].Id;
            fetchedBetters = tournamentService.GetBettersByTournamentId(tournamentId);
        }

        [When(@"fetching betters from tournament by tournament name: ""(.*)""")]
        public void FetchingBettersFromTournamentByTournamentName(string tournamentName)
        {
            fetchedBetters = tournamentService.GetBettersByTournamentName(tournamentName);
        }

        [When(@"fetching player references from created tournament (.*) by tournament id")]
        public void WhenFetchingPlayerReferencesFromCreatedTournamentByTournamentId(int tournamentIndex)
        {
            Tournament tournament = createdTournaments[tournamentIndex];
            List<PlayerReference> playerReferences = tournamentService.GetPlayerReferencesByTournamentId(tournament.Id);

            foreach (PlayerReference playerReference in playerReferences)
            {
                fetchedPlayerReferences.Add(playerReference);
            }
        }

        [When(@"fetching player references from tournament by tournament name: ""(.*)""")]
        public void WhenFetchingPlayerReferencesFromTournamentByTournamentName(string tournamentName)
        {
            List<PlayerReference> playerReferences = tournamentService.GetPlayerReferencesByTournamentName(tournamentName);

            foreach (PlayerReference playerReference in playerReferences)
            {
                fetchedPlayerReferences.Add(playerReference);
            }
        }

        [Then(@"created tournament (.*) should be valid with name: ""(.*)""")]
        public void ThenCreatedTournamentShouldBeValidWithName(int tournamentIndex, string correctName)
        {
            CheckTournamentValidity(createdTournaments[tournamentIndex], correctName);
        }

        [Then(@"fetched tournament (.*) should be valid with name: ""(.*)""")]
        public void ThenFetchedTournamentShouldBeValidWithName(int tournamentIndex, string correctName)
        {
            CheckTournamentValidity(fetchedTournaments[tournamentIndex], correctName);
        }

        [Then(@"created tournament (.*) should contain valid betters with names: ""(.*)""")]
        public void ThenCreatedTournamentShouldContainValidBettersWithNames(int tournamentIndex, string commaSeparatedBetterNames)
        {
            Tournament tournament = createdTournaments[tournamentIndex];
            List<string> betterNames = StringUtility.ToStringList(commaSeparatedBetterNames, ",");

            foreach (string betterName in betterNames)
            {
                Better better = tournament.Betters.FirstOrDefault(better => better.User.Name == betterName);

                better.Should().NotBeNull();
                CheckUserValidity(better.User, betterName);
            }
        }

        [Then(@"fetched tournament (.*) should contain valid betters with names: ""(.*)""")]
        public void ThenFetchedTournamentShouldContainValidBettersWithNames(int tournamentIndex, string commaSeparatedBetterNames)
        {
            Tournament tournament = fetchedTournaments[tournamentIndex];
            List<string> betterNames = StringUtility.ToStringList(commaSeparatedBetterNames, ",");

            foreach (string betterName in betterNames)
            {
                Better better = tournament.Betters.FirstOrDefault(better => better.User.Name == betterName);

                better.Should().NotBeNull();
                CheckUserValidity(better.User, betterName);
            }
        }

        [Then(@"created tournament (.*) should be invalid")]
        public void ThenCreatedTournamentShouldBeInvalid(int tournamentIndex)
        {
            createdTournaments[tournamentIndex].Should().BeNull();
        }

        [Then(@"fetched tournament (.*) should be invalid")]
        public void ThenFetchedTournamentShouldBeInvalid(int tournamentIndex)
        {
            fetchedTournaments[tournamentIndex].Should().BeNull();
        }

        [Then(@"better (.*) in created tournament (.*) should be invalid")]
        public void ThenBetterInCreatedTournamentShouldBeInvalid(int betterIndex, int tournamentIndex)
        {
            Tournament tournament = createdTournaments[tournamentIndex];
            tournament.Betters[betterIndex].Should().BeNull();
        }

        [Then(@"better amount in created tournament (.*) should be (.*)")]
        public void ThenBetterAmountInCreatedTournamentShouldBe(int tournamentIndex, int betterAmount)
        {
            Tournament tournament = createdTournaments[tournamentIndex];
            tournament.Betters.Should().HaveCount(betterAmount);
        }

        [Then(@"created tournament (.*) should contain exactly these player references with names: ""(.*)""")]
        public void ThenCreatedTournamentShouldContainExactlyThesePlayerReferencesWithNames(int tournamentIndex, string commaSeparetedPlayerNames)
        {
            Tournament tournament = createdTournaments[tournamentIndex];
            List<string> playerNames = StringUtility.ToStringList(commaSeparetedPlayerNames, ",");

            List<PlayerReference> playerReferences = tournament.GetPlayerReferencesInTournament();
            playerReferences.Should().NotBeNull();
            playerReferences.Should().HaveCount(playerNames.Count);
            foreach (string playerName in playerNames)
            {
                playerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }

        [Then(@"fetched player references should be exactly these player references with names: ""(.*)""")]
        public void ThenFetchedPlayerReferencesShouldBeExactlyThesePlayerReferencesWithNames(string commaSeparetedPlayerNames)
        {
            List<string> playerNames = StringUtility.ToStringList(commaSeparetedPlayerNames, ",");

            foreach (string playerName in playerNames)
            {
                fetchedPlayerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
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
            tournament.Betters.Should().BeEmpty();
            tournament.Settings.Should().BeEmpty();
            tournament.MiscBetCatalogue.Should().BeEmpty();
        }
    }
}
