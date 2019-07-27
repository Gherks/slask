using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.TestCore;
using System;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class MatchTests
    {
        [Fact]
        public void EnsureMatchIsValidWhenAddedToTournament()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.GetRoundByRoundName("Round-Robin Group A").Groups.First().Matches.First();

            match.Should().NotBeNull();
            match.Player1.Should().NotBeNull();
            match.Player2.Should().NotBeNull();
            match.StartDateTime.Should().NotBeBefore(DateTimeHelper.Now);
            match.Group.Should().NotBeNull();
        }

        [Fact]
        public void TournamentMatchMustContainTwoPlayers()
        {
            TournamentServiceContext services = GivenServices();
            Group group = services.WhenCreatedGroupInRoundRobinRoundInTournament();
            Match matchMissingBothPlayers = group.AddMatch("", "", DateTimeHelper.Now.AddSeconds(1));

            matchMissingFirstPlayer.Should().BeNull();
            matchMissingSecondPlayer.Should().BeNull();
            matchMissingBothPlayers.Should().BeNull();
        }

        [Fact]
        public void TournamentMatchMustContainDifferentPlayers()
        {
            TournamentServiceContext services = GivenServices();
            Group group = services.WhenCreatedGroupInRoundRobinRoundInTournament();
            Match match = group.AddMatch("Maru", "Maru", DateTimeHelper.Now.AddSeconds(1));

            match.Should().BeNull();
        }

        [Fact]
        public void CanFindPlayerInMatchByPlayerName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.GetRoundByRoundName("Round-Robin Group A").Groups.First().Matches.First();

            match.ContainsPlayer(match.Player1.Name).Should().BeTrue();
        }

        [Fact]
        public void CanFindPlayerInMatchByPlayerId()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.GetRoundByRoundName("Round-Robin Group A").Groups.First().Matches.First();

            match.ContainsPlayer(match.Player1.Id).Should().BeTrue();
        }

        [Fact]
        public void MatchStartDateTimeMustBeInTheFuture()
        {
            TournamentServiceContext services = GivenServices();
            Group group = services.WhenCreatedGroupInRoundRobinRoundInTournament();

            Match match = group.AddMatch("Maru", "Stork", DateTimeHelper.Now.AddSeconds(-1));

            match.Should().BeNull();
        }

        [Fact]
        public void MatchStartDateCannotBeChangedToSometimeInThePast()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.GetRoundByRoundName("Round-Robin Group A").Groups.First().Matches.First();

            DateTime currentMathTime = match.StartDateTime;
            match.ChangeStartDateTime(DateTimeHelper.Now.AddSeconds(-1));

            match.StartDateTime.Should().Be(currentMathTime);
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
