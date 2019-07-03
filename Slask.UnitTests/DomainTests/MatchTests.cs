using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using System;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class MatchTests
    {
        [Fact]
        public void EnsureMatchIsValidWhenAddedToTournament()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.Should().NotBeNull();
            match.Player1.Should().NotBeNull();
            match.Player2.Should().NotBeNull();
            match.StartDateTime.Should().NotBeBefore(DateTime.Now);
            match.Group.Should().NotBeNull();
        }

        [Fact]
        public void CanFindPlayerInMatchByPlayerName()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.ContainsPlayer(match.Player1.Name).Should().BeTrue();
        }

        [Fact]
        public void CanFindPlayerInMatchByPlayerId()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.ContainsPlayer(match.Player1.Id).Should().BeTrue();
        }

        [Fact]
        public void MatchStartDateTimeMustBeInTheFuture()
        {
            TournamentServiceContext services = GivenServices();
            Group group = services.WhenAddedGroupToTournament();

            Match match = group.AddMatch("Maru", "Stork", DateTime.Now.AddSeconds(-1));

            match.Should().BeNull();
        }

        [Fact]
        public void MatchStartDateCannotBeChangedToSometimeInThePast()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();
            DateTime currentMathTime = match.StartDateTime;

            match.ChangeStartDateTime(DateTime.Now.AddSeconds(-1));

            match.StartDateTime.Should().Be(currentMathTime);
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
