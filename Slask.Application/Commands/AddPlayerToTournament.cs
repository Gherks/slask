using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class AddPlayerToTournament : CommandInterface
    {
        public Guid TournamentId { get; }
        public string PlayerName { get; }

        public AddPlayerToTournament(Guid tournamentId, string playerName)
        {
            TournamentId = tournamentId;
            PlayerName = playerName;
        }
    }

    public sealed class AddPlayerToTournamentHandler : CommandHandlerInterface<AddPlayerToTournament>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public AddPlayerToTournamentHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(AddPlayerToTournament command)
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

            _tournamentService.Save();
            return Result.Success();
        }
    }
}
