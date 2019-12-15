using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.ServiceTests
{
    public class TournamentServiceTests
    {
        private readonly TournamentServiceContext services;
        private readonly Tournament tournament;

        public TournamentServiceTests()
        {
            services = TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
            tournament = services.TournamentService.CreateTournament("GSL 2019");
        }

        [Fact]
        public void CanCreateTournament()
        {
            string tournamentName = "Homestorycup XX";

            Tournament tournament = services.TournamentService.CreateTournament(tournamentName);

            tournament.Should().NotBeNull();
            tournament.Id.Should().NotBeEmpty();
            tournament.Name.Should().Be(tournamentName);
            tournament.Rounds.Should().BeEmpty();
            tournament.PlayerReferences.Should().BeEmpty();
            tournament.Betters.Should().BeEmpty();
            tournament.Settings.Should().BeEmpty();
            tournament.MiscBetCatalogue.Should().BeEmpty();
        }

        [Fact]
        public void CannotCreateTournamentWithEmptyName()
        {
            Tournament tournament = services.TournamentService.CreateTournament("");

            tournament.Should().BeNull();
        }

        [Fact]
        public void CannotCreateTournamentWithNameAlreadyInUseNoMatterLetterCasing()
        {
            Tournament secondTournament = services.TournamentService.CreateTournament(tournament.Name.ToUpper());

            secondTournament.Should().BeNull();
        }

        [Fact]
        public void CanRenameTournament()
        {
            bool result = services.TournamentService.RenameTournament(tournament.Id, "BHA Open 2019");

            result.Should().BeTrue();
            tournament.Name.Should().Be("BHA Open 2019");
        }

        [Fact]
        public void CannotRenameTournamentToEmptyName()
        {
            bool result = services.TournamentService.RenameTournament(tournament.Id, "");

            result.Should().BeFalse();
            tournament.Name.Should().Be("GSL 2019");
        }

        [Fact]
        public void CannotRenameTournamentToNameAlreadyInUseNoMatterLetterCasing()
        {
            Tournament secondTournament = services.TournamentService.CreateTournament("BHA Open 2019");

            bool result = services.TournamentService.RenameTournament(secondTournament.Id, tournament.Name.ToUpper());

            result.Should().BeFalse();
            secondTournament.Name.Should().Be("BHA Open 2019");
        }

        [Fact]
        public void CannotRenameNonexistingTournament()
        {
            bool result = services.TournamentService.RenameTournament(Guid.NewGuid(), "BHA Open 2019");

            result.Should().BeFalse();
        }

        [Fact]
        public void CanGetTournamentById()
        {
            Tournament fetchedTournament = services.TournamentService.GetTournamentById(tournament.Id);

            fetchedTournament.Should().NotBeNull();
            fetchedTournament.Name.Should().Be(tournament.Name);
        }

        [Fact]
        public void CanGetTournamentByName()
        {
            Tournament fetchedTournament = services.TournamentService.GetTournamentByName(tournament.Name);

            fetchedTournament.Should().NotBeNull();
            fetchedTournament.Name.Should().Be(tournament.Name);
        }

        [Fact]
        public void CanAddBetterToTournamentWithUserService()
        {
            InitializeUsersAndBetters();

            tournament.Betters.Should().NotBeEmpty();
        }

        [Fact]
        public void CanOnlyAddUserAsBetterOncePerTournament()
        {
            InitializeUsersAndBetters();

            Better better = tournament.AddBetter(tournament.Betters.First().User);

            better.Should().BeNull();
        }

        [Fact]
        public void CanGetAllBettersInTournamentByTournamentId()
        {
            InitializeUsersAndBetters();

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
            InitializeUsersAndBetters();

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
            InitializeUsersAndBetters();
            InitializeRoundGroupAndPlayers();

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
            InitializeUsersAndBetters();
            InitializeRoundGroupAndPlayers();

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
            InitializeUsersAndBetters();
            InitializeRoundGroupAndPlayers();

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

        private void InitializeUsersAndBetters()
        {
            services.UserService.CreateUser("Stålberto");
            services.UserService.CreateUser("Bönis");
            services.UserService.CreateUser("Guggelito");

            tournament.AddBetter(services.UserService.GetUserByName("Stålberto"));
            tournament.AddBetter(services.UserService.GetUserByName("Bönis"));
            tournament.AddBetter(services.UserService.GetUserByName("Guggelito"));
        }

        private void InitializeRoundGroupAndPlayers()
        {
            RoundBase round = tournament.AddRoundRobinRound("Round robin round", 3, 2);
            GroupBase group = round.AddGroup();

            group.AddPlayerReference("Maru");
            group.AddPlayerReference("Stork");
            group.AddPlayerReference("Taeja");
            group.AddPlayerReference("Rain");
            group.AddPlayerReference("Bomber");
            group.AddPlayerReference("FanTaSy");
            group.AddPlayerReference("Stephano");
            group.AddPlayerReference("Thorzain");
        }
    }
}
