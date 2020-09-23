using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class RemovePlayerFromTournament : CommandInterface
    {
        public Guid TournamentId { get; }
        public string PlayerName { get; }

        public RemovePlayerFromTournament(Guid tournamentId, string playerName)
        {
            TournamentId = tournamentId;
            PlayerName = playerName;
        }
    }

    public sealed class RemovePlayerFromTournamentHandler : CommandHandlerInterface<RemovePlayerFromTournament>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public RemovePlayerFromTournamentHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(RemovePlayerFromTournament command)
        {
            Tournament tournament = _tournamentService.GetTournamentById(command.TournamentId);

            if(tournament == null)
            {
                return Result.Failure($"Could not player ({ command.PlayerName}) from tournament ({ command.TournamentId }). Tournament not found.");
            }

            bool playerRemoved = _tournamentService.RemovePlayerReferenceFromTournament(tournament, command.PlayerName);

            if (!playerRemoved)
            {
                return Result.Failure($"Could not player ({ command.PlayerName}) from tournament ({ command.TournamentId }).");
            }

            _tournamentService.Save();
            return Result.Success();
        }
    }
}
