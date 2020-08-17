using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Persistence.Services;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Slask.Xunit.IntegrationTests.PersistenceTests.ServiceTests
{
    public class TournamentServiceTests
    {
        private const string tournamentName = "GSL 2019";
        private readonly string testDatabaseName;

        public TournamentServiceTests()
        {
            testDatabaseName = Guid.NewGuid().ToString();

            using (TournamentService tournamentService = CreateTournamentService())
            {
                tournamentService.CreateTournament(tournamentName);
                tournamentService.Save();
            }
        }

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
        public void CannotCreateTournamentWithNameAlreadyInUseNoMatterLetterCasing()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament secondTournament = tournamentService.CreateTournament(tournamentName.ToUpper());
                secondTournament.Should().BeNull();
            }
        }

        [Fact]
        public void CanAddBettersToTournament()
        {
            InitializeUsersAndBetters();
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
        public void CanOnlyAddUserAsBetterOncePerTournament()
        {
            InitializeUsersAndBetters();

            using (UserService userService = CreateUserService())
            {
                using (TournamentService tournamentService = CreateTournamentService())
                {
                    Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                    Better better = tournamentService.AddBetterToTournament(tournament, userService.GetUserByName("Stålberto"));
                    better.Should().BeNull();
                }
            }
        }

        [Fact]
        public void CanGetAllBettersInTournamentByTournamentId()
        {
            InitializeUsersAndBetters();

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                List<Better> betters = tournamentService.GetBettersByTournamentId(tournament.Id);

                betters.Should().NotBeNullOrEmpty();
                betters.Should().HaveCount(3);
                betters[0].User.Name.Should().Be("Stålberto");
                betters[1].User.Name.Should().Be("Bönis");
                betters[2].User.Name.Should().Be("Guggelito");
            }
        }

        [Fact]
        public void CanGetAllBettersInTournamentByTournamentName()
        {
            InitializeUsersAndBetters();

            using (TournamentService tournamentService = CreateTournamentService())
            {
                List<Better> betters = tournamentService.GetBettersByTournamentName(tournamentName);

                betters.Should().NotBeNullOrEmpty();
                betters.Should().HaveCount(3);
                betters[0].User.Name.Should().Be("Stålberto");
                betters[1].User.Name.Should().Be("Bönis");
                betters[2].User.Name.Should().Be("Guggelito");
            }
        }

        [Fact]
        public void CanRemoveBetterFromTournamentById()
        {
            InitializeUsersAndBetters();

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                tournament.Betters.Should().HaveCount(3);

                List<Better> betters = tournamentService.GetBettersByTournamentName(tournamentName);
                bool removalResult = tournamentService.RemoveBetterFromTournamentById(tournament, betters.First().Id);
                tournamentService.Save();

                removalResult.Should().BeTrue();
                tournament.Betters.Should().HaveCount(2);
            }
        }

        [Fact]
        public void CanRemoveBetterFromTournamentByName()
        {
            InitializeUsersAndBetters();

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                tournament.Betters.Should().HaveCount(3);

                bool removalResult = tournamentService.RemoveBetterFromTournamentByName(tournament, "Stålberto");
                tournamentService.Save();

                removalResult.Should().BeTrue();
                tournament.Betters.Should().HaveCount(2);
            }
        }

        [Fact]
        public void CannotRemoveNonexistingBetterFromTournamentById()
        {
            InitializeUsersAndBetters();

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                tournament.Betters.Should().HaveCount(3);

                bool removalResult = tournamentService.RemoveBetterFromTournamentById(tournament, Guid.NewGuid());
                tournamentService.Save();

                removalResult.Should().BeFalse();
                tournament.Betters.Should().HaveCount(3);
            }
        }

        [Fact]
        public void CannotRemoveNonexistingBetterFromTournamentByName()
        {
            InitializeUsersAndBetters();

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                tournament.Betters.Should().HaveCount(3);

                bool removalResult = tournamentService.RemoveBetterFromTournamentByName(tournament, "Kimmieboi");
                tournamentService.Save();

                removalResult.Should().BeFalse();
                tournament.Betters.Should().HaveCount(3);
            }
        }

        [Fact]
        public void CanGetEmptyPlayerReferencesListInRoundWithoutGroup()
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
        public void CanRegisterPlayerReferencesToRound()
        {
            InitializeRoundGroupAndPlayers();
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

        private UserService CreateUserService()
        {
            return new UserService(InMemoryContextCreator.Create(testDatabaseName));
        }

        private TournamentService CreateTournamentService()
        {
            return new TournamentService(InMemoryContextCreator.Create(testDatabaseName));
        }

        private void InitializeUsersAndBetters()
        {
            using (UserService userService = CreateUserService())
            {
                userService.CreateUser("Stålberto");
                userService.CreateUser("Bönis");
                userService.CreateUser("Guggelito");
                userService.Save();

                using (TournamentService tournamentService = CreateTournamentService())
                {
                    Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                    tournamentService.AddBetterToTournament(tournament, userService.GetUserByName("Stålberto"));
                    tournamentService.AddBetterToTournament(tournament, userService.GetUserByName("Bönis"));
                    tournamentService.AddBetterToTournament(tournament, userService.GetUserByName("Guggelito"));
                    tournamentService.Save();
                }
            }
        }

        private void InitializeRoundGroupAndPlayers()
        {
            List<string> playerNames = new List<string> { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                RoundRobinRound round = tournamentService.AddRoundRobinRoundToTournament(tournament);
                tournamentService.SetPlayersPerGroupCountInRound(round, playerNames.Count);

                foreach (string playerName in playerNames)
                {
                    tournamentService.AddPlayerReference(tournament, playerName);
                }

                tournamentService.Save();
            }
        }
    }
}
