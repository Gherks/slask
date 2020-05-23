using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.PlayerTests
{
    public class PlayerInBracketGroupTests
    {
        private readonly List<string> playerNames = new List<string> { "Maru", "Stork", "Taeja", "Rain" };

        private readonly Tournament tournament;
        private readonly BracketRound round;
        private readonly BracketGroup group;
        private readonly Match match;
        private readonly Player player;

        public PlayerInBracketGroupTests()
        {
            tournament = Tournament.Create("GSL 2019");
            round = tournament.AddBracketRound();
            round.SetBestOf(5);
            round.SetAdvancingPerGroupCount(1);
            round.SetPlayersPerGroupCount(4);

            foreach (string playerName in playerNames)
            {
                round.RegisterPlayerReference(playerName);
            }

            group = round.Groups.First() as BracketGroup;
            match = group.Matches.First();
            player = match.Player1;
        }

        [Fact]
        public void CanCreatePlayer()
        {
            player.Should().NotBeNull();
            player.Id.Should().NotBeEmpty();
            player.PlayerReference.Name.Should().Be(playerNames.First());
            player.Score.Should().Be(0);
            player.MatchId.Should().Be(match.Id);
            player.Match.Should().Be(match);
        }

        [Fact]
        public void CanIncreaseScore()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            int score = 1;

            player.IncreaseScore(score).Should().BeTrue();

            player.Score.Should().Be(score);
        }

        [Fact]
        public void CanDecreasePlayerScore()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            player.IncreaseScore(2).Should().BeTrue();
            player.DecreaseScore(1).Should().BeTrue();

            player.Score.Should().Be(1);
        }

        [Fact]
        public void CannotDecreasePlayerScoreBelowZero()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            player.DecreaseScore(1).Should().BeTrue();

            player.Score.Should().Be(0);
        }

        [Fact]
        public void CannotIncreasePlayerScoreWhenTournamentHasIssues()
        {
            TurnTournamentInvalid();

            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            player.IncreaseScore(1).Should().BeFalse();

            player.Score.Should().Be(0);
        }

        [Fact]
        public void CannotDecreasePlayerScoreWhenTournamentHasIssues()
        {
            TurnTournamentInvalid();

            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            player.DecreaseScore(1).Should().BeFalse();

            player.Score.Should().Be(0);
        }

        private void TurnTournamentInvalid()
        {
            tournament.AddDualTournamentRound();
        }
    }
}
