using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Persistence;
using Slask.Persistence.Services;
using Slask.TestCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Xunit.IntegrationTests.PersistenceTests.ServiceTests
{
    public class RoundServiceTests
    {
        private readonly UserService _userService;
        private readonly TournamentService _tournamentService;
        private readonly RoundService _roundService;
        private readonly Tournament _tournament;


        public RoundServiceTests()
        {
            SlaskContext slaskContext = InMemoryContextCreator.Create();

            _userService = new UserService(slaskContext);
            _tournamentService = new TournamentService(slaskContext);
            _roundService = new RoundService(slaskContext);

            _tournament = _tournamentService.CreateTournament("GSL 2019");
            _tournamentService.Save(_tournament);
        }

        [Fact]
        public void CanRegisterPlayerReferencesToRound()
        {
            InitializeRoundGroupAndPlayers();
        }

        [Fact]
        public void PlayerReferencesAreAddedToTournamentWhenNewPlayersAreAddedTournament()
        {
            InitializeRoundGroupAndPlayers();

            List<PlayerReference> playerReferences = _tournament.GetPlayerReferences();

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

        private void InitializeRoundGroupAndPlayers()
        {
            List<string> playerNames = new List<string> { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };

            RoundRobinRound round = _tournamentService.AddRoundRobinRoundToTournament(_tournament);
            _tournamentService.Save(_tournament);

            round.SetPlayersPerGroupCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                _roundService.RegisterPlayerReferenceToRound(round, playerName);
            }

            _roundService.Save();
            _tournamentService.Save(_tournament);
        }
    }
}
