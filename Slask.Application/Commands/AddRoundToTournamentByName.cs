using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class AddRoundToTournamentByName : CommandInterface
    {
        public string TournamentName { get; }
        public string RoundType { get; }

        public AddRoundToTournamentByName(string tournamentName, string roundType)
        {
            TournamentName = tournamentName;
            RoundType = roundType;
        }
    }

    public sealed class AddRoundToTournamentByNameHandler : CommandHandlerInterface<AddRoundToTournamentByName>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public AddRoundToTournamentByNameHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(AddRoundToTournamentByName command)
        {
            Tournament tournament = _tournamentService.GetTournamentByName(command.TournamentName);

            if (tournament == null)
            {
                return Result.Failure($"Could not add round ({ command.RoundType }) to tournament. Tournament ({ command.TournamentName }) not found.");
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
