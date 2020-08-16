using FluentAssertions;
using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Domain.Utilities;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Slask.Domain.SpecFlow.IntegrationTests.RoundTests
{
    [Binding, Scope(Feature = "Round")]
    public class RoundSteps : RoundStepDefinitions
    {

    }

    public class RoundStepDefinitions : TournamentStepDefinitions
    {
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
                TestUtilities.ParseScoreAddedToMatchPlayer(row, out int _, out int groupIndex, out int matchIndex, out string scoringPlayer, out int scoreAdded);

                GroupBase group = createdGroups[groupIndex];
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

        [When(@"players per group count in round (.*) is set to (.*)")]
        public void WhenPlayersPerGroupCountInRoundIsSetTo(int roundIndex, int playersPerGroupCount)
        {
            RoundBase round = createdRounds[roundIndex];

            round.SetPlayersPerGroupCount(playersPerGroupCount);
        }

        [When(@"advancing per group count in round (.*) is set to (.*)")]
        public void WhenAdvancingPerGroupCountInRoundIsSetTo(int roundIndex, int playersPerGroupCount)
        {
            RoundBase round = createdRounds[roundIndex];

            round.SetAdvancingPerGroupCount(playersPerGroupCount);
        }

        [Then(@"rounds in tournament (.*) should be valid with values")]
        public void ThenRoundsInTournamentShouldBeValidWithValues(int tournamentIndex, Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            Tournament tournament = createdTournaments[tournamentIndex];

            for (int index = 0; index < table.Rows.Count; ++index)
            {
                TestUtilities.ParseRoundTable(table.Rows[index], out string roundType, out string name, out int advancingCount, out int playersPerGroupCount);

                RoundBase round = tournament.Rounds[index];

                if (round is BracketRound bracketRound)
                {
                    roundType.Should().Be("Bracket");
                }
                else if (round is DualTournamentRound dualTournamentRound)
                {
                    roundType.Should().Be("Dual tournament");
                }
                else if (round is RoundRobinRound roundRobinRound)
                {
                    roundType.Should().Be("Round robin");
                }

                CheckRoundValidity(round, name, advancingCount, playersPerGroupCount);
            }
        }

        //[Then(@"fetched round (.*) in tournament should be valid with values")]
        //public void ThenFetchedRoundInTournamentShouldBeValidWithValues(int roundIndex, Table table)
        //{
        //    if (table == null)
        //    {
        //        throw new ArgumentNullException(nameof(table));
        //    }

        //    RoundBase round = fetchedRounds[roundIndex];

        //    for (int index = 0; index < table.Rows.Count; ++index)
        //    {
        //        TestUtilities.ParseRoundTable(table.Rows[index], out string roundType, out string name, out int advancingCount, out int playersPerGroupCount);

        //        if (round is BracketRound bracketRound)
        //        {
        //            roundType.Should().Be("Bracket");
        //        }
        //        else if (round is DualTournamentRound dualTournamentRound)
        //        {
        //            roundType.Should().Be("Dual tournament");
        //        }
        //        else if (round is RoundRobinRound roundRobinRound)
        //        {
        //            roundType.Should().Be("Round robin");
        //        }

        //        CheckRoundValidity(round, name, advancingCount, playersPerGroupCount);
        //    }
        //}

        [Then(@"players per group count in round (.*) should be (.*)")]
        public void PlayersPerGroupCountInRoundIsSetTo(int roundIndex, int playersPerGroupCount)
        {
            RoundBase round = createdRounds[roundIndex];

            round.PlayersPerGroupCount.Should().Be(playersPerGroupCount);
        }

        [Then(@"advancing per group count in round (.*) should be (.*)")]
        public void AdvancingPerGroupCountInRoundIsSetTo(int roundIndex, int advancingPerGrouCount)
        {
            RoundBase round = createdRounds[roundIndex];

            round.AdvancingPerGroupCount.Should().Be(advancingPerGrouCount);
        }

        [Then(@"play state of round (.*) is set to ""(.*)""")]
        public void ThenPlayStateOfRoundIsSetTo(int roundIndex, string playStateString)
        {
            RoundBase round = createdRounds[roundIndex];

            PlayStateEnum playState = ParsePlayStateString(playStateString);

            round.GetPlayState().Should().Be(playState);
        }

        [Then(@"advancing players in group (.*) is exactly ""(.*)""")]
        public void ThenAdvancingPlayersInGroupIsExactly(int groupIndex, string commaSeparatedPlayerNames)
        {
            GroupBase group = createdGroups[groupIndex];
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            List<PlayerReference> playerReferences = AdvancingPlayersSolver.FetchFrom(group);

            playerReferences.Should().HaveCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                playerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }
        protected static void CheckRoundValidity(RoundBase round, string correctName, int advancingCount, int playersPerGroupCount)
        {
            if (round == null)
            {
                throw new ArgumentNullException(nameof(round));
            }

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be(correctName);
            round.PlayersPerGroupCount.Should().Be(playersPerGroupCount);
            round.AdvancingPerGroupCount.Should().Be(advancingCount);
            round.Groups.Should().HaveCount(1);
            round.TournamentId.Should().NotBeEmpty();
            round.Tournament.Should().NotBeNull();
        }
    }
}
