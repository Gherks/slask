using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.Persistence.Services;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.PersistenceTests.ServiceTests
{
    [Binding, Scope(Feature = "TournamentService")]
    public class TournamentServiceSteps : TournamentServiceStepDefinitions
    {

    }

    public class TournamentServiceStepDefinitions : UserServiceStepDefinitions
    {
        protected readonly TournamentService _tournamentService;
        protected readonly List<Tournament> _createdTournaments;
        protected readonly List<Tournament> _fetchedTournaments;
        protected List<Better> _createdBetters;
        protected List<Better> _fetchedBetters;
        protected readonly List<PlayerReference> _createdPlayerReferences;
        protected readonly List<PlayerReference> _fetchedPlayerReferences;

        public TournamentServiceStepDefinitions()
        {
            _tournamentService = new TournamentService(InMemoryContextCreator.Create());
            _createdTournaments = new List<Tournament>();
            _fetchedTournaments = new List<Tournament>();
            _createdBetters = new List<Better>();
            _fetchedBetters = new List<Better>();
            _createdPlayerReferences = new List<PlayerReference>();
            _fetchedPlayerReferences = new List<PlayerReference>();
        }

        [Given(@"a tournament named ""(.*)"" has been created")]
        [When(@"a tournament named ""(.*)"" has been created")]
        public Tournament GivenATournamentNamedHasBeenCreated(string name)
        {
            Tournament tournament = _tournamentService.CreateTournament(name);

            if (tournament != null)
            {
                _createdTournaments.Add(tournament);
                _tournamentService.Save();

                return tournament;
            }

            return null;
        }

        [Given(@"a tournament named ""(.*)"" has been created with users ""(.*)"" added to it")]
        [When(@"a tournament named ""(.*)"" has been created with users ""(.*)"" added to it")]
        public Tournament GivenATournamentNamedWithUsersAddedToIt(string tournamentName, string commaSeparatedUserNames)
        {
            List<string> userNames = StringUtility.ToStringList(commaSeparatedUserNames, ",");
            foreach (string userName in userNames)
            {
                createdUsers.Add(userService.CreateUser(userName));
                userService.Save();
            }

            Tournament tournament = GivenATournamentNamedHasBeenCreated(tournamentName);

            foreach (User user in createdUsers)
            {
                _createdBetters.Add(tournament.AddBetter(user));
            }

            return tournament;
        }

        [Given(@"a tournament named ""(.*)"" with player references ""(.*)"" added to it")]
        [When(@"a tournament named ""(.*)"" with player references ""(.*)"" added to it")]
        public Tournament GivenATournamentNamedWithPlayerReferencesAddedToIt(string tournamentName, string commaSeparatedPlayerNames)
        {
            Tournament tournament = GivenATournamentNamedHasBeenCreated(tournamentName);
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            RoundBase bracketRound = tournament.AddBracketRound();

            foreach (string playerName in playerNames)
            {
                _createdPlayerReferences.Add(bracketRound.RegisterPlayerReference(playerName));
            }

            return tournament;
        }

        [Given(@"fetching tournament by tournament id: (.*)")]
        [When(@"fetching tournament by tournament id: (.*)")]
        public Tournament GivenFetchingTournamentByTournamentId(Guid tournamentId)
        {
            _fetchedTournaments.Add(_tournamentService.GetTournamentById(tournamentId));
            return _fetchedTournaments.Last();
        }

        [Given(@"fetching tournament (.*) by tournament id")]
        [When(@"fetching tournament (.*) by tournament id")]
        public Tournament GivenFetchingTournamentByTournamentId(int tournamentIndex)
        {
            Guid tournamentId = _createdTournaments[tournamentIndex].Id;
            _fetchedTournaments.Add(GivenFetchingTournamentByTournamentId(tournamentId));
            return _fetchedTournaments.Last();
        }

        [Given(@"fetching tournament by tournament name: ""(.*)""")]
        [When(@"fetching tournament by tournament name: ""(.*)""")]
        public Tournament GivenFetchingTournamentByTournamentName(string name)
        {
            _fetchedTournaments.Add(_tournamentService.GetTournamentByName(name));
            return _fetchedTournaments.Last();
        }

        [Given(@"users ""(.*)"" is added to tournament ""(.*)""")]
        [When(@"users ""(.*)"" is added to tournament ""(.*)""")]
        public void GivenUsersIsAddedToTournament(string commaSeparatedUserNames, string tournamentName)
        {
            Tournament tournament = _tournamentService.GetTournamentByName(tournamentName);
            List<string> userNames = StringUtility.ToStringList(commaSeparatedUserNames, ",");

            foreach (string userName in userNames)
            {
                tournament.AddBetter(userService.GetUserByName(userName));
            }
        }

        [When(@"tournament (.*) is renamed to: ""(.*)""")]
        public void WhenTournamentIsRenamedTo(int tournamentIndex, string newName)
        {
            Guid tournamentId = _createdTournaments[tournamentIndex].Id;
            _tournamentService.RenameTournament(tournamentId, newName);
        }

        [When(@"fetching betters from tournament (.*) by tournament id")]
        public void WhenFetchingBettersFromTournamentByTournamentId(int tournamentIndex)
        {
            Guid tournamentId = _createdTournaments[tournamentIndex].Id;
            _fetchedBetters = _tournamentService.GetBettersByTournamentId(tournamentId);
        }

        [When(@"fetching betters from tournament by tournament name: ""(.*)""")]
        public void FetchingBettersFromTournamentByTournamentName(string tournamentName)
        {
            _fetchedBetters = _tournamentService.GetBettersByTournamentName(tournamentName);
        }

        [When(@"fetching player references from tournament (.*) by tournament id")]
        public void WhenFetchingPlayerReferencesFromTournamentByTournamentId(int tournamentIndex)
        {
            Tournament tournament = _createdTournaments[tournamentIndex];
            List<PlayerReference> playerReferences = _tournamentService.GetPlayerReferencesByTournamentId(tournament.Id);

            foreach (PlayerReference playerReference in playerReferences)
            {
                _fetchedPlayerReferences.Add(playerReference);
            }
        }

        [When(@"fetching player references from tournament by tournament name: ""(.*)""")]
        public void WhenFetchingPlayerReferencesFromTournamentByTournamentName(string tournamentName)
        {
            List<PlayerReference> playerReferences = _tournamentService.GetPlayerReferencesByTournamentName(tournamentName);

            foreach (PlayerReference playerReference in playerReferences)
            {
                _fetchedPlayerReferences.Add(playerReference);
            }
        }

        [Then(@"tournament (.*) should be valid with name: ""(.*)""")]
        public void ThenTournamentShouldBeValidWithName(int tournamentIndex, string correctName)
        {
            CheckTournamentValidity(_createdTournaments[tournamentIndex], correctName);
        }

        [Then(@"fetched tournament (.*) should be valid with name: ""(.*)""")]
        public void ThenFetchedTournamentShouldBeValidWithName(int tournamentIndex, string correctName)
        {
            CheckTournamentValidity(_fetchedTournaments[tournamentIndex], correctName);
        }

        [Then(@"tournament (.*) should contain valid betters with names: ""(.*)""")]
        public void ThenTournamentShouldContainValidBettersWithNames(int tournamentIndex, string commaSeparatedBetterNames)
        {
            Tournament tournament = _createdTournaments[tournamentIndex];
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
            Tournament tournament = _fetchedTournaments[tournamentIndex];
            List<string> betterNames = StringUtility.ToStringList(commaSeparatedBetterNames, ",");

            foreach (string betterName in betterNames)
            {
                Better better = tournament.Betters.FirstOrDefault(better => better.User.Name == betterName);

                better.Should().NotBeNull();
                CheckUserValidity(better.User, betterName);
            }
        }

        [Then(@"tournament (.*) should be invalid")]
        public void ThenTournamentShouldBeInvalid(int tournamentIndex)
        {
            _createdTournaments[tournamentIndex].Should().BeNull();
        }

        [Then(@"fetched tournament (.*) should be invalid")]
        public void ThenFetchedTournamentShouldBeInvalid(int tournamentIndex)
        {
            _fetchedTournaments[tournamentIndex].Should().BeNull();
        }

        [Then(@"better (.*) in tournament (.*) should be invalid")]
        public void ThenBetterInTournamentShouldBeInvalid(int betterIndex, int tournamentIndex)
        {
            Tournament tournament = _createdTournaments[tournamentIndex];
            tournament.Betters[betterIndex].Should().BeNull();
        }

        [Then(@"better count in tournament (.*) should be (.*)")]
        public void ThenBetterCountIndTournamentShouldBe(int tournamentIndex, int betterCount)
        {
            Tournament tournament = _createdTournaments[tournamentIndex];
            tournament.Betters.Should().HaveCount(betterCount);
        }

        [Then(@"tournament (.*) should contain exactly these player references with names: ""(.*)""")]
        public void ThenTournamentShouldContainExactlyThesePlayerReferencesWithNames(int tournamentIndex, string commaSeparetedPlayerNames)
        {
            Tournament tournament = _createdTournaments[tournamentIndex];
            List<string> playerNames = StringUtility.ToStringList(commaSeparetedPlayerNames, ",");

            List<PlayerReference> playerReferences = tournament.GetPlayerReferences();

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

            _fetchedPlayerReferences.Should().HaveCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                _fetchedPlayerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
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
        }
    }
}
