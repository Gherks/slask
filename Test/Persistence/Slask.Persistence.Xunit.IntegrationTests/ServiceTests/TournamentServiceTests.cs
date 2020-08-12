using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Persistence;
using Slask.Persistence.Services;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Xunit.IntegrationTests.PersistenceTests.ServiceTests
{
    public class TournamentServiceTests
    {
        private readonly UserService _userService;
        private readonly TournamentService _tournamentService;
        private readonly Tournament _tournament;


        // GetTournaments() returns all tournaments containing only tournament data + start date

        public TournamentServiceTests()
        {
            SlaskContext slaskContext = InMemoryContextCreator.Create();

            _userService = new UserService(slaskContext);
            _tournamentService = new TournamentService(slaskContext);
            _tournament = _tournamentService.CreateTournament("GSL 2019");
            _tournamentService.Save();
        }

        [Fact]
        public void CanCreateTournament()
        {
            string tournamentName = "Homestorycup XX";

            Tournament tournament = _tournamentService.CreateTournament(tournamentName);
            _tournamentService.Save();

            tournament.Id.Should().NotBeEmpty();
            tournament.Name.Should().Be(tournamentName);
            tournament.Rounds.Should().BeEmpty();
            tournament.Betters.Should().BeEmpty();
        }

        [Fact]
        public void CannotCreateTournamentWithNameAlreadyInUseNoMatterLetterCasing()
        {
            Tournament secondTournament = _tournamentService.CreateTournament(_tournament.Name.ToUpper());
            secondTournament.Should().BeNull();
        }

        [Fact]
        public void CanSaveTournament()
        {
            InitializeUsersAndBetters();
            _tournamentService.Save();
        }

        [Fact]
        public void CanRenameTournament()
        {
            bool renameResult = _tournamentService.RenameTournament(_tournament.Id, "BHA Open 2019");
            _tournamentService.Save();

            renameResult.Should().BeTrue();
            _tournament.Name.Should().Be("BHA Open 2019");
        }

        [Fact]
        public void CannotRenameTournamentToEmptyName()
        {
            bool renameResult = _tournamentService.RenameTournament(_tournament.Id, "");
            _tournamentService.Save();

            renameResult.Should().BeFalse();
            _tournament.Name.Should().Be("GSL 2019");
        }

        [Fact]
        public void CannotRenameTournamentToNameAlreadyInUseNoMatterLetterCasing()
        {
            string tournamentName = "bha open 2019";

            Tournament tournament = _tournamentService.CreateTournament(tournamentName);
            _tournamentService.Save();

            bool renameResult = _tournamentService.RenameTournament(tournament.Id, tournamentName.ToUpper());
            _tournamentService.Save();

            renameResult.Should().BeFalse();
            tournament.Name.Should().Be(tournamentName);
        }

        [Fact]
        public void CannotRenameNonexistingTournament()
        {
            bool renameResult = _tournamentService.RenameTournament(Guid.NewGuid(), "BHA Open 2019");

            renameResult.Should().BeFalse();
        }

        [Fact]
        public void CanGetTournamentById()
        {
            Tournament tournament = _tournamentService.GetTournamentById(_tournament.Id);

            tournament.Name.Should().Be(_tournament.Name);
        }

        [Fact]
        public void CanGetTournamentByName()
        {
            Tournament tournament = _tournamentService.GetTournamentByName(_tournament.Name);

            tournament.Name.Should().Be(_tournament.Name);
        }

        [Fact]
        public void CanAddBetterToTournamentWithUserService()
        {
            InitializeUsersAndBetters();

            _tournament.Betters.Should().NotBeEmpty();
        }

        [Fact]
        public void CanOnlyAddUserAsBetterOncePerTournament()
        {
            InitializeUsersAndBetters();

            Better better = _tournamentService.AddBetterToTournament(_tournament, _userService.GetUserByName("Stålberto"));

            better.Should().BeNull();
        }

        [Fact]
        public void CanGetAllBettersInTournamentByTournamentId()
        {
            InitializeUsersAndBetters();

            List<Better> betters = _tournamentService.GetBettersByTournamentId(_tournament.Id);

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

            List<Better> betters = _tournamentService.GetBettersByTournamentName(_tournament.Name);

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

            _tournamentService.AddRoundRobinRound(_tournament);

            List<PlayerReference> playerReferences = _tournamentService.GetPlayerReferencesByTournamentId(_tournament.Id);

            playerReferences.Should().BeEmpty();
        }

        [Fact]
        public void CanRegisterPlayerReferencesToRound()
        {
            InitializeRoundGroupAndPlayers();
        }

        //[Fact]
        //public void PlayerReferencesAreAddedToTournamentWhenNewPlayersAreAddedTournament()
        //{
        //    InitializeRoundGroupAndPlayers();

        //    List<PlayerReference> playerReferences = _tournament.GetPlayerReferences();

        //    playerReferences.Should().NotBeNull();
        //    playerReferences.Should().HaveCount(8);

        //    playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Maru").Should().NotBeNull();
        //    playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Stork").Should().NotBeNull();
        //    playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Taeja").Should().NotBeNull();
        //    playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Rain").Should().NotBeNull();
        //    playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Bomber").Should().NotBeNull();
        //    playerReferences.FirstOrDefault(playerReference => playerReference.Name == "FanTaSy").Should().NotBeNull();
        //    playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Stephano").Should().NotBeNull();
        //    playerReferences.FirstOrDefault(playerReference => playerReference.Name == "Thorzain").Should().NotBeNull();
        //}

        [Fact]
        public void CanGetAllPlayerReferencesInTournamentByTournamentId()
        {
            InitializeRoundGroupAndPlayers();

            List<PlayerReference> playerReferences = _tournamentService.GetPlayerReferencesByTournamentId(_tournament.Id);

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
            InitializeRoundGroupAndPlayers();

            List<PlayerReference> playerReferences = _tournamentService.GetPlayerReferencesByTournamentName(_tournament.Name);

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
            _userService.CreateUser("Stålberto");
            _userService.CreateUser("Bönis");
            _userService.CreateUser("Guggelito");
            _userService.Save();

            _tournamentService.AddBetterToTournament(_tournament, _userService.GetUserByName("Stålberto"));
            _tournamentService.AddBetterToTournament(_tournament, _userService.GetUserByName("Bönis"));
            _tournamentService.AddBetterToTournament(_tournament, _userService.GetUserByName("Guggelito"));
            _tournamentService.Save();
        }

        private void InitializeRoundGroupAndPlayers()
        {
            List<string> playerNames = new List<string> { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };

            RoundRobinRound round = _tournamentService.AddRoundRobinRound(_tournament);
            _tournamentService.SetPlayersPerGroupCountInRound(round, playerNames.Count);

            foreach (string playerName in playerNames)
            {
                _tournamentService.RegisterPlayerReference(_tournament, playerName);
            }

            _tournamentService.Save();
        }
    }
}
