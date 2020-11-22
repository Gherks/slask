using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using Slask.Domain.Rounds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Slask.Application.Commands
{
    public class CommandQueryUtilities
    {

        internal static User GetUserByIdentifier(UserRepositoryInterface userRepository, string userIdentifier)
        {
            if (Guid.TryParse(userIdentifier, out Guid userId))
            {
                return userRepository.GetUser(userId);
            }

            return userRepository.GetUser(userIdentifier);
        }

        internal static Tournament GetTournamentByIdentifier(TournamentRepositoryInterface tournamentRepository, string tournamentIdentifier)
        {
            if (Guid.TryParse(tournamentIdentifier, out Guid tournamentId))
            {
                return tournamentRepository.GetTournament(tournamentId);
            }

            return tournamentRepository.GetTournament(tournamentIdentifier);
        }

        internal static RoundBase GetRoundByIdentifier(Tournament tournament, string roundIdentifier)
        {
            if (Guid.TryParse(roundIdentifier, out Guid roundId))
            {
                return tournament.GetRound(roundId);
            }

            return tournament.GetRound(roundIdentifier);
        }
    }
}
