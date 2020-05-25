using FluentAssertions;
using Slask.Domain;
using Xunit;

namespace Slask.Xunit.IntegrationTests.DomainTests
{
    public class BetterTests
    {
        private readonly User user;
        private readonly Tournament tournament;

        public BetterTests()
        {
            user = User.Create("Stålberto");
            tournament = Tournament.Create("GSL 2019");
        }

        [Fact]
        public void CanCreateBetter()
        {
            Better better = tournament.AddBetter(user);

            better.Id.Should().NotBeEmpty();
            better.User.Should().NotBeNull();
            better.Bets.Should().BeEmpty();
            better.TournamentId.Should().Be(tournament.Id);
            better.Tournament.Should().Be(tournament);
        }

        [Fact]
        public void CannotCreateBetterWithNullUser()
        {
            Better better = Better.Create(null, tournament);

            better.Should().BeNull();
        }

        [Fact]
        public void CannotCreateBetterWithNullTournament()
        {
            Better better = Better.Create(user, null);

            better.Should().BeNull();
        }
    }
}
