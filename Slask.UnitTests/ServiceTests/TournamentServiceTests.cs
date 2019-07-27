using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.ServiceTests
{
    public class TournamentServiceTests
    {

        [Fact]
        public void CanGetTournamentById()
        {
            TournamentServiceContext services = GivenServices();
            Tournament createdTournament = services.WhenCreatedTournament();
            Tournament fetchedTournament = services.TournamentService.GetTournamentById(createdTournament.Id);

            fetchedTournament.Should().NotBeNull();
            fetchedTournament.Name.Should().Be("WCS 2019");
        }

        [Fact]
        public void CanGetTournamentByName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament createdTournament = services.WhenCreatedTournament();
            Tournament fetchedTournament = services.TournamentService.GetTournamentByName(createdTournament.Name);

            fetchedTournament.Should().NotBeNull();
            fetchedTournament.Name.Should().Be("WCS 2019");
        }

        [Fact]
        public void CanAddBetterToTournamentWithUserService()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedBetterInTournament();

            tournament.Betters.First().Should().NotBeNull();
        }

        [Fact]
        public void CanOnlyAddUserAsBetterOncePerTournament()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedBetterInTournament();
            Better better = tournament.AddBetter(services.UserService.GetUserByName(tournament.Betters.First().User.Name));

            better.Should().BeNull();
        }

        [Fact]
        public void PlayerNamesAreAddedToListWhenNewPlayersAreAddedTournament()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenAddedMatchesToTournament();

            tournament.PlayerReferences.Should().NotBeNull();
            tournament.PlayerReferences.Count.Should().Be(8);

            tournament.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == "Maru").Should().NotBeNull();
            tournament.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == "Stork").Should().NotBeNull();
            tournament.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == "Taeja").Should().NotBeNull();
            tournament.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == "Rain").Should().NotBeNull();
            tournament.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == "Bomber").Should().NotBeNull();
            tournament.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == "FanTaSy").Should().NotBeNull();
            tournament.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == "Stephano").Should().NotBeNull();
            tournament.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == "Thorzain").Should().NotBeNull();
        }

        [Fact]
        public void CanGetAllPlayerNamesInTournamentByTournamentId()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenAddedMatchesToTournament();

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
        public void CanGetAllPlayerNamesInTournamentByTournamentName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenAddedMatchesToTournament();

            List<PlayerReference> playerReferences = services.TournamentService.GetPlayerReferencesByTournamentName(tournament.Name);

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
        public void CanGetAllBettersInTournamentByTournamentName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedBetterInTournament();

            List<Better> betters = services.TournamentService.GetBettersByTournamentName(tournament.Name);

            betters.Should().NotBeNullOrEmpty();
            betters.Count.Should().Be(1);
        }

        [Fact]
        public void CanGetAllBettersInTournamentByTournamentId()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedBetterInTournament();

            List<Better> betters = services.TournamentService.GetBettersByTournamentId(tournament.Id);

            betters.Should().NotBeNullOrEmpty();
            betters.Count.Should().Be(1);
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
