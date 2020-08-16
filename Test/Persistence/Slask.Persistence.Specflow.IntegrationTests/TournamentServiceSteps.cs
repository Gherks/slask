﻿using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using Slask.Domain.Utilities.StandingsSolvers;
using Slask.Persistence.Services;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.PersistenceTests
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
        protected readonly List<Better> _createdBetters;
        protected readonly List<Better> _fetchedBetters;
        protected readonly List<RoundBase> _createdRounds;
        protected readonly List<RoundBase> _fetchedRounds;
        protected readonly List<GroupBase> _createdGroups;
        protected readonly List<GroupBase> _fetchedGroups;
        protected readonly List<PlayerReference> _createdPlayerReferences;
        protected readonly List<PlayerReference> _fetchedPlayerReferences;

        public TournamentServiceStepDefinitions()
        {
            _tournamentService = new TournamentService(InMemoryContextCreator.Create());
            _createdTournaments = new List<Tournament>();
            _fetchedTournaments = new List<Tournament>();
            _createdBetters = new List<Better>();
            _fetchedBetters = new List<Better>();
            _createdRounds = new List<RoundBase>();
            _fetchedRounds = new List<RoundBase>();
            _createdGroups = new List<GroupBase>();
            _fetchedGroups = new List<GroupBase>();
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
                _createdUsers.Add(_userService.CreateUser(userName));
            }
            _userService.Save();

            Tournament tournament = GivenATournamentNamedHasBeenCreated(tournamentName);

            foreach (User user in _createdUsers)
            {
                _tournamentService.AddBetterToTournament(tournament, user);
            }
            _tournamentService.Save();

            return tournament;
        }

        [Given(@"players ""(.*)"" is registered to tournament (.*)")]
        [When(@"players ""(.*)"" is registered to tournament (.*)")]
        [Then(@"players ""(.*)"" is registered to tournament (.*)")]
        public void GivenPlayersIsRegisteredToRound(string commaSeparatedPlayerNames, int tournamentIndex)
        {
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");
            Tournament tournament = _createdTournaments[tournamentIndex];

            foreach (string playerName in playerNames)
            {
                _tournamentService.RegisterPlayerReference(tournament, playerName);
            }

            _tournamentService.Save();

            RoundBase round = tournament.GetFirstRound();
            _createdGroups.Clear();
            while (round != null)
            {
                _createdGroups.AddRange(round.Groups);
                round = round.GetNextRound();
            }
        }

        [Given(@"tournament (.*) adds rounds")]
        [When(@"tournament (.*) adds rounds")]
        public void GivenTournamentAddsRounds(int tournamentIndex, Table table)
        {
            if (_createdTournaments.Count <= tournamentIndex)
            {
                throw new IndexOutOfRangeException("Given tournament index is out of bounds");
            }

            Tournament tournament = _createdTournaments[tournamentIndex];

            foreach (TableRow row in table.Rows)
            {
                TestUtilities.ParseRoundTable(row, out string type, out string name, out int advancingCount, out int playersPerGroupCount);

                if (type.Length > 0)
                {
                    type = TestUtilities.ParseRoundGroupTypeString(type);
                    RoundBase round = null;

                    if (type == "BRACKET")
                    {
                        round = _tournamentService.AddBracketRoundToTournament(tournament);
                    }
                    else if (type == "DUALTOURNAMENT")
                    {
                        round = _tournamentService.AddDualTournamentRoundToTournament(tournament);
                    }
                    else if (type == "ROUNDROBIN")
                    {
                        round = _tournamentService.AddRoundRobinRoundToTournament(tournament);
                    }
                    _tournamentService.Save();

                    if (round == null)
                    {
                        return;
                    }

                    _tournamentService.RenameRoundInTournament(round, name);
                    _tournamentService.Save();
                    _tournamentService.SetAdvancingPerGroupCountInRound(round, advancingCount);
                    _tournamentService.Save();
                    _tournamentService.SetPlayersPerGroupCountInRound(round, playersPerGroupCount);
                    _tournamentService.Save();

                    _createdRounds.Add(round);
                    bool addedValidRound = round != null;

                    if (addedValidRound)
                    {
                        _createdGroups.AddRange(round.Groups);
                    }

                    round = tournament.GetFirstRound();
                    _createdGroups.Clear();
                    while (round != null)
                    {
                        _createdGroups.AddRange(round.Groups);
                        round = round.GetNextRound();
                    }
                }
            }
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
                tournament.AddBetter(_userService.GetUserByName(userName));
                _tournamentService.Save();
            }
        }

        [Given(@"betters places match bets")]
        [When(@"betters places match bets")]
        public void GivenBettersPlacesMatchBets(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                TestUtilities.ParseBetterMatchBetPlacements(row, out string betterName, out int _, out int groupIndex, out int matchIndex, out string playerName);

                GroupBase group = _createdGroups[groupIndex];
                Match match = group.Matches[matchIndex];

                _tournamentService.BetterPlacesMatchBetOnMatch(group.Round.Tournament.Id, match.Id, betterName, playerName);
                _tournamentService.Save();
            }
        }

        [Given(@"groups within tournament is played out")]
        [When(@"groups within tournament is played out")]
        public void GivenGroupsWithinTournamentIsPlayedOut(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                TestUtilities.ParseTargetGroupToPlay(row, out int tournamentIndex, out int roundIndex, out int groupIndex);

                bool tournamentIndexIsValid = _createdTournaments.Count > tournamentIndex;
                bool roundIndexIsValid = _createdTournaments[tournamentIndex].Rounds.Count > roundIndex;
                bool groupIndexIsValid = _createdTournaments[tournamentIndex].Rounds[roundIndex].Groups.Count > groupIndex;

                if (!tournamentIndexIsValid || !roundIndexIsValid || !groupIndexIsValid)
                {
                    throw new IndexOutOfRangeException("Tournament, round, or group with given index does not exist");
                }

                SystemTimeMocker.Reset();
                Tournament tournament = _createdTournaments[tournamentIndex];
                RoundBase round = tournament.Rounds[roundIndex];
                GroupBase group = round.Groups[groupIndex];

                while (group.GetPlayState() != PlayStateEnum.Finished)
                {
                    foreach (Match match in group.Matches)
                    {
                        if (match.IsReady() && match.GetPlayState() == PlayStateEnum.NotBegun)
                        {
                            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
                            break;
                        }
                    }

                    bool playedMatchesSuccessfully = PlayAvailableMatches(group);

                    if (!playedMatchesSuccessfully)
                    {
                        break;
                    }
                }
            }
        }

        [Given(@"better standings in tournament (.*) from first to last looks like this")]
        [When(@"better standings in tournament (.*) from first to last looks like this")]
        [Then(@"better standings in tournament (.*) from first to last looks like this")]
        public void GivenBetterStandingsInTournamentFromFirstToLastLooksLikeThis(int tournamentIndex, Table table)
        {
            Tournament tournament = _createdTournaments[tournamentIndex];

            List<StandingsEntry<Better>> betterStandings = tournament.GetBetterStandings();

            betterStandings.Should().HaveCount(table.Rows.Count);

            for (int index = 0; index < table.Rows.Count; ++index)
            {
                TestUtilities.ParseBetterStandings(table.Rows[index], out string betterName, out int points);

                betterStandings[index].Object.User.Name.Should().Be(betterName);
                betterStandings[index].Points.Should().Be(points);
            }
        }

        [Given(@"score is added to players in given matches in groups")]
        [When(@"score is added to players in given matches in groups")]
        public void GivenScoreIsAddedToPlayersInGivenMatchesInGroups(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            foreach (TableRow row in table.Rows)
            {
                TestUtilities.ParseSoreAddedToMatchPlayer(row, out int groupIndex, out int matchIndex, out string scoringPlayer, out int scoreAdded);

                GroupBase group = _createdGroups[groupIndex];
                Match match = group.Matches[matchIndex];

                SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

                Player player = match.FindPlayer(scoringPlayer);

                if (player != null)
                {
                    player.IncreaseScore(scoreAdded);
                }
                else
                {
                    throw new Exception("Invalid player name in given match within given group");
                }
            }
        }

        [When(@"tournament (.*) is renamed to: ""(.*)""")]
        public void WhenTournamentIsRenamedTo(int tournamentIndex, string newName)
        {
            Guid tournamentId = _createdTournaments[tournamentIndex].Id;
            _tournamentService.RenameTournament(tournamentId, newName);
            _tournamentService.Save();
        }

        [When(@"fetching betters from tournament (.*) by tournament id")]
        public void WhenFetchingBettersFromTournamentByTournamentId(int tournamentIndex)
        {
            Guid tournamentId = _createdTournaments[tournamentIndex].Id;

            _fetchedBetters.Clear();
            _fetchedBetters.AddRange(_tournamentService.GetBettersByTournamentId(tournamentId));
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

        private static void CheckTournamentValidity(Tournament tournament, string correctName)
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

        private bool PlayAvailableMatches(GroupBase group)
        {
            foreach (Match match in group.Matches)
            {
                int winningScore = (int)Math.Ceiling(match.BestOf / 2.0);

                bool matchShouldHaveStarted = match.StartDateTime < SystemTime.Now;
                bool matchIsNotFinished = match.GetPlayState() != PlayStateEnum.Finished;

                if (matchShouldHaveStarted && matchIsNotFinished)
                {
                    // Give points to player with name that precedes the other alphabetically
                    bool increasePlayer1Score = match.Player1.Name.CompareTo(match.Player2.Name) <= 0;
                    bool scoreIncreased;

                    if (increasePlayer1Score)
                    {
                        scoreIncreased = match.Player1.IncreaseScore(winningScore);
                    }
                    else
                    {
                        scoreIncreased = match.Player2.IncreaseScore(winningScore);
                    }

                    if (!scoreIncreased)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
