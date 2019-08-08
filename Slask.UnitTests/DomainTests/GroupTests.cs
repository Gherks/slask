using FluentAssertions;
using Moq;
using Slask.Common;
using Slask.Domain;
using Slask.TestCore;
using System;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class GroupTests
    {
        [Fact]
        public void CanCreateRoundRobinRound()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_04_AddGroupToRoundRobinRound();

            group.Should().NotBeNull();
            group.Id.Should().NotBeEmpty();
            group.IsReady.Should().BeFalse();
            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().BeEmpty();
            group.RoundId.Should().Be(group.Round.Id);
            group.Round.Should().Be(group.Round);
        }

        [Fact]
        public void CanCreateBracketRound()
        {
            TournamentServiceContext services = GivenServices();
            BracketGroup group = services.HomestoryCup_12_AddGroupToBracketRound();

            group.Should().NotBeNull();
            group.Id.Should().NotBeEmpty();
            group.IsReady.Should().BeFalse();
            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().BeEmpty();
            group.RoundId.Should().Be(group.Round.Id);
            group.Round.Should().Be(group.Round);
        }

        [Fact]
        public void PlayerReferenceIsAddedToTournamentWhenBrandNewPlayerIsAddedToMatch()
        {
            var timeMock = new Mock<DateTimeProvider>();
            timeMock.SetupGet(tp => tp.Now).Returns(new DateTime(2010, 3, 11));
            DateTimeProvider.Current = timeMock.Object;
        }

        [Fact]
        public void PlayerReferenceIsAddedToGroupWhenBrandNewPlayerIsAddedToMatch()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CannotAddBrandNewPlayerToMatchAfterGroupHasStartedPlaying()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CanAddSeveralPlayerReferencesAtOnce()
        {
            throw new NotImplementedException();
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
