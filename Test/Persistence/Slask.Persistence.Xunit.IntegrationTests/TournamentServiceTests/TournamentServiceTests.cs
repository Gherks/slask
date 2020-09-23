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
    public class tournamentRepositoryTest : tournamentRepositoryTestBase
    {
        [Fact]
        public void CanCreateTournament()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);

                tournament.Id.Should().NotBeEmpty();
                tournament.Name.Should().Be(tournamentName);
                tournament.Rounds.Should().BeEmpty();
                tournament.Betters.Should().BeEmpty();
            }
        }

        [Fact]
        public void CanRemoveTournamentById()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);

                bool removeResult = tournamentRepository.RemoveTournament(tournament.Id);
                tournamentRepository.Save();

                removeResult.Should().BeTrue();

                tournament = tournamentRepository.GetTournamentByName(tournamentName);
                tournament.Should().BeNull();
            }
        }

        [Fact]
        public void CanRemoveTournamentByName()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);

                bool removeResult = tournamentRepository.RemoveTournament(tournament.Name);
                tournamentRepository.Save();
                removeResult.Should().BeTrue();

                tournament = tournamentRepository.GetTournamentByName(tournamentName);
                tournament.Should().BeNull();
            }
        }

        [Fact]
        public void CannotCreateTournamentWithNameAlreadyInUseNoMatterLetterCasing()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament secondTournament = tournamentRepository.CreateTournament(tournamentName.ToUpper());
                secondTournament.Should().BeNull();
            }
        }

        [Fact]
        public void CanRenameTournament()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);

                bool renameResult = tournamentRepository.RenameTournament(tournament.Id, "BHA Open 2019");
                tournamentRepository.Save();

                renameResult.Should().BeTrue();
                tournament.Name.Should().Be("BHA Open 2019");
            }
        }

        [Fact]
        public void CannotRenameTournamentToEmptyName()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);

                bool renameResult = tournamentRepository.RenameTournament(tournament.Id, "");
                tournamentRepository.Save();

                renameResult.Should().BeFalse();
                tournament.Name.Should().Be("GSL 2019");
            }
        }

        [Fact]
        public void CannotRenameTournamentToNameAlreadyInUseNoMatterLetterCasing()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                bool renameResult = tournamentRepository.RenameTournament(tournament.Id, tournamentName.ToUpper());
                tournamentRepository.Save();

                renameResult.Should().BeFalse();
                tournament.Name.Should().Be(tournamentName);
            }
        }

        [Fact]
        public void CannotRenameNonexistingTournament()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                bool renameResult = tournamentRepository.RenameTournament(Guid.NewGuid(), "BHA Open 2019");

                renameResult.Should().BeFalse();
            }
        }

        [Fact]
        public void CanGetListOfSeveralTournaments()
        {
            List<string> tournamentNames = new List<string>() { tournamentName, "WCS 2019", "BHA Cup", "DH Masters" };

            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
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
            List<string> tournamentNames = new List<string>() { tournamentName };
            for (int index = 1; index < 30; ++index)
            {
                tournamentNames.Add("Tourney " + index.ToString());
            }

            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
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
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);

                tournament = tournamentRepository.GetTournamentById(tournament.Id);

                tournament.Name.Should().Be(tournamentName);
            }
        }

        [Fact]
        public void CanGetTournamentByName()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);

                tournament.Name.Should().Be(tournamentName);
            }
        }
    }
}
