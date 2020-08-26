using FluentAssertions;
using Slask.Domain;
using Slask.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Persistence.Xunit.IntegrationTests.TournamentServiceTests
{
    public class BetterTests : TournamentServiceTestBase
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

        //////////////[Fact]
        //////////////public void CannotRemoveBetterFromTournamentThatHasStarted()
        //////////////{
        //////////////    throw new NotImplementedException();
        //////////////}

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
    }
}
