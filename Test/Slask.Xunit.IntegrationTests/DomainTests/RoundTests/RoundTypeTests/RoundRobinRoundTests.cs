using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Domain.Utilities;
using System;
using System.Linq;
using Xunit;

namespace Slask.Xunit.IntegrationTests.DomainTests.RoundTests.RoundTypeTests
{
    public class RoundRobinRoundTests : IDisposable
    {
        private readonly Tournament tournament;
        private readonly RoundRobinRound round;

        public RoundRobinRoundTests()
        {
            tournament = Tournament.Create("GSL 2019");
            round = tournament.AddRoundRobinRound();
        }

        public void Dispose()
        {
            SystemTimeMocker.Reset();
        }

        [Fact]
        public void CanCreateRoundRobinRound()
        {
            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be("Round A");
            round.PlayersPerGroupCount.Should().Be(2);
            round.AdvancingPerGroupCount.Should().Be(1);
            round.Groups.Should().HaveCount(1);
            round.TournamentId.Should().Be(tournament.Id);
            round.Tournament.Should().Be(tournament);
            round.HasProblematicTie().Should().BeFalse();
        }

        [Fact]
        public void CanChangeAdvancingPerGroupCount()
        {
            RoundRobinRound round = tournament.AddRoundRobinRound();

            round.AdvancingPerGroupCount.Should().Be(1);
            round.SetAdvancingPerGroupCount(4);
            round.AdvancingPerGroupCount.Should().Be(4);
        }

        [Fact]
        public void CannotSetAdvancingPerGroupCountToAnythingLessThanOne()
        {
            RoundRobinRound round = tournament.AddRoundRobinRound();

            round.AdvancingPerGroupCount.Should().Be(1);

            round.SetPlayersPerGroupCount(0);
            round.SetPlayersPerGroupCount(-1);
            round.SetPlayersPerGroupCount(-2);

            round.AdvancingPerGroupCount.Should().Be(1);
        }

        [Fact]
        public void CanChangePlayersPerGroupSize()
        {
            RoundRobinRound round = tournament.AddRoundRobinRound();

            round.Groups.First().Matches.Should().HaveCount(1);
            round.PlayersPerGroupCount.Should().Be(2);

            round.SetPlayersPerGroupCount(4);

            round.Groups.First().Matches.Should().HaveCount(6);
            round.PlayersPerGroupCount.Should().Be(4);
        }

        [Fact]
        public void CannotSetPlayersPerGroupSizeToAnythingLessThanTwo()
        {
            RoundRobinRound round = tournament.AddRoundRobinRound();

            round.Groups.First().Matches.Should().HaveCount(1);
            round.PlayersPerGroupCount.Should().Be(2);

            round.SetPlayersPerGroupCount(1);
            round.SetPlayersPerGroupCount(0);
            round.SetPlayersPerGroupCount(-1);

            round.Groups.First().Matches.Should().HaveCount(1);
            round.PlayersPerGroupCount.Should().Be(2);
        }

        [Fact]
        public void DoesNotFlagRoundAsProlematicTieWhenNoProblematicTieHappens()
        {
            round.SetPlayersPerGroupCount(3);
            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            round.RegisterPlayerReference("Taeja");

            Match match;

            match = round.Groups.First().Matches[0];
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.Player1.IncreaseScore(2);

            match = round.Groups.First().Matches[1];
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.Player2.IncreaseScore(2);

            match = round.Groups.First().Matches[2];
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.Player1.IncreaseScore(2);

            round.HasProblematicTie().Should().BeFalse();
        }

        [Fact]
        public void CanDetectWhenRoundRobinRoundContainsProblematicTie()
        {
            round.SetPlayersPerGroupCount(3);
            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            round.RegisterPlayerReference("Taeja");

            foreach (Match match in round.Groups.First().Matches)
            {
                SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
                match.Player1.IncreaseScore(2);
            }

            round.HasProblematicTie().Should().BeTrue();
        }

        [Fact]
        public void RoundIsOngoingUntilTieIsSolved()
        {
            round.SetPlayersPerGroupCount(3);
            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            round.RegisterPlayerReference("Taeja");

            foreach (Match match in round.Groups.First().Matches)
            {
                SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
                match.Player1.IncreaseScore(2);
            }

            round.GetPlayState().Should().Be(PlayState.Ongoing);
        }

        [Fact]
        public void SolvingTieWhenThereIsNoTieDoesNothing()
        {
            round.SetPlayersPerGroupCount(3);
            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            round.RegisterPlayerReference("Taeja");

            Match match;

            match = round.Groups.First().Matches[0];
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.Player1.IncreaseScore(2);

            match = round.Groups.First().Matches[1];
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.Player2.IncreaseScore(2);

            match = round.Groups.First().Matches[2];
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.Player1.IncreaseScore(2);

            round.Groups.First().SolveTieByChoosing("Maru");

            round.HasProblematicTie().Should().BeFalse();
            round.Groups.First().ChoosenTyingPlayerEntries.Should().BeEmpty();
        }
    }
}
