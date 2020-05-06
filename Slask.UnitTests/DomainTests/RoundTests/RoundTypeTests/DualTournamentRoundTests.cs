using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.RoundTests.RoundTypeTests
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
            DualTournamentRound dualTournamentRound = DualTournamentRound.Create(tournament);

            dualTournamentRound.Should().NotBeNull();
            dualTournamentRound.Id.Should().NotBeEmpty();
            dualTournamentRound.Name.Should().Be("Round A");
            dualTournamentRound.BestOf.Should().Be(3);
            dualTournamentRound.AdvancingPerGroupCount.Should().Be(2);
            dualTournamentRound.Groups.Should().HaveCount(1);
            dualTournamentRound.TournamentId.Should().Be(tournament.Id);
            dualTournamentRound.Tournament.Should().Be(tournament);
        }
    }
}
