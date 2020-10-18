using Slask.Domain;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Persistence.Repositories;
using Slask.TestCore;
using System;
using System.Collections.Generic;

namespace Slask.Persistence.Xunit.IntegrationTests.tournamentRepositoryTests
{
    public class TournamentRepositoryTestBase
    {
        protected const string _tournamentName = "GSL 2019";
        protected readonly List<string> _playerNames = new List<string>
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

        private readonly string _testDatabaseName = "InMemoryTestDatabase_" + Guid.NewGuid().ToString();

        public TournamentRepositoryTestBase()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                tournamentRepository.CreateTournament(_tournamentName);
                tournamentRepository.Save();
            }
        }

        protected UserRepository CreateuserRepository()
        {
            return new UserRepository(InMemoryContextCreator.Create(_testDatabaseName));
        }

        protected TournamentRepository CreateTournamentRepository()
        {
            return new TournamentRepository(InMemoryContextCreator.Create(_testDatabaseName));
        }

        protected void InitializeUsersAndBetters()
        {
            using (UserRepository userRepository = CreateuserRepository())
            {
                userRepository.CreateUser("Stålberto");
                userRepository.CreateUser("Bönis");
                userRepository.CreateUser("Guggelito");
                userRepository.Save();

                using (TournamentRepository tournamentRepository = CreateTournamentRepository())
                {
                    Tournament tournament = tournamentRepository.GetTournamentByName(_tournamentName);

                    tournamentRepository.AddBetterToTournament(tournament, userRepository.GetUserByName("Stålberto"));
                    tournamentRepository.AddBetterToTournament(tournament, userRepository.GetUserByName("Bönis"));
                    tournamentRepository.AddBetterToTournament(tournament, userRepository.GetUserByName("Guggelito"));
                    tournamentRepository.Save();
                }
            }
        }

        protected void InitializeRoundGroupAndPlayers()
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(_tournamentName);

                RoundRobinRound round = tournamentRepository.AddRoundRobinRoundToTournament(tournament);
                tournamentRepository.SetPlayersPerGroupCountInRound(round, _playerNames.Count);

                foreach (string playerName in _playerNames)
                {
                    tournamentRepository.AddPlayerReference(tournament, playerName);
                }

                tournamentRepository.Save();
            }
        }
    }
}
