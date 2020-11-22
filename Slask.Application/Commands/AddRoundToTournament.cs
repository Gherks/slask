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
        public string TournamentIdentifier { get; }
        public string RoundType { get; }

        public AddRoundToTournament(string tournamentIdentifier, string roundType)
        {
            TournamentIdentifier = tournamentIdentifier;
            RoundType = roundType;
        }
    }

    public sealed class AddRoundToTournamentHandler : CommandHandlerInterface<AddRoundToTournament>
    {
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public AddRoundToTournamentHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(AddRoundToTournament command)
        {
            Tournament tournament = CommandQueryUtilities.GetTournamentByIdentifier(_tournamentRepository, command.TournamentIdentifier);

            if (tournament == null)
            {
                return Result.Failure($"Could not add round ({ command.RoundType }) to tournament. Tournament ({ command.TournamentIdentifier }) not found.");
            }

            string parsedRoundType = command.RoundType.ToUpperNoSpaces();
            RoundBase round;

            switch (parsedRoundType)
            {
                case "BRACKET":
                    round = _tournamentRepository.AddBracketRoundToTournament(tournament);
                    break;
                case "DUALTOURNAMENT":
                    round = _tournamentRepository.AddDualTournamentRoundToTournament(tournament);
                    break;
                case "ROUNDROBIN":
                    round = _tournamentRepository.AddRoundRobinRoundToTournament(tournament);
                    break;
                default:
                    return Result.Failure($"Could not add round ({ command.RoundType }) to tournament. Invalid round type ({ command.RoundType }) given.");
            }

            if (round == null)
            {
                return Result.Failure($"Could not add round ({ command.RoundType }) to tournament.");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
