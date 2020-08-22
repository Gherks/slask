using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
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

        public TournamentServiceStepDefinitions()
        {
            testDatabaseName = Guid.NewGuid().ToString();
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

        [Given(@"tournament named ""(.*)"" adds rounds")]
        [When(@"tournament named ""(.*)"" adds rounds")]
        public void GivenTournamentAddsRounds(string tournamentName, Table table)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                foreach (TableRow row in table.Rows)
                {
                    TestUtilities.ParseRoundTable(row, out string type, out string name, out int advancingCount, out int playersPerGroupCount);

                    if (type.Length > 0)
                    {
                        type = TestUtilities.ParseRoundGroupTypeString(type);
                        RoundBase round = null;

                        if (type == "BRACKET")
                        {
                            round = tournamentService.AddBracketRoundToTournament(tournament);
                        }
                        else if (type == "DUALTOURNAMENT")
                        {
                            round = tournamentService.AddDualTournamentRoundToTournament(tournament);
                        }
                        else if (type == "ROUNDROBIN")
                        {
                            round = tournamentService.AddRoundRobinRoundToTournament(tournament);
                        }
                        tournamentService.Save();

                        if (round == null)
                        {
                            return;
                        }

                        tournamentService.RenameRoundInTournament(round, name);
                        tournamentService.SetAdvancingPerGroupCountInRound(round, advancingCount);
                        tournamentService.SetPlayersPerGroupCountInRound(round, playersPerGroupCount);
                        tournamentService.Save();
                    }
                }
            }
        }

        [Given(@"players ""(.*)"" is registered to tournament named ""(.*)""")]
        [When(@"players ""(.*)"" is registered to tournament named ""(.*)""")]
        public void GivenPlayersIsRegisteredToRound(string commaSeparatedPlayerNames, string tournamentName)
        {
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                foreach (string playerName in playerNames)
                {
                    tournamentService.AddPlayerReference(tournament, playerName);
                }

                tournamentService.Save();
            }
        }

        [Given(@"betters places match bets in tournament named ""(.*)""")]
        [When(@"betters places match bets in tournament named ""(.*)""")]
        public void GivenBettersPlacesMatchBets(string tournamentName, Table table)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                tournamentService.Save();
                foreach (TableRow row in table.Rows)
                {
                    TestUtilities.ParseBetterMatchBetPlacements(row, out string betterName, out int roundIndex, out int groupIndex, out int matchIndex, out string playerName);

                    Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                    RoundBase round = tournament.Rounds[roundIndex];
                    GroupBase group = round.Groups[groupIndex];
                    Match match = group.Matches[matchIndex];

                    tournamentService.BetterPlacesMatchBetOnMatch(tournament.Id, match.Id, betterName, playerName);
                    tournamentService.Save();
                }
            }
        }

        [Given(@"groups within tournament named ""(.*)"" is played out")]
        [When(@"groups within tournament named ""(.*)"" is played out")]
        public void GivenGroupsWithinTournamentIsPlayedOut(string tournamentName, Table table)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                foreach (TableRow row in table.Rows)
                {
                    TestUtilities.ParseTargetGroupToPlay(row, out int _, out int roundIndex, out int groupIndex);

                    SystemTimeMocker.Reset();
                    Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                    RoundBase round = tournament.Rounds[roundIndex];
                    GroupBase group = round.Groups[groupIndex];

                    foreach (Match match in group.Matches)
                    {
                        PlayMatch(tournamentService, match);
                    }

                    tournamentService.Save();
                }
            }
        }

        [Given(@"better standings in tournament named ""(.*)"" from first to last looks like this")]
        [When(@"better standings in tournament named ""(.*)"" from first to last looks like this")]
        public void GivenBetterStandingsInTournamentFromFirstToLastLooksLikeThis(string tournamentName, Table table)
        {
            Tournament tournament;
            using (TournamentService tournamentService = CreateTournamentService())
            {
                tournament = tournamentService.GetTournamentByName(tournamentName);
            }

            List<StandingsEntry<Better>> betterStandings = tournament.GetBetterStandings();

            betterStandings.Should().HaveCount(table.Rows.Count);

            for (int index = 0; index < table.Rows.Count; ++index)
            {
                TestUtilities.ParseBetterStandings(table.Rows[index], out string betterName, out int points);

                betterStandings[index].Object.User.Name.Should().Be(betterName);
                betterStandings[index].Points.Should().Be(points);
            }
        }

        [Given(@"score is added to players in given matches within groups in tournament named ""(.*)""")]
        [When(@"score is added to players in given matches within groups in tournament named ""(.*)""")]
        public void GivenScoreIsAddedToPlayersInGivenMatchesInGroups(string tournamentName, Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                foreach (TableRow row in table.Rows)
                {
                    TestUtilities.ParseScoreAddedToMatchPlayer(row, out int roundIndex, out int groupIndex, out int matchIndex, out string scoringPlayer, out int scoreAdded);

                    RoundBase round = tournament.Rounds[roundIndex];
                    GroupBase group = round.Groups[groupIndex];
                    Match match = group.Matches[matchIndex];

                    SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

                    Player player = match.FindPlayer(scoringPlayer);

                    if (player != null)
                    {
                        tournamentService.AddScoreToPlayerInMatch(tournament, match.Id, player.Id, scoreAdded);
                    }
                    else
                    {
                        // LOG Error: Invalid player name in given match within given group
                        throw new NotImplementedException();
                    }
                }

                tournamentService.Save();
            }
        }

        private void PlayMatch(TournamentService tournamentService, Match match)
        {
            bool matchHaveNotStarted = match.StartDateTime > SystemTime.Now;

            if (matchHaveNotStarted)
            {
                SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            }

            int winningScore = (int)Math.Ceiling(match.BestOf / 2.0);

            // Give points to player with name that precedes the other alphabetically
            bool increasePlayer1Score = match.Player1.GetName().CompareTo(match.Player2.GetName()) <= 0;

            Tournament tournament = match.Group.Round.Tournament;
            Guid scoringPlayerId = increasePlayer1Score ? match.Player1.Id : match.Player2.Id;

            tournamentService.AddScoreToPlayerInMatch(tournament, match.Id, scoringPlayerId, winningScore);
            tournamentService.Save();
        }

        protected UserService CreateUserService()
        {
            return new UserService(InMemoryContextCreator.Create(testDatabaseName));
        }

        protected TournamentService CreateTournamentService()
        {
            return new TournamentService(InMemoryContextCreator.Create(testDatabaseName));
        }
    }
}
