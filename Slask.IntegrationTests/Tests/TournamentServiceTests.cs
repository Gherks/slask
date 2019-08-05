using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.TestCore;
using System;
using System.Linq;
using Xunit;

namespace Slask.IntegrationTests.Tests
{
    [Collection("Integration test collection")]
    public class TournamentServiceTests
    {
        [Fact]
        public void CanDetermineMatchStatusCorrectly()
        {

        }

        [Fact]
        public void CanDetermineLoserOfMatch()
        {

        }

        [Fact]
        public void CanAddBetterToTournamentWithUserService()
        {
            TournamentServiceContext services = GivenServices();
        }

        [Fact]
        public void CanOnlyAddUserAsBetterOncePerTournament()
        {
            TournamentServiceContext services = GivenServices();
        }

        [Fact]
        public void CannotAddSamePlayerTwiceToMatch()
        {
            TournamentServiceContext services = GivenServices();
        }

        [Fact]
        public void BothPlayersMustHaveANameWhenAddingPlayersToMatch()
        {
            TournamentServiceContext services = GivenServices();
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new IntegrationTestSlaskContextCreator());
        }
    }
}
