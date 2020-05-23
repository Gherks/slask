using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using System.Linq;
using Xunit;

namespace Slask.Xunit.UnitTests.DomainTests.RoundTests.RoundTypeTests
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
            DualTournamentRound round = DualTournamentRound.Create(tournament);

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be("Round A");
            round.PlayersPerGroupCount.Should().Be(4);
            round.BestOf.Should().Be(3);
            round.AdvancingPerGroupCount.Should().Be(2);
            round.Groups.Should().HaveCount(0);
            round.TournamentId.Should().Be(tournament.Id);
            round.Tournament.Should().Be(tournament);
        }
	}
}
