using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds.Bases;
using Slask.Persistence;
using Slask.Persistence.Services;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.ServiceTests
{
    public class TournamentServiceTests
    {
        private readonly UserService userService;
        private readonly TournamentService tournamentService;
        private readonly Tournament tournament;

        public TournamentServiceTests()
        {
            SlaskContext slaskContext = InMemoryContextCreator.Create();

            userService = new UserService(slaskContext);
            tournamentService = new TournamentService(slaskContext);
            tournament = tournamentService.CreateTournament("GSL 2019");
        }

        [Fact]
        public void CanCreateTournament()
        {
            string tournamentName = "Homestorycup XX";

            Tournament tournament = tournamentService.CreateTournament(tournamentName);

            tournament.Should().NotBeNull();
            tournament.Id.Should().NotBeEmpty();
            tournament.Name.Should().Be(tournamentName);
            tournament.Rounds.Should().BeEmpty();
            tournament.Betters.Should().BeEmpty();
            tournament.Settings.Should().BeEmpty();
            tournament.MiscBetCatalogue.Should().BeEmpty();
        }

        [Fact]
        public void CannotCreateTournamentWithEmptyName()
        {
            Tournament tournament = tournamentService.CreateTournament("");

            tournament.Should().BeNull();
        }

        [Fact]
        public void CannotCreateTournamentWithNameAlreadyInUseNoMatterLetterCasing()
        {
            Tournament secondTournament = tournamentService.CreateTournament(tournament.Name.ToUpper());

            secondTournament.Should().BeNull();
        }

        [Fact]
        public void CanRenameTournament()
        {
            bool result = tournamentService.RenameTournament(tournament.Id, "BHA Open 2019");

            result.Should().BeTrue();
            tournament.Name.Should().Be("BHA Open 2019");
        }

        [Fact]
        public void CannotRenameTournamentToEmptyName()
        {
            bool result = tournamentService.RenameTournament(tournament.Id, "");

            result.Should().BeFalse();
            tournament.Name.Should().Be("GSL 2019");
        }

        [Fact]
        public void CannotRenameTournamentToNameAlreadyInUseNoMatterLetterCasing()
        {
            Tournament secondTournament = tournamentService.CreateTournament("BHA Open 2019");

            bool result = tournamentService.RenameTournament(secondTournament.Id, tournament.Name.ToUpper());

            result.Should().BeFalse();
            secondTournament.Name.Should().Be("BHA Open 2019");
        }

        [Fact]
        public void CannotRenameNonexistingTournament()
        {
            bool result = tournamentService.RenameTournament(Guid.NewGuid(), "BHA Open 2019");

            result.Should().BeFalse();
        }

        [Fact]
        public void CanGetTournamentById()
        {
            Tournament fetchedTournament = tournamentService.GetTournamentById(tournament.Id);

            fetchedTournament.Should().NotBeNull();
            fetchedTournament.Name.Should().Be(tournament.Name);
        }

        [Fact]
        public void CanGetTournamentByName()
        {
            Tournament fetchedTournament = tournamentService.GetTournamentByName(tournament.Name);

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

            List<Better> betters = tournamentService.GetBettersByTournamentId(tournament.Id);

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

            List<Better> betters = tournamentService.GetBettersByTournamentName(tournament.Name);

            betters.Should().NotBeNullOrEmpty();
            betters.Should().HaveCount(3);
            betters[0].User.Name.Should().Be("Stålberto");
            betters[1].User.Name.Should().Be("Bönis");
            betters[2].User.Name.Should().Be("Guggelito");
        }

        [Fact]
        public void CanGetEmptyPlayerReferencesListInRoundWithoutGroup()
        {
            InitializeUsersAndBetters();

            RoundBase round = tournament.AddRoundRobinRound("Round robin round", 3, 2);

            List<PlayerReference> playerReferences = tournament.GetPlayerReferences();

            playerReferences.Should().HaveCount(0);
        }

        [Fact]
        public void PlayerReferencesAreAddedToTournamentWhenNewPlayersAreAddedTournament()
        {
            InitializeUsersAndBetters();
            InitializeRoundGroupAndPlayers();

            List<PlayerReference> playerReferences = tournament.GetPlayerReferences();

            playerReferences.Should().NotBeNull();
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
        public void CanGetAllPlayerReferencesInTournamentByTournamentId()
        {
            InitializeUsersAndBetters();
            InitializeRoundGroupAndPlayers();

            List<PlayerReference> playerReferences = tournamentService.GetPlayerReferencesByTournamentId(tournament.Id);

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

            List<PlayerReference> playerReferences = tournamentService.GetPlayerReferencesByTournamentName(tournament.Name);

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
            userService.CreateUser("Stålberto");
            userService.CreateUser("Bönis");
            userService.CreateUser("Guggelito");

            tournament.AddBetter(userService.GetUserByName("Stålberto"));
            tournament.AddBetter(userService.GetUserByName("Bönis"));
            tournament.AddBetter(userService.GetUserByName("Guggelito"));
        }

        private void InitializeRoundGroupAndPlayers()
        {
            RoundBase round = tournament.AddRoundRobinRound("Round robin round", 3, 2);

            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            round.RegisterPlayerReference("Taeja");
            round.RegisterPlayerReference("Rain");
            round.RegisterPlayerReference("Bomber");
            round.RegisterPlayerReference("FanTaSy");
            round.RegisterPlayerReference("Stephano");
            round.RegisterPlayerReference("Thorzain");
        }
    }
}
