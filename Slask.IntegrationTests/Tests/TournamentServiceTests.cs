using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.TestCore;
using System;
using System.Collections.Generic;
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
        public void CanGetAllPlayerReferencesInTournamentByTournamentId()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();
            Tournament tournament = group.Round.Tournament;

            List<PlayerReference> playerReferences = services.TournamentService.GetPlayerReferencesByTournamentId(tournament.Id);

            playerReferences.Should().NotBeNullOrEmpty();
            playerReferences.Count.Should().Be(8);

            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Maru").Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Stork").Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Taeja").Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Rain").Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Bomber").Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "FanTaSy").Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Stephano").Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Thorzain").Should().NotBeNull();
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
