using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Rounds;
using System;

namespace Slask.Application.Commands
{
    public sealed class AddRoundToTournament : CommandInterface
    {
        public Guid TournamentId { get; }
        public string RoundType { get; }

        public AddRoundToTournament(Guid tournamentId, string roundType)
        {
            TournamentId = tournamentId;
            RoundType = roundType;
        }
    }

    public sealed class AddRoundToTournamentHandler : CommandHandlerInterface<AddRoundToTournament>
    {
        private readonly TournamentRepositoryInterface tournamentRepository;

        public AddRoundToTournamentHandler(TournamentRepositoryInterface tournamentRepository)
        {
            tournamentRepository = tournamentRepository;
        }

        public Result Handle(AddRoundToTournament command)
        {
            Tournament tournament = tournamentRepository.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not add round ({ command.RoundType }) to tournament. Tournament ({ command.TournamentId }) not found.");
            }

            string parsedRoundType = StringUtility.ToUpperNoSpaces(command.RoundType);
            RoundBase round;

            switch (parsedRoundType)
            {
                case "BRACKET":
                    round = tournamentRepository.AddBracketRoundToTournament(tournament);
                    break;
                case "DUALTOURNAMENT":
                    round = tournamentRepository.AddDualTournamentRoundToTournament(tournament);
                    break;
                case "ROUNDROBIN":
                    round = tournamentRepository.AddRoundRobinRoundToTournament(tournament);
                    break;
                default:
                    return Result.Failure($"Could not add round ({ command.RoundType }) to tournament. Invalid round type ({ command.RoundType }) given.");
            }

            if (round == null)
            {
                return Result.Failure($"Could not add round ({ command.RoundType }) to tournament.");
            }

            tournamentRepository.Save();
            return Result.Success();
        }
    }
}
