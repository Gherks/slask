using FluentAssertions;
using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Domain.Utilities;
using System.Linq;
using Xunit;

namespace Slask.Domain.Xunit.IntegrationTests.GroupTests.GroupTypeTests
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
            tournament.RegisterPlayerReference("Maru");
            tournament.RegisterPlayerReference("Stork");
            tournament.RegisterPlayerReference("Taeja");

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
            tournament.RegisterPlayerReference("Maru");
            tournament.RegisterPlayerReference("Stork");
            tournament.RegisterPlayerReference("Taeja");
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
            tournament.RegisterPlayerReference("Maru");
            tournament.RegisterPlayerReference("Stork");
            tournament.RegisterPlayerReference("Taeja");
            GroupBase group = round.Groups.First();

            foreach (Match match in round.Groups.First().Matches)
            {
                SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
                match.Player1.IncreaseScore(2);
            }

            group.GetPlayState().Should().Be(PlayStateEnum.Ongoing);
        }

        [Fact]
        public void CanSolveTie()
        {
            round.SetPlayersPerGroupCount(3);
            round.SetAdvancingPerGroupCount(2);
            PlayerReference maruPlayerReference = tournament.RegisterPlayerReference("Maru");
            PlayerReference storkPlayerReference = tournament.RegisterPlayerReference("Stork");
            PlayerReference taejaPlayerReference = tournament.RegisterPlayerReference("Taeja");

            BracketRound bracketRound = tournament.AddBracketRound();

            GroupBase group = round.Groups.First();

            foreach (Match match in round.Groups.First().Matches)
            {
                SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
                match.Player1.IncreaseScore(2);
            }

            group.HasProblematicTie().Should().BeTrue();
            group.HasSolvedTie().Should().BeFalse();
            group.SolveTieByChoosing(taejaPlayerReference.Id).Should().BeFalse();

            group.HasProblematicTie().Should().BeTrue();
            group.HasSolvedTie().Should().BeFalse();
            group.SolveTieByChoosing(storkPlayerReference.Id).Should().BeTrue();

            group.HasProblematicTie().Should().BeTrue();
            group.HasSolvedTie().Should().BeTrue();

            bracketRound.Groups.First().Matches[0].Player1.GetName().Should().Be("Taeja");
            bracketRound.Groups.First().Matches[0].Player2.GetName().Should().Be("Stork");
        }

        [Fact]
        public void SolvingTieWhenThereIsNoTieDoesNothing()
        {
            round.SetPlayersPerGroupCount(3);
            PlayerReference maruPlayerReference = tournament.RegisterPlayerReference("Maru");
            PlayerReference storkPlayerReference = tournament.RegisterPlayerReference("Stork");
            PlayerReference taejaPlayerReference = tournament.RegisterPlayerReference("Taeja");

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
            group.SolveTieByChoosing(maruPlayerReference.Id).Should().BeFalse();
        }
    }
}
