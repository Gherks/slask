using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.GroupTests
{
    public class RoundRobinGroupTests
    {
        private readonly Tournament tournament;
        private readonly RoundRobinRound round;

        public RoundRobinGroupTests()
        {
            tournament = Tournament.Create("GSL 2019");
            round = tournament.AddRoundRobinRound() as RoundRobinRound;
        }

        [Fact]
        public void DoesNotFlagRoundAsProlematicTieWhenNoProblematicTieHappens()
        {
            round.SetPlayersPerGroupCount(3);
            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            round.RegisterPlayerReference("Taeja");

            GroupBase group = round.Groups.First();
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

            group.HasProblematicTie().Should().BeFalse();
        }

        [Fact]
        public void CanDetectWhenRoundRobinRoundContainsProblematicTie()
        {
            round.SetPlayersPerGroupCount(3);
            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            round.RegisterPlayerReference("Taeja");
            GroupBase group = round.Groups.First();

            foreach (Match match in round.Groups.First().Matches)
            {
                SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
                match.Player1.IncreaseScore(2);
            }

            group.HasProblematicTie().Should().BeTrue();
        }

        [Fact]
        public void RoundIsOngoingUntilTieIsSolved()
        {
            round.SetPlayersPerGroupCount(3);
            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            round.RegisterPlayerReference("Taeja");
            GroupBase group = round.Groups.First();

            foreach (Match match in round.Groups.First().Matches)
            {
                SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
                match.Player1.IncreaseScore(2);
            }

            group.GetPlayState().Should().Be(PlayState.Ongoing);
        }

        [Fact]
        public void CanSolveTie()
        {
            round.SetPlayersPerGroupCount(3);
            round.SetAdvancingPerGroupCount(2);
            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            round.RegisterPlayerReference("Taeja");

            BracketRound bracketRound = tournament.AddBracketRound();

            GroupBase group = round.Groups.First();

            foreach (Match match in round.Groups.First().Matches)
            {
                SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
                match.Player1.IncreaseScore(2);
            }

            group.HasProblematicTie().Should().BeTrue();
            group.HasSolvedTie().Should().BeFalse();
            group.SolveTieByChoosing("Taeja").Should().BeFalse();

            group.HasProblematicTie().Should().BeTrue();
            group.HasSolvedTie().Should().BeFalse();
            group.SolveTieByChoosing("Stork").Should().BeTrue();

            group.HasProblematicTie().Should().BeTrue();
            group.HasSolvedTie().Should().BeTrue();

            bracketRound.Groups.First().Matches[0].Player1.Name.Should().Be("Taeja");
            bracketRound.Groups.First().Matches[0].Player2.Name.Should().Be("Stork");
        }

        [Fact]
        public void SolvingTieWhenThereIsNoTieDoesNothing()
        {
            round.SetPlayersPerGroupCount(3);
            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            round.RegisterPlayerReference("Taeja");

            GroupBase group = round.Groups.First();
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

            group.HasProblematicTie().Should().BeFalse();
            group.SolveTieByChoosing("Maru").Should().BeFalse();
        }
    }
}
