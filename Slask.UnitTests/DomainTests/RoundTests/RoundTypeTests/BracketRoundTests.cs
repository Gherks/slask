using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.RoundTests.RoundTypeTests
{
    public class BracketRoundTests
    {
        private readonly Tournament tournament;

        public BracketRoundTests()
        {
            tournament = Tournament.Create("GSL 2019");
        }

        [Fact]
        public void CanCreateBracketRound()
        {
            BracketRound bracketRound = BracketRound.Create(tournament);

            bracketRound.Should().NotBeNull();
            bracketRound.Id.Should().NotBeEmpty();
            bracketRound.Name.Should().Be("Round A");
            bracketRound.PlayersPerGroupCount.Should().Be(2);
            bracketRound.BestOf.Should().Be(3);
            bracketRound.AdvancingPerGroupCount.Should().Be(1);
            bracketRound.Groups.Should().HaveCount(1);
            bracketRound.TournamentId.Should().Be(tournament.Id);
            bracketRound.Tournament.Should().Be(tournament);
        }
    }
}
