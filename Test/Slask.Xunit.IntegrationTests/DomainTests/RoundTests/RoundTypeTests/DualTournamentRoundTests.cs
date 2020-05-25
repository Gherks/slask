using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds.RoundTypes;
using System.Linq;
using Xunit;

namespace Slask.Xunit.IntegrationTests.DomainTests.RoundTests.RoundTypeTests
{
    public class DualTournamentRoundTests
    {
        private readonly Tournament tournament;

        public DualTournamentRoundTests()
        {
            tournament = Tournament.Create("GSL 2019");
        }

        [Fact]
        public void CanCreateDualTournamentRound()
        {
            DualTournamentRound round = tournament.AddDualTournamentRound();

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be("Round A");
            round.PlayersPerGroupCount.Should().Be(4);
            round.AdvancingPerGroupCount.Should().Be(2);
            round.Groups.Should().HaveCount(1);
            round.TournamentId.Should().Be(tournament.Id);
            round.Tournament.Should().Be(tournament);
        }

        [Fact]
        public void AdvancingCountInDualTournamentRoundCannotBeAnythingOtherThanTwo()
        {
            DualTournamentRound round = tournament.AddDualTournamentRound();

            for (int advancingPerGroupCount = 0; advancingPerGroupCount < 16; ++advancingPerGroupCount)
            {
                round.SetAdvancingPerGroupCount(advancingPerGroupCount);
                round.AdvancingPerGroupCount.Should().Be(2);
            }
        }

        [Fact]
        public void CannotChangeGroupSize()
        {
            DualTournamentRound round = tournament.AddDualTournamentRound();

            round.Groups.First().Matches.Should().HaveCount(5);
            round.PlayersPerGroupCount.Should().Be(4);

            round.SetPlayersPerGroupCount(8);

            round.Groups.First().Matches.Should().HaveCount(5);
            round.PlayersPerGroupCount.Should().Be(4);
        }
    }
}
