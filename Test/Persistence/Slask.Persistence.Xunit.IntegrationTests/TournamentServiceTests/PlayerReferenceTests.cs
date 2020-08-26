using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Persistence.Xunit.IntegrationTests.TournamentServiceTests
{
    public class PlayerReferenceTests : TournamentServiceTestBase
    {
        [Fact]
        public void CanGetEmptyPlayerReferencesListFromTournament()
        {
            InitializeUsersAndBetters();

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                tournamentService.AddRoundRobinRoundToTournament(tournament);

                List<PlayerReference> playerReferences = tournamentService.GetPlayerReferencesByTournamentId(tournament.Id);

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

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                bool removeResult = tournamentService.RemovePlayerReferenceFromTournament(tournament, playerNames[0]);
                removeResult.Should().BeTrue();

                tournamentService.RemovePlayerReferenceFromTournament(tournament, playerNames[1]);
                removeResult.Should().BeTrue();

                tournamentService.Save();

                playerNames.RemoveRange(0, 2);

                tournament.PlayerReferences.Should().HaveCount(playerNames.Count);
                foreach (string playerName in playerNames)
                {
                    tournament.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName);
                }
            }

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                tournament.PlayerReferences.Should().HaveCount(playerNames.Count);
                foreach (string playerName in playerNames)
                {
                    tournament.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName);
                }
            }
        }

        [Fact]
        public void CanRenamePlayerReferenceInTournament()
        {
            InitializeRoundGroupAndPlayers();

            string oldName = playerNames.First();
            string newName = oldName + "-san";

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                PlayerReference playerReference = tournament.GetPlayerReferenceByName(oldName);
                tournamentService.RenamePlayerReferenceInTournament(playerReference, newName);
                tournamentService.Save();
            }

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                PlayerReference playerReference = tournament.GetPlayerReferenceByName(newName);

                playerReference.Should().NotBeNull();
                playerReference.Name.Should().Be(newName);
            }
        }

        [Fact]
        public void CanGetAllPlayerReferencesInTournamentByTournamentId()
        {
            InitializeRoundGroupAndPlayers();

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

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
        }

        [Fact]
        public void CanGetAllPlayerReferencesInTournamentByTournamentName()
        {
            InitializeRoundGroupAndPlayers();

            using (TournamentService tournamentService = CreateTournamentService())
            {
                List<PlayerReference> playerReferences = tournamentService.GetPlayerReferencesByTournamentName(tournamentName);

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
