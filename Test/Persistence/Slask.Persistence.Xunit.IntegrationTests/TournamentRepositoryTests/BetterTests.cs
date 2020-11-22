using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Persistence.Xunit.IntegrationTests.TournamentRepositoryTests
{
    public class BetterTests : TournamentRepositoryTestBase
    {
        [Fact]
        public void CanAddBettersToTournament()
        {
            InitializeUsersAndBetters();
        }

        [Fact]
        public void CanOnlyAddUserAsBetterOncePerTournament()
        {
            InitializeUsersAndBetters();

            using (UserRepository userRepository = CreateuserRepository())
            {
                using (TournamentRepository tournamentRepository = CreateTournamentRepository())
                {
                    Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                    Better better = tournamentRepository.AddBetterToTournament(tournament, userRepository.GetUser("Stålberto"));
                    better.Should().BeNull();
                }
            }
        }

        [Fact]
        public void CanGetAllBettersInTournamentByTournamentId()
        {
            InitializeUsersAndBetters();

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                List<Better> betters = tournamentRepository.GetBettersByTournamentId(tournament.Id).ToList();

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

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                List<Better> betters = tournamentRepository.GetBettersByTournamentName(_tournamentName).ToList();

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

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                tournament.Betters.Should().HaveCount(3);

                List<Better> betters = tournamentRepository.GetBettersByTournamentName(_tournamentName).ToList();
                bool removalResult = tournamentRepository.RemoveBetterFromTournamentById(tournament, betters.First().Id);
                tournamentRepository.Save();

                removalResult.Should().BeTrue();
                tournament.Betters.Should().HaveCount(2);
            }
        }

        [Fact]
        public void CanRemoveBetterFromTournamentByName()
        {
            InitializeUsersAndBetters();

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                tournament.Betters.Should().HaveCount(3);

                bool removalResult = tournamentRepository.RemoveBetterFromTournamentByName(tournament, "Stålberto");
                tournamentRepository.Save();

                removalResult.Should().BeTrue();
                tournament.Betters.Should().HaveCount(2);
            }
        }

        //////////////[Fact]
        //////////////public void CannotRemoveBetterFromTournamentThatHasStarted()
        //////////////{
        //////////////    throw new NotImplementedException();
        //////////////}

        [Fact]
        public void CannotRemoveNonexistingBetterFromTournamentById()
        {
            InitializeUsersAndBetters();

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                tournament.Betters.Should().HaveCount(3);

                bool removalResult = tournamentRepository.RemoveBetterFromTournamentById(tournament, Guid.NewGuid());
                tournamentRepository.Save();

                removalResult.Should().BeFalse();
                tournament.Betters.Should().HaveCount(3);
            }
        }

        [Fact]
        public void CannotRemoveNonexistingBetterFromTournamentByName()
        {
            InitializeUsersAndBetters();

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(_tournamentName);

                tournament.Betters.Should().HaveCount(3);

                bool removalResult = tournamentRepository.RemoveBetterFromTournamentByName(tournament, "Kimmieboi");
                tournamentRepository.Save();

                removalResult.Should().BeFalse();
                tournament.Betters.Should().HaveCount(3);
            }
        }
    }
}
