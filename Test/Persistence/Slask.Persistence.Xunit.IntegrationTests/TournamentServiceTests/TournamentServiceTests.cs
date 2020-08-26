using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Slask.Persistence.Xunit.IntegrationTests.TournamentServiceTests
{
    public class TournamentServiceTest : TournamentServiceTestBase
    {
        [Fact]
        public void CanCreateTournament()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                tournament.Id.Should().NotBeEmpty();
                tournament.Name.Should().Be(tournamentName);
                tournament.Rounds.Should().BeEmpty();
                tournament.Betters.Should().BeEmpty();
            }
        }

        [Fact]
        public void CanRemoveTournamentById()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                bool removeResult = tournamentService.RemoveTournament(tournament.Id);
                tournamentService.Save();

                removeResult.Should().BeTrue();

                tournament = tournamentService.GetTournamentByName(tournamentName);
                tournament.Should().BeNull();
            }
        }

        [Fact]
        public void CanRemoveTournamentByName()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                bool removeResult = tournamentService.RemoveTournament(tournament.Name);
                tournamentService.Save();
                removeResult.Should().BeTrue();

                tournament = tournamentService.GetTournamentByName(tournamentName);
                tournament.Should().BeNull();
            }
        }

        [Fact]
        public void CannotCreateTournamentWithNameAlreadyInUseNoMatterLetterCasing()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament secondTournament = tournamentService.CreateTournament(tournamentName.ToUpper());
                secondTournament.Should().BeNull();
            }
        }

        [Fact]
        public void CanRenameTournament()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                bool renameResult = tournamentService.RenameTournament(tournament.Id, "BHA Open 2019");
                tournamentService.Save();

                renameResult.Should().BeTrue();
                tournament.Name.Should().Be("BHA Open 2019");
            }
        }

        [Fact]
        public void CannotRenameTournamentToEmptyName()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                bool renameResult = tournamentService.RenameTournament(tournament.Id, "");
                tournamentService.Save();

                renameResult.Should().BeFalse();
                tournament.Name.Should().Be("GSL 2019");
            }
        }

        [Fact]
        public void CannotRenameTournamentToNameAlreadyInUseNoMatterLetterCasing()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                bool renameResult = tournamentService.RenameTournament(tournament.Id, tournamentName.ToUpper());
                tournamentService.Save();

                renameResult.Should().BeFalse();
                tournament.Name.Should().Be(tournamentName);
            }
        }

        [Fact]
        public void CannotRenameNonexistingTournament()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                bool renameResult = tournamentService.RenameTournament(Guid.NewGuid(), "BHA Open 2019");

                renameResult.Should().BeFalse();
            }
        }

        [Fact]
        public void CanGetListOfSeveralTournaments()
        {
            List<string> tournamentNames = new List<string>() { tournamentName, "WCS 2019", "BHA Cup", "DH Masters" };

            using (TournamentService tournamentService = CreateTournamentService())
            {
                foreach (string tournamentName in tournamentNames)
                {
                    tournamentService.CreateTournament(tournamentName);
                }
                tournamentService.Save();

                IEnumerable<Tournament> tournaments = tournamentService.GetTournaments();

                tournaments.Should().HaveCount(tournamentNames.Count);
                foreach (string tournamentName in tournamentNames)
                {
                    tournaments.FirstOrDefault(tournament => tournament.Name == tournamentName).Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void CanGetCertainRangeOfCreatedTournaments()
        {
            List<string> tournamentNames = new List<string>() { tournamentName };
            for (int index = 1; index < 30; ++index)
            {
                tournamentNames.Add("Tourney " + index.ToString());
            }

            using (TournamentService tournamentService = CreateTournamentService())
            {
                foreach (string tournamentName in tournamentNames)
                {
                    tournamentService.CreateTournament(tournamentName);

                    // Wait a bit between creation just to make sure the created date has some space between each tournament
                    Thread.Sleep(100);
                }
                tournamentService.Save();

                int startIndex = 15;
                int grabCount = 6;

                List<Tournament> tournaments = tournamentService.GetTournaments(startIndex, grabCount).ToList();

                tournaments.Should().HaveCount(grabCount);
                for (int index = 0; index < tournaments.Count; ++index)
                {
                    string expectedName = "Tourney " + (startIndex + index).ToString();
                    tournaments[index].Name.Should().Be(expectedName);
                }
            }
        }

        [Fact]
        public void CanGetTournamentById()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                tournament = tournamentService.GetTournamentById(tournament.Id);

                tournament.Name.Should().Be(tournamentName);
            }
        }

        [Fact]
        public void CanGetTournamentByName()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                tournament.Name.Should().Be(tournamentName);
            }
        }

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
        public void CanRegisterPlayerReferencesToTournament()
        {
            InitializeRoundGroupAndPlayers();
        }

        [Fact]
        public void CanExcludePlayerReferenceFromTournament()
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

        //BetterPlacesMatchBetOnMatch
        //AddScoreToPlayerInMatch
    }
}
