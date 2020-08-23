using Slask.Domain;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Persistence.Services;
using Slask.TestCore;
using System;
using System.Collections.Generic;

namespace Slask.Persistence.Xunit.IntegrationTests.TournamentServiceTests
{
    public class TournamentServiceTestBase
    {
        protected const string tournamentName = "GSL 2019";
        protected readonly List<string> playerNames = new List<string> 
        { 
            "Maru",
            "Stork",
            "Taeja",
            "Rain",
            "Bomber",
            "FanTaSy",
            "Stephano",
            "Thorzain" 
        };

        private readonly string testDatabaseName;

        public TournamentServiceTestBase()
        {
            testDatabaseName = Guid.NewGuid().ToString();

            using (TournamentService tournamentService = CreateTournamentService())
            {
                tournamentService.CreateTournament(tournamentName);
                tournamentService.Save();
            }
        }

        protected UserService CreateUserService()
        {
            return new UserService(InMemoryContextCreator.Create(testDatabaseName));
        }

        protected TournamentService CreateTournamentService()
        {
            return new TournamentService(InMemoryContextCreator.Create(testDatabaseName));
        }

        protected void InitializeUsersAndBetters()
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

        protected void InitializeRoundGroupAndPlayers()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                RoundRobinRound round = tournamentService.AddRoundRobinRoundToTournament(tournament);
                tournamentService.SetPlayersPerGroupCountInRound(round, playerNames.Count);

                foreach (string playerName in playerNames)
                {
                    tournamentService.RegisterPlayerReference(tournament, playerName);
                }

                tournamentService.Save();
            }
        }
    }
}
