using Moq;
using Slask.Common;
using Slask.TestCore;
using System;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class GroupTests
    {
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

        }

        [Fact]
        public void CannotAddBrandNewPlayerToMatchAfterGroupHasStartedPlaying()
        {

        }

        [Fact]
        public void CanAddSeveralPlayerReferencesAtOnce()
        {

        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
