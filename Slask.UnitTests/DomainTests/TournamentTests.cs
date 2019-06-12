using FluentAssertions;
using Slask.Domain;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class TournamentTests
    {
        [Fact]
        public void TournamentHasNameInitially()
        {
            Tournament tournament = WhenTournamentCreated();
            tournament.Name.Should().NotBeEmpty();
        }

        [Fact]
        public void TournamentCreationFailsWithoutName()
        {
            Tournament tournament = Tournament.Create("");
            tournament.Should().BeNull();
        }

        [Fact]
        public void TournamentListsInitiallyEmpty()
        {
            Tournament tournament = WhenTournamentCreated();
            tournament.Rounds.Should().BeEmpty();
            tournament.Players.Should().BeEmpty();
            tournament.Betters.Should().BeEmpty();
            tournament.Settings.Should().BeEmpty();
            tournament.MiscBetCatalogue.Should().BeEmpty();
        }

        [Fact]
        public void TournamentCanBeRenamed()
        {
            Tournament tournament = WhenTournamentCreated();
            tournament.Rename("BHA Open 2019");
            
            tournament.Name.Should().Be("BHA Open 2019");
        }

        private Tournament WhenTournamentCreated()
        {
            return Tournament.Create("WCS 2019");
        }
    }
}
