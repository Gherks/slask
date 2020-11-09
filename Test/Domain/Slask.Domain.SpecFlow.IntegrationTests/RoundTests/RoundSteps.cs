using FluentAssertions;
using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

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

            foreach (AssignScoreToPlayer assignScoreToPlayer in table.CreateSet<AssignScoreToPlayer>())
            {
                GroupBase group = createdGroups[assignScoreToPlayer.GroupIndex];
                Match match = group.Matches[assignScoreToPlayer.MatchIndex];

                SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

                Guid playerReferenceId = match.FindPlayer(assignScoreToPlayer.ScoringPlayer);

                if (playerReferenceId != Guid.Empty)
                {
                    match.IncreaseScoreForPlayer(playerReferenceId, assignScoreToPlayer.ScoreAdded);
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

            for (int index = 0; index < table.RowCount; ++index)
            {
                RoundSettings roundSettings = table.Rows[index].CreateInstance<RoundSettings>();

                RoundBase round = tournament.Rounds[index];

                if (round is BracketRound bracketRound)
                {
                    roundSettings.ContestType.Should().Be(ContestTypeEnum.Bracket);
                }
                else if (round is DualTournamentRound dualTournamentRound)
                {
                    roundSettings.ContestType.Should().Be(ContestTypeEnum.DualTournament);
                }
                else if (round is RoundRobinRound roundRobinRound)
                {
                    roundSettings.ContestType.Should().Be(ContestTypeEnum.RoundRobin);
                }

                CheckRoundValidity(round, roundSettings);
            }
        }

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

            Enum.TryParse(playStateString, out PlayStateEnum playState);

            round.GetPlayState().Should().Be(playState);
        }

        [Then(@"advancing players in group (.*) is exactly ""(.*)""")]
        public void ThenAdvancingPlayersInGroupIsExactly(int groupIndex, string commaSeparatedPlayerNames)
        {
            GroupBase group = createdGroups[groupIndex];
            List<string> playerNames = commaSeparatedPlayerNames.ToStringList(",");

            List<PlayerReference> playerReferences = AdvancingPlayersSolver.FetchFrom(group);

            playerReferences.Should().HaveCount(playerNames.Count);
            foreach (string playerName in playerNames)
            {
                playerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }
        private static void CheckRoundValidity(RoundBase round, RoundSettings roundSettings)
        {
            if (round == null)
            {
                throw new ArgumentNullException(nameof(round));
            }

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be(roundSettings.Name);
            round.PlayersPerGroupCount.Should().Be(roundSettings.PlayersPerGroupCount);
            round.AdvancingPerGroupCount.Should().Be(roundSettings.AdvancingCount);
            round.Groups.Should().HaveCount(1);
            round.TournamentId.Should().NotBeEmpty();
            round.Tournament.Should().NotBeNull();
        }

        private sealed class RoundSettings
        {
            public ContestTypeEnum ContestType { get; set; }
            public string Name { get; set; }
            public int AdvancingCount { get; set; }
            public int PlayersPerGroupCount { get; set; }
        }

        private sealed class AssignScoreToPlayer
        {
            public int GroupIndex { get; set; }
            public int MatchIndex { get; set; }
            public string ScoringPlayer { get; set; }
            public int ScoreAdded { get; set; }
        }
    }
}
