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
        public void CanCreateTournament()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.TournamentService.CreateTournament("GSL 2019");

            tournament.Should().NotBeNull();
            tournament.Should().NotBeNull();

            tournament.Id.Should().NotBeEmpty();
            tournament.Name.Should().Be("GSL 2019");
            tournament.Rounds.Should().BeEmpty();
            tournament.PlayerReferences.Should().BeEmpty();
            tournament.Betters.Should().BeEmpty();
            tournament.Settings.Should().BeEmpty();
            tournament.MiscBetCatalogue.Should().BeEmpty();
        }

        [Fact]
        public void CannotCreateTournamentWithEmptyName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.TournamentService.CreateTournament("");

            tournament.Should().BeNull();
        }

        [Fact]
        public void CannotCreateTournamentWithNameAlreadyInUseNoMatterLetterCasing()
        {
            TournamentServiceContext services = GivenServices();
            Tournament firstTournament = HomestoryCupSetup.Part01_CreateTournament(services);
            Tournament secondTournament = services.TournamentService.CreateTournament(firstTournament.Name.ToUpper());

            secondTournament.Should().BeNull();
        }

        [Fact]
        public void CanRenameTournament()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part01_CreateTournament(services);

            bool result = services.TournamentService.RenameTournament(tournament.Id, "BHA Open 2019");

            result.Should().BeTrue();
            tournament.Name.Should().Be("BHA Open 2019");
        }

        [Fact]
        public void CannotRenameTournamentToEmptyName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part01_CreateTournament(services);

            bool result = services.TournamentService.RenameTournament(tournament.Id, "");

            result.Should().BeFalse();
            tournament.Name.Should().Be("Homestory Cup");
        }

        [Fact]
        public void CannotRenameTournamentToNameAlreadyInUseNoMatterLetterCasing()
        {
            TournamentServiceContext services = GivenServices();
            Tournament firstTournament = HomestoryCupSetup.Part01_CreateTournament(services);
            Tournament secondTournament = services.TournamentService.CreateTournament("BHA Open 2019");

            bool result = services.TournamentService.RenameTournament(secondTournament.Id, firstTournament.Name.ToUpper());

            result.Should().BeFalse();
            secondTournament.Name.Should().Be("BHA Open 2019");
        }

        [Fact]
        public void CannotRenameNonexistingTournament()
        {
            TournamentServiceContext services = GivenServices();

            bool result = services.TournamentService.RenameTournament(Guid.NewGuid(), "BHA Open 2019");

            result.Should().BeFalse();
        }

        [Fact]
        public void CanGetTournamentById()
        {
            TournamentServiceContext services = GivenServices();
            Tournament createdTournament = HomestoryCupSetup.Part01_CreateTournament(services);
            Tournament fetchedTournament = services.TournamentService.GetTournamentById(createdTournament.Id);

            fetchedTournament.Should().NotBeNull();
            fetchedTournament.Name.Should().Be("Homestory Cup");
        }

        [Fact]
        public void CanGetTournamentByName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament createdTournament = HomestoryCupSetup.Part01_CreateTournament(services);
            Tournament fetchedTournament = services.TournamentService.GetTournamentByName(createdTournament.Name);

            fetchedTournament.Should().NotBeNull();
            fetchedTournament.Name.Should().Be("Homestory Cup");
        }

        [Fact]
        public void CanAddBetterToTournamentWithUserService()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part02_BettersAddedToTournament(services);

            tournament.Betters.First().Should().NotBeNull();
        }

        [Fact]
        public void CanOnlyAddUserAsBetterOncePerTournament()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part02_BettersAddedToTournament(services);
            Better better = tournament.AddBetter(services.UserService.GetUserByName(tournament.Betters.First().User.Name));

            better.Should().BeNull();
        }

        [Fact]
        public void CanGetAllBettersInTournamentByTournamentId()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part02_BettersAddedToTournament(services);

            List<Better> betters = services.TournamentService.GetBettersByTournamentId(tournament.Id);

            betters.Should().NotBeNullOrEmpty();
            betters.Should().HaveCount(3);
            betters[0].User.Name.Should().Be("Stålberto");
            betters[1].User.Name.Should().Be("Bönis");
            betters[2].User.Name.Should().Be("Guggelito");
        }

        [Fact]
        public void CanGetAllBettersInTournamentByTournamentName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part02_BettersAddedToTournament(services);

            List<Better> betters = services.TournamentService.GetBettersByTournamentName(tournament.Name);

            betters.Should().NotBeNullOrEmpty();
            betters.Should().HaveCount(3);
            betters[0].User.Name.Should().Be("Stålberto");
            betters[1].User.Name.Should().Be("Bönis");
            betters[2].User.Name.Should().Be("Guggelito");
        }

        [Fact]
        public void PlayerReferencesAreAddedToTournamentWhenNewPlayersAreAddedTournament()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup(services);
            Tournament tournament = group.Round.Tournament;

            tournament.PlayerReferences.Should().NotBeNull();
            tournament.PlayerReferences.Should().HaveCount(8);

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
        public void CanGetAllPlayerReferencesInTournamentByTournamentId()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup(services);
            Tournament tournament = group.Round.Tournament;

            List<PlayerReference> playerReferences = services.TournamentService.GetPlayerReferencesByTournamentId(tournament.Id);

            playerReferences.Should().NotBeNullOrEmpty();
            playerReferences.Should().HaveCount(8);

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
        public void CanGetAllPlayerReferencesInTournamentByTournamentName()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup(services);
            Tournament tournament = group.Round.Tournament;

            List<PlayerReference> playerReferences = services.TournamentService.GetPlayerReferencesByTournamentName(tournament.Name);

            playerReferences.Should().NotBeNullOrEmpty();
            playerReferences.Should().HaveCount(8);

            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Maru").Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Stork").Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Taeja").Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Rain").Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Bomber").Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "FanTaSy").Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Stephano").Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Thorzain").Should().NotBeNull();
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
