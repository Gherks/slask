using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class AddPlayerToTournamentByGuid : CommandInterface
    {
        public Guid TournamentId { get; }
        public string PlayerName { get; }

        public AddPlayerToTournamentByGuid(Guid tournamentId, string playerName)
        {
            TournamentId = tournamentId;
            PlayerName = playerName;
        }
    }

    public sealed class AddPlayerToTournamentByGuidHandler : CommandHandlerInterface<AddPlayerToTournamentByGuid>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public AddPlayerToTournamentByGuidHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(AddPlayerToTournamentByGuid command)
        {
            Tournament tournament = _tournamentService.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not add new player ({ command.PlayerName }) to tournament. Tournament ({ command.TournamentId }) not found.");
            }

            PlayerReference playerReference = _tournamentService.AddPlayerReference(tournament, command.PlayerName);

            if (playerReference == null)
            {
                return Result.Failure($"Could not add new player ({ command.PlayerName }) to tournament.");
            }

            return Result.Success();
        }
    }
}
