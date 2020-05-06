using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.RoundTests.RoundTypeTests
{
    public class RoundRobinRoundTests
    {
        private readonly Tournament tournament;

        public RoundRobinRoundTests()
        {
            tournament = Tournament.Create("GSL 2019");
        }

        [Fact]
        public void CanCreateRoundRobinRound()
        {
            RoundRobinRound roundRobinRound = RoundRobinRound.Create(tournament);

            roundRobinRound.Should().NotBeNull();
            roundRobinRound.Id.Should().NotBeEmpty();
            roundRobinRound.Name.Should().Be("Round A");
            roundRobinRound.PlayersPerGroupCount.Should().Be(2);
            roundRobinRound.BestOf.Should().Be(3);
            roundRobinRound.AdvancingPerGroupCount.Should().Be(1);
            roundRobinRound.Groups.Should().HaveCount(1);
            roundRobinRound.TournamentId.Should().Be(tournament.Id);
            roundRobinRound.Tournament.Should().Be(tournament);
        }
    }
}
