using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Bets;
using Slask.TestCore;
using System;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class BetterTests
    {
        private readonly Tournament tournament;
        private readonly User user;

        public BetterTests()
        {
            tournament = Tournament.Create("GSL 2019");
            user = User.Create("Stålberto");
        }

        [Fact]
        public void CanCreateBetter()
        {
            Better better = Better.Create(user, tournament);

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
