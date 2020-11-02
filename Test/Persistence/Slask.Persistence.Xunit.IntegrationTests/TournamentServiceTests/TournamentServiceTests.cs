using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Slask.Persistence.Xunit.IntegrationTests.tournamentRepositoryTests
{
    public class TournamentRepositoryTest : TournamentRepositoryTestBase
    {
        [Fact]
        public void CanCreateTournament()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                tournament.Id.Should().NotBeEmpty();
                tournament.Name.Should().Be(_tournamentName);
                tournament.Rounds.Should().BeEmpty();
                tournament.Betters.Should().BeEmpty();
            }
        }

        [Fact]
        public void CanDetermineThatTournamentExistById()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                tournamentRepository.TournamentExist(tournament.Id).Should().BeTrue();
            }
        }

        [Fact]
        public void CanDetermineThatTournamentExistByNameNoMatterLetterCasing()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                tournamentRepository.TournamentExist(_tournamentName.ToUpper()).Should().BeTrue();
            }
        }

        [Fact]
        public void CanDetermineThatTournamentDoesNotExistById()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                tournamentRepository.TournamentExist(Guid.NewGuid()).Should().BeFalse();
            }
        }

        [Fact]
        public void CanDetermineThatTournamentDoesNotExistByName()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                tournamentRepository.TournamentExist("non-existing-tournament").Should().BeFalse();
            }
        }

        [Fact]
        public void CanRemoveTournamentById()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                bool removeResult = tournamentRepository.RemoveTournament(tournament.Id);
                tournamentRepository.Save();

                removeResult.Should().BeTrue();

                tournament = tournamentRepository.GetTournament(_tournamentName);
                tournament.Should().BeNull();
            }
        }

        [Fact]
        public void CanRemoveTournamentByName()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                bool removeResult = tournamentRepository.RemoveTournament(tournament.Name);
                tournamentRepository.Save();
                removeResult.Should().BeTrue();

                tournament = tournamentRepository.GetTournament(_tournamentName);
                tournament.Should().BeNull();
            }
        }

        [Fact]
        public void CannotCreateTournamentWithNameAlreadyInUseNoMatterLetterCasing()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament secondTournament = tournamentRepository.CreateTournament(_tournamentName.ToUpper());
                secondTournament.Should().BeNull();
            }
        }

        [Fact]
        public void CanRenameTournamentById()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                bool renameResult = tournamentRepository.RenameTournament(tournament.Id, "BHA Open 2019");
                tournamentRepository.Save();

                renameResult.Should().BeTrue();
                tournament.Name.Should().Be("BHA Open 2019");
            }
        }

        [Fact]
        public void CanRenameTournamentByName()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                bool renameResult = tournamentRepository.RenameTournament(tournament.Name, "BHA Open 2019");
                tournamentRepository.Save();

                renameResult.Should().BeTrue();
                tournament.Name.Should().Be("BHA Open 2019");
            }
        }

        [Fact]
        public void CannotRenameTournamentByIdToEmptyName()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                bool renameResult = tournamentRepository.RenameTournament(tournament.Id, "");
                tournamentRepository.Save();

                renameResult.Should().BeFalse();
                tournament.Name.Should().Be("GSL 2019");
            }
        }

        [Fact]
        public void CannotRenameTournamentByNameToEmptyName()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                bool renameResult = tournamentRepository.RenameTournament(tournament.Name, "");
                tournamentRepository.Save();

                renameResult.Should().BeFalse();
                tournament.Name.Should().Be("GSL 2019");
            }
        }

        [Fact]
        public void CannotRenameTournamentByIdToNameAlreadyInUseNoMatterLetterCasing()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                string occupiedName = "Homestory Cup XX";
                tournamentRepository.CreateTournament(occupiedName);
                tournamentRepository.Save();

                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);
                bool renameResult = tournamentRepository.RenameTournament(tournament.Id, occupiedName.ToUpper());
                tournamentRepository.Save();

                renameResult.Should().BeFalse();
                tournament.Name.Should().Be(_tournamentName);
            }
        }

        [Fact]
        public void CannotRenameTournamentByNameToNameAlreadyInUseNoMatterLetterCasing()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                string occupiedName = "Homestory Cup XX";
                tournamentRepository.CreateTournament(occupiedName);
                tournamentRepository.Save();

                bool renameResult = tournamentRepository.RenameTournament(_tournamentName, occupiedName.ToUpper());
                tournamentRepository.Save();

                renameResult.Should().BeFalse();
            }
        }

        [Fact]
        public void CannotRenameNonexistingTournamentById()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                bool renameResult = tournamentRepository.RenameTournament(Guid.NewGuid(), "BHA Open 2019");

                renameResult.Should().BeFalse();
            }
        }

        [Fact]
        public void CannotRenameNonexistingTournamentByName()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                bool renameResult = tournamentRepository.RenameTournament(Guid.NewGuid().ToString(), "BHA Open 2019");

                renameResult.Should().BeFalse();
            }
        }

        [Fact]
        public void CanGetListOfSeveralTournaments()
        {
            List<string> tournamentNames = new List<string>() { _tournamentName, "WCS 2019", "BHA Cup", "DH Masters" };

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                foreach (string tournamentName in tournamentNames)
                {
                    tournamentRepository.CreateTournament(tournamentName);
                }
                tournamentRepository.Save();

                IEnumerable<Tournament> tournaments = tournamentRepository.GetTournaments();

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
            List<string> tournamentNames = new List<string>() { _tournamentName };
            for (int index = 1; index < 30; ++index)
            {
                tournamentNames.Add("Tourney " + index.ToString());
            }

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                foreach (string tournamentName in tournamentNames)
                {
                    tournamentRepository.CreateTournament(tournamentName);

                    // Wait a bit between creation just to make sure the created date has some space between each tournament
                    Thread.Sleep(100);
                }
                tournamentRepository.Save();

                int startIndex = 15;
                int grabCount = 6;

                List<Tournament> tournaments = tournamentRepository.GetTournaments(startIndex, grabCount).ToList();

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
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                tournament = tournamentRepository.GetTournament(tournament.Id);

                tournament.Name.Should().Be(_tournamentName);
            }
        }

        [Fact]
        public void CanGetTournamentByName()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                tournament.Name.Should().Be(_tournamentName);
            }
        }
    }
}
