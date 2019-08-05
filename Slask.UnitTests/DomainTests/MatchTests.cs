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
        // public void CannotAddPointsToPlayersInMatchThatHasNotStarted

        [Fact]
        public void CanCreateMatch()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();

            group.Matches.Count.Should().Be(28);

            foreach (Match match in group.Matches)
            {
                match.Should().NotBeNull();
                match.Player1.Should().NotBeNull();
                match.Player2.Should().NotBeNull();
                match.StartDateTime.Should().NotBeBefore(DateTimeHelper.Now);
                match.GroupId.Should().Be(group.Id);
                match.Group.Should().Be(group);
            }
        }

        [Fact]
        public void MatchMustContainDifferentPlayers()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();

            foreach (Match match in group.Matches)
            {
                match.Player1.Should().NotBe(match.Player2);
            }
        }

        [Fact]
        public void CanFindPlayerInMatchByPlayerName()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();
            Match match = group.Matches.First();

            Player foundPlayer = match.FindPlayer(match.Player1.Name);

            foundPlayer.Should().NotBeNull();
            foundPlayer.Id.Should().Be(match.Player1.Id);
            foundPlayer.Name.Should().Be(match.Player1.Name);
        }

        [Fact]
        public void ReturnsNullWhenLookingForNonExistingPlayerInMatchByPlayerName()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();
            Match match = group.Matches.First();

            Player foundPlayer = match.FindPlayer("non-existing-player");

            foundPlayer.Should().BeNull();
        }

        [Fact]
        public void CanFindPlayerInMatchByPlayerId()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();
            Match match = group.Matches.First();

            Player foundPlayer = match.FindPlayer(match.Player1.Id);

            foundPlayer.Should().NotBeNull();
            foundPlayer.Id.Should().Be(match.Player1.Id);
            foundPlayer.Name.Should().Be(match.Player1.Name);
        }

        [Fact]
        public void ReturnsNullWhenLookingForNonExistingPlayerInMatchByPlayerId()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();
            Match match = group.Matches.First();

            Player foundPlayer = match.FindPlayer(Guid.NewGuid());

            foundPlayer.Should().BeNull();
            foundPlayer.Id.Should().Be(match.Player1.Id);
            foundPlayer.Name.Should().Be(match.Player1.Name);
        }

        [Fact]
        public void MatchStartDateTimeCannotBeChangedToSometimeInThePast()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();
            Match match = group.Matches.First();
            DateTime initialDateTime = match.StartDateTime;

            match.SetStartDateTime(DateTimeHelper.Now.AddSeconds(-1));

            match.StartDateTime.Should().Be(initialDateTime);
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
