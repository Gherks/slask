using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Domain.Utilities;
using Slask.Domain.Utilities.StandingsSolvers;
using Slask.Persistence.Repositories;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.PersistenceTests
{
    public class SpecflowCoreSteps
    {
        private readonly string testDatabaseName;

        public SpecflowCoreSteps()
        {
            testDatabaseName = Guid.NewGuid().ToString();
        }

        [Given(@"users ""(.*)"" has been created")]
        [When(@"users ""(.*)"" has been created")]
        public void GivenUsersHasBeenCreated(string commaSeparatedUserNames)
        {
            List<string> userNames = StringUtility.ToStringList(commaSeparatedUserNames, ",");

            using (UserRepository userRepository = CreateUserRepository())
            {
                foreach (string userName in userNames)
                {
                    userRepository.CreateUser(userName);
                }
                userRepository.Save();
            }
        }

        [Given(@"a tournament named ""(.*)"" has been created")]
        [When(@"a tournament named ""(.*)"" has been created")]
        public void GivenATournamentNamedWithUsersAddedToIt(string tournamentName)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.CreateTournament(tournamentName);
                tournamentRepository.Save();
            }
        }

        [Given(@"a tournament named ""(.*)"" has been created with users ""(.*)"" added to it")]
        [When(@"a tournament named ""(.*)"" has been created with users ""(.*)"" added to it")]
        public void GivenATournamentNamedWithUsersAddedToIt(string tournamentName, string commaSeparatedUserNames)
        {
            List<string> userNames = StringUtility.ToStringList(commaSeparatedUserNames, ",");

            using (UserRepository userRepository = CreateUserRepository())
            {
                foreach (string userName in userNames)
                {
                    userRepository.CreateUser(userName);
                }
                userRepository.Save();

                using (TournamentRepository tournamentRepository = CreateTournamentRepository())
                {
                    Tournament tournament = tournamentRepository.CreateTournament(tournamentName);

                    foreach (string userName in userNames)
                    {
                        User user = userRepository.GetUserByName(userName);
                        tournamentRepository.AddBetterToTournament(tournament, user);
                    }

                    tournamentRepository.Save();
                }
            }
        }

        [Given(@"tournament named ""(.*)"" adds rounds")]
        [When(@"tournament named ""(.*)"" adds rounds")]
        public void GivenTournamentAddsRounds(string tournamentName, Table table)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);

                foreach (TableRow row in table.Rows)
                {
                    TestUtilities.ParseRoundTable(row, out ContestTypeEnum contestType, out string name, out int advancingCount, out int playersPerGroupCount);

                    if (contestType != ContestTypeEnum.None)
                    {
                        RoundBase round = null;

                        if (contestType == ContestTypeEnum.Bracket)
                        {
                            round = tournamentRepository.AddBracketRoundToTournament(tournament);
                        }
                        else if (contestType == ContestTypeEnum.DualTournament)
                        {
                            round = tournamentRepository.AddDualTournamentRoundToTournament(tournament);
                        }
                        else if (contestType == ContestTypeEnum.RoundRobin)
                        {
                            round = tournamentRepository.AddRoundRobinRoundToTournament(tournament);
                        }
                        tournamentRepository.Save();

                        if (round == null)
                        {
                            return;
                        }

                        tournamentRepository.RenameRoundInTournament(round, name);
                        tournamentRepository.SetAdvancingPerGroupCountInRound(round, advancingCount);
                        tournamentRepository.SetPlayersPerGroupCountInRound(round, playersPerGroupCount);
                        tournamentRepository.Save();
                    }
                }
            }
        }

        [Given(@"players ""(.*)"" is registered to tournament named ""(.*)""")]
        [When(@"players ""(.*)"" is registered to tournament named ""(.*)""")]
        public void GivenPlayersIsRegisteredToRound(string commaSeparatedPlayerNames, string tournamentName)
        {
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);

                foreach (string playerName in playerNames)
                {
                    tournamentRepository.AddPlayerReference(tournament, playerName);
                }

                tournamentRepository.Save();
            }
        }

        [Given(@"betters places match bets in tournament named ""(.*)""")]
        [When(@"betters places match bets in tournament named ""(.*)""")]
        public void GivenBettersPlacesMatchBets(string tournamentName, Table table)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                tournamentRepository.Save();
                foreach (TableRow row in table.Rows)
                {
                    TestUtilities.ParseBetterMatchBetPlacements(row, out string betterName, out int roundIndex, out int groupIndex, out int matchIndex, out string playerName);

                    Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                    RoundBase round = tournament.Rounds[roundIndex];
                    GroupBase group = round.Groups[groupIndex];
                    Match match = group.Matches[matchIndex];

                    tournamentRepository.BetterPlacesMatchBetOnMatch(tournament.Id, match.Id, betterName, playerName);
                    tournamentRepository.Save();
                }
            }
        }

        [Given(@"groups within tournament named ""(.*)"" is played out")]
        [When(@"groups within tournament named ""(.*)"" is played out")]
        public void GivenGroupsWithinTournamentIsPlayedOut(string tournamentName, Table table)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                foreach (TableRow row in table.Rows)
                {
                    TestUtilities.ParseTargetGroupToPlay(row, out int _, out int roundIndex, out int groupIndex);

                    SystemTimeMocker.Reset();
                    Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                    RoundBase round = tournament.Rounds[roundIndex];
                    GroupBase group = round.Groups[groupIndex];

                    foreach (Match match in group.Matches)
                    {
                        PlayMatch(tournamentRepository, match);
                    }

                    tournamentRepository.Save();
                }
            }
        }

        [Given(@"better standings in tournament named ""(.*)"" from first to last looks like this")]
        [When(@"better standings in tournament named ""(.*)"" from first to last looks like this")]
        public void GivenBetterStandingsInTournamentFromFirstToLastLooksLikeThis(string tournamentName, Table table)
        {
            Tournament tournament;
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                tournament = tournamentRepository.GetTournamentByName(tournamentName);
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

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);

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
                        tournamentRepository.AddScoreToPlayerInMatch(tournament, match.Id, player.Id, scoreAdded);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                tournamentRepository.Save();
            }
        }

        [Then(@"tournament named ""(.*)"" should contain rounds")]
        public void ThenFetchedTournamentShouldContainRounds(string tournamentName, Table table)
        {
            Tournament tournament;
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                tournament = tournamentRepository.GetTournamentByName(tournamentName);
            }

            for (int index = 0; index < table.Rows.Count; ++index)
            {
                TestUtilities.ParseRoundTable(table.Rows[index], out ContestTypeEnum contestType, out _, out _, out _);

                if (contestType != ContestTypeEnum.None)
                {
                    RoundBase round = tournament.Rounds[index];

                    if (contestType == ContestTypeEnum.Bracket)
                    {
                        round.ContestType.Should().Be(ContestTypeEnum.Bracket);

                        (round is BracketRound).Should().BeTrue();
                        (round is DualTournamentRound).Should().BeFalse();
                        (round is RoundRobinRound).Should().BeFalse();
                    }
                    else if (contestType == ContestTypeEnum.DualTournament)
                    {
                        round.ContestType.Should().Be(ContestTypeEnum.DualTournament);

                        (round is BracketRound).Should().BeFalse();
                        (round is DualTournamentRound).Should().BeTrue();
                        (round is RoundRobinRound).Should().BeFalse();
                    }
                    else if (contestType == ContestTypeEnum.RoundRobin)
                    {
                        round.ContestType.Should().Be(ContestTypeEnum.RoundRobin);

                        (round is BracketRound).Should().BeFalse();
                        (round is DualTournamentRound).Should().BeFalse();
                        (round is RoundRobinRound).Should().BeTrue();
                    }
                }
            }
        }

        [Then(@"groups within round (.*) in tournament named ""(.*)"" is of type ""(.*)""")]
        public void ThenGroupsWithinRoundInFetchedTournamentIsOfType(int roundIndex, string tournamentName, string groupType)
        {
            Tournament tournament;
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                tournament = tournamentRepository.GetTournamentByName(tournamentName);
            }

            RoundBase round = tournament.Rounds[roundIndex];
            ContestTypeEnum contestType = TestUtilities.ParseContestTypeString(groupType);

            if (contestType == ContestTypeEnum.Bracket)
            {
                foreach (GroupBase group in round.Groups)
                {
                    group.ContestType.Should().Be(ContestTypeEnum.Bracket);

                    (group is BracketGroup).Should().BeTrue();
                    (group is DualTournamentGroup).Should().BeFalse();
                    (group is RoundRobinGroup).Should().BeFalse();
                }
            }
            else if (contestType == ContestTypeEnum.DualTournament)
            {
                foreach (GroupBase group in round.Groups)
                {
                    group.ContestType.Should().Be(ContestTypeEnum.DualTournament);

                    (group is BracketGroup).Should().BeFalse();
                    (group is DualTournamentGroup).Should().BeTrue();
                    (group is RoundRobinGroup).Should().BeFalse();
                }
            }
            else if (contestType == ContestTypeEnum.RoundRobin)
            {
                foreach (GroupBase group in round.Groups)
                {
                    group.ContestType.Should().Be(ContestTypeEnum.RoundRobin);

                    (group is BracketGroup).Should().BeFalse();
                    (group is DualTournamentGroup).Should().BeFalse();
                    (group is RoundRobinGroup).Should().BeTrue();
                }
            }
        }

        protected UserRepository CreateUserRepository()
        {
            return new UserRepository(InMemoryContextCreator.Create(testDatabaseName));
        }

        protected TournamentRepository CreateTournamentRepository()
        {
            return new TournamentRepository(InMemoryContextCreator.Create(testDatabaseName));
        }

        private void PlayMatch(TournamentRepository tournamentRepository, Match match)
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

            tournamentRepository.AddScoreToPlayerInMatch(tournament, match.Id, scoringPlayerId, winningScore);
            tournamentRepository.Save();
        }
    }
}
