using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Repositories;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Persistence.Xunit.IntegrationTests.TournamentRepositoryTests
{
    public class PlayerReferenceTests : TournamentRepositoryTestBase
    {
        [Fact]
        public void CanGetEmptyPlayerReferencesListFromTournament()
        {
            InitializeUsersAndBetters();

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                tournamentRepository.AddRoundRobinRoundToTournament(tournament);

                List<PlayerReference> playerReferences = tournamentRepository.GetPlayerReferencesInTournament(tournament.Id).ToList();

                playerReferences.Should().BeEmpty();
            }
        }

        [Fact]
        public void CanAddPlayerReferencesToTournament()
        {
            InitializeRoundGroupAndPlayers();
        }

        [Fact]
        public void CanRemovePlayerReferenceFromTournament()
        {
            InitializeRoundGroupAndPlayers();

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                bool removeResult = tournamentRepository.RemovePlayerReferenceFromTournament(tournament, _playerNames[0]);
                removeResult.Should().BeTrue();

                tournamentRepository.RemovePlayerReferenceFromTournament(tournament, _playerNames[1]);
                removeResult.Should().BeTrue();

                tournamentRepository.Save();

                _playerNames.RemoveRange(0, 2);

                tournament.PlayerReferences.Should().HaveCount(_playerNames.Count);
                foreach (string playerName in _playerNames)
                {
                    tournament.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName);
                }
            }

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                tournament.PlayerReferences.Should().HaveCount(_playerNames.Count);
                foreach (string playerName in _playerNames)
                {
                    tournament.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName);
                }
            }
        }

        [Fact]
        public void CanRenamePlayerReferenceInTournament()
        {
            InitializeRoundGroupAndPlayers();

            string oldName = _playerNames.First();
            string newName = oldName + "-san";

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                PlayerReference playerReference = tournament.GetPlayerReference(oldName);
                tournamentRepository.RenamePlayerReferenceInTournament(playerReference, newName);
                tournamentRepository.Save();
            }

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                PlayerReference playerReference = tournament.GetPlayerReference(newName);

                playerReference.Should().NotBeNull();
                playerReference.Name.Should().Be(newName);
            }
        }

        [Fact]
        public void CanGetAllPlayerReferencesInTournamentByTournamentId()
        {
            InitializeRoundGroupAndPlayers();

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                List<PlayerReference> playerReferences = tournamentRepository.GetPlayerReferencesInTournament(tournament.Id).ToList();

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
        }

        [Fact]
        public void CanGetAllPlayerReferencesInTournamentByTournamentName()
        {
            InitializeRoundGroupAndPlayers();

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                List<PlayerReference> playerReferences = tournamentRepository.GetPlayerReferencesInTournament(_tournamentName).ToList();

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
        }
    }
}
