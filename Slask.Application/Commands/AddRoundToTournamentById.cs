using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class AddRoundToTournamentById : CommandInterface
    {
        public Guid TournamentId { get; }
        public string RoundType { get; }

        public AddRoundToTournamentById(Guid tournamentId, string roundType)
        {
            TournamentId = tournamentId;
            RoundType = roundType;
        }
    }

    public sealed class AddRoundToTournamentByIdHandler : CommandHandlerInterface<AddRoundToTournamentById>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public AddRoundToTournamentByIdHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(AddRoundToTournamentById command)
        {
            Tournament tournament = _tournamentService.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not add round ({ command.RoundType }) to tournament. Tournament ({ command.TournamentId }) not found.");
            }

            string parsedRoundType = StringUtility.ToUpperNoSpaces(command.RoundType);
            RoundBase round;

            switch (parsedRoundType)
            {
                case "BRACKET":
                    round = _tournamentService.AddBracketRoundToTournament(tournament);
                    break;
                case "DUALTOURNAMENT":
                    round = _tournamentService.AddDualTournamentRoundToTournament(tournament);
                    break;
                case "ROUNDROBIN":
                    round = _tournamentService.AddRoundRobinRoundToTournament(tournament);
                    break;
                default:
                    return Result.Failure($"Could not add round ({ command.RoundType }) to tournament. Invalid round type ({ command.RoundType }) given.");
            }

            if (round == null)
            {
                return Result.Failure($"Could not add round ({ command.RoundType }) to tournament.");
            }

            return Result.Success();
        }
    }
}
