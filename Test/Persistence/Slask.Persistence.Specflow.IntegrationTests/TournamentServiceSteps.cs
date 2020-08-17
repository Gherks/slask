using FluentAssertions;
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
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.PersistenceTests
{
    [Binding, Scope(Feature = "TournamentService")]
    public class TournamentServiceSteps : TournamentServiceStepDefinitions
    {

    }

    public class TournamentServiceStepDefinitions
    {
        private readonly string testDatabaseName;

        protected readonly TournamentService _tournamentService;

        public TournamentServiceStepDefinitions()
        {
            testDatabaseName = Guid.NewGuid().ToString();

            _tournamentService = CreateTournamentService();
        }

        [Given(@"a tournament named ""(.*)"" has been created with users ""(.*)"" added to it")]
        [When(@"a tournament named ""(.*)"" has been created with users ""(.*)"" added to it")]
        public void GivenATournamentNamedWithUsersAddedToIt(string tournamentName, string commaSeparatedUserNames)
        {
            List<string> userNames = StringUtility.ToStringList(commaSeparatedUserNames, ",");

            using (UserService userService = CreateUserService())
            {
                foreach (string userName in userNames)
                {
                    userService.CreateUser(userName);
                }
                userService.Save();

                using (TournamentService tournamentService = CreateTournamentService())
                {
                    Tournament tournament = tournamentService.CreateTournament(tournamentName);

                    foreach (string userName in userNames)
                    {
                        User user = userService.GetUserByName(userName);
                        tournamentService.AddBetterToTournament(tournament, user);
                    }

                    tournamentService.Save();
                }
            }
        }

        [Given(@"players ""(.*)"" is registered to tournament (.*)")]
        [When(@"players ""(.*)"" is registered to tournament (.*)")]
        public void GivenPlayersIsRegisteredToRound(string commaSeparatedPlayerNames, int tournamentIndex)
        {
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");
            Tournament tournament = _tournamentService.GetTournamentByName("Homestory Cup XX");

            foreach (string playerName in playerNames)
            {
                _tournamentService.AddPlayerReference(tournament, playerName);
            }

            _tournamentService.Save();
        }

        [Given(@"tournament (.*) adds rounds")]
        [When(@"tournament (.*) adds rounds")]
        public void GivenTournamentAddsRounds(int tournamentIndex, Table table)
        {
            Tournament tournament = _tournamentService.GetTournamentByName("Homestory Cup XX");

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
                }
            }
        }

        [Given(@"betters places match bets")]
        [When(@"betters places match bets")]
        public void GivenBettersPlacesMatchBets(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                TestUtilities.ParseBetterMatchBetPlacements(row, out string betterName, out int roundIndex, out int groupIndex, out int matchIndex, out string playerName);

                Tournament tournament = _tournamentService.GetTournamentByName("Homestory Cup XX");
                RoundBase round = tournament.Rounds[roundIndex];
                GroupBase group = round.Groups[groupIndex];
                Match match = group.Matches[matchIndex];

                _tournamentService.BetterPlacesMatchBetOnMatch(tournament.Id, match.Id, betterName, playerName);
                _tournamentService.Save();
            }
        }

        [Given(@"groups within tournament is played out")]
        [When(@"groups within tournament is played out")]
        public void GivenGroupsWithinTournamentIsPlayedOut(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                TestUtilities.ParseTargetGroupToPlay(row, out int _, out int roundIndex, out int groupIndex);

                SystemTimeMocker.Reset();
                Tournament tournament = _tournamentService.GetTournamentByName("Homestory Cup XX");
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
                    _tournamentService.Save();

                    if (!playedMatchesSuccessfully)
                    {
                        break;
                    }
                }
            }
        }

        [Given(@"better standings in tournament (.*) from first to last looks like this")]
        [When(@"better standings in tournament (.*) from first to last looks like this")]
        public void GivenBetterStandingsInTournamentFromFirstToLastLooksLikeThis(int tournamentIndex, Table table)
        {
            Tournament tournament = _tournamentService.GetTournamentByName("Homestory Cup XX");

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
                TestUtilities.ParseScoreAddedToMatchPlayer(row, out int roundIndex, out int groupIndex, out int matchIndex, out string scoringPlayer, out int scoreAdded);

                Tournament tournament = _tournamentService.GetTournamentByName("Homestory Cup XX");
                RoundBase round = tournament.Rounds[roundIndex];
                GroupBase group = round.Groups[groupIndex];
                Match match = group.Matches[matchIndex];

                SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

                Player player = match.FindPlayer(scoringPlayer);

                if (player != null)
                {
                    _tournamentService.AddScoreToPlayerInMatch(tournament.Id, match.Id, player.Id, scoreAdded);
                }
                else
                {
                    // LOG Error: Invalid player name in given match within given group
                }
            }

            _tournamentService.Save();
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
                        scoreIncreased = _tournamentService.AddScoreToPlayerInMatch(group.Round.Tournament.Id, match.Id, match.Player1.Id, winningScore);
                    }
                    else
                    {
                        scoreIncreased = _tournamentService.AddScoreToPlayerInMatch(group.Round.Tournament.Id, match.Id, match.Player2.Id, winningScore);
                    }

                    if (!scoreIncreased)
                    {
                        return false;
                    }
                }
            }

            _tournamentService.Save();

            return true;
        }

        private UserService CreateUserService()
        {
            return new UserService(InMemoryContextCreator.Create(testDatabaseName));
        }

        private TournamentService CreateTournamentService()
        {
            return new TournamentService(InMemoryContextCreator.Create(testDatabaseName));
        }
    }
}
