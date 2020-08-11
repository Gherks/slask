using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds;
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
            _tournamentService.Save(_tournament);
        }

        [Fact]
        public void CanCreateTournament()
        {
            string tournamentName = "Homestorycup XX";

            Tournament tournament = _tournamentService.CreateTournament(tournamentName);
            _tournamentService.Save(tournament);

            tournament.Should().NotBeNull();
            tournament.Id.Should().NotBeEmpty();
            tournament.Name.Should().Be(tournamentName);
            tournament.Rounds.Should().BeEmpty();
            tournament.Betters.Should().BeEmpty();
        }

        [Fact]
        public void CannotSaveInvalidTournament()
        {
            Tournament tournament = _tournamentService.CreateTournament("");
            _tournamentService.Save(tournament);

            tournament.Should().BeNull();
        }

        [Fact]
        public void CannotCreateTournamentWithNameAlreadyInUseNoMatterLetterCasing()
        {
            Tournament secondTournament = _tournamentService.CreateTournament(_tournament.Name.ToUpper());
            _tournamentService.Save(_tournament);

            secondTournament.Should().BeNull();
        }

        [Fact]
        public void CanSaveTournament()
        {
            InitializeUsersAndBetters();

            bool saveResult = _tournamentService.Save(_tournament);
            saveResult.Should().BeTrue();
        }

        [Fact]
        public void CannotSaveTournamentWithIssues()
        {
            _tournament.AddDualTournamentRound();
            bool saveResult = _tournamentService.Save(_tournament);

            saveResult.Should().BeFalse();
        }

        [Fact]
        public void CanRenameTournament()
        {
            bool result = _tournamentService.RenameTournament(_tournament.Id, "BHA Open 2019");
            _tournamentService.Save(_tournament);

            result.Should().BeTrue();
            _tournament.Name.Should().Be("BHA Open 2019");
        }

        [Fact]
        public void CannotRenameTournamentToEmptyName()
        {
            bool result = _tournamentService.RenameTournament(_tournament.Id, "");
            _tournamentService.Save(_tournament);

            result.Should().BeFalse();
            _tournament.Name.Should().Be("GSL 2019");
        }

        [Fact]
        public void CannotRenameTournamentToNameAlreadyInUseNoMatterLetterCasing()
        {
            string tournamentName = "bha open 2019";

            Tournament tournament = _tournamentService.CreateTournament(tournamentName);

            bool result = _tournamentService.RenameTournament(tournament.Id, tournamentName.ToUpper());

            result.Should().BeFalse();
            tournament.Name.Should().Be(tournamentName);
        }

        [Fact]
        public void CannotRenameNonexistingTournament()
        {
            bool result = _tournamentService.RenameTournament(Guid.NewGuid(), "BHA Open 2019");

            result.Should().BeFalse();
        }

        [Fact]
        public void CanGetTournamentById()
        {
            Tournament fetchedTournament = _tournamentService.GetTournamentById(_tournament.Id);

            fetchedTournament.Should().NotBeNull();
            fetchedTournament.Name.Should().Be(_tournament.Name);
        }

        [Fact]
        public void CanGetTournamentByName()
        {
            Tournament fetchedTournament = _tournamentService.GetTournamentByName(_tournament.Name);

            fetchedTournament.Should().NotBeNull();
            fetchedTournament.Name.Should().Be(_tournament.Name);
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

            Better better = _tournament.AddBetter(_tournament.Betters.First().User);

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

            RoundBase round = _tournament.AddRoundRobinRound();

            List<PlayerReference> playerReferences = _tournament.GetPlayerReferences();

            playerReferences.Should().BeEmpty();
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
            _tournamentService.Save(_tournament);
        }

        private void InitializeRoundGroupAndPlayers()
        {
            List<string> playerNames = new List<string> { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };

            RoundRobinRound round = _tournamentService.AddRoundRobinRoundToTournament(_tournament);
            _tournamentService.Save(_tournament); // TEST
            round.SetPlayersPerGroupCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                round.RegisterPlayerReference(playerName);
            }

            _tournamentService.Save(_tournament);
        }
    }
}
