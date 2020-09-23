using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class RenamePlayerInTournament : CommandInterface
    {
        public Guid TournamentId { get; }
        public string CurrentPlayerName { get; }
        public string NewPlayerName { get; }

        public RenamePlayerInTournament(Guid tournamentId, string currentPlayerName, string newPlayerName)
        {
            TournamentId = tournamentId;
            CurrentPlayerName = currentPlayerName;
            NewPlayerName = newPlayerName;
        }
    }

    public sealed class RenamePlayerInTournamentHandler : CommandHandlerInterface<RenamePlayerInTournament>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public RenamePlayerInTournamentHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(RenamePlayerInTournament command)
        {
            Tournament tournament = _tournamentService.GetTournamentById(command.TournamentId);

            if(tournament == null)
            {
                return Result.Failure($"Could not rename player ({ command.CurrentPlayerName }) to { command.NewPlayerName } in tournament ({ command.TournamentId }). Tournament not found.");
            }

            PlayerReference playerReference = tournament.GetPlayerReferenceByName(command.CurrentPlayerName);

            if (playerReference == null)
            {
                return Result.Failure($"Could not rename player ({ command.CurrentPlayerName }) to { command.NewPlayerName } in tournament ({ command.TournamentId }). Player not found.");
            }

            bool renameSuccessful = _tournamentService.RenamePlayerReferenceInTournament(playerReference, command.NewPlayerName);

            if (!renameSuccessful)
            {
                return Result.Failure($"Could not rename player ({ command.CurrentPlayerName }) to { command.NewPlayerName } in tournament ({ command.TournamentId }).");
            }

            _tournamentService.Save();
            return Result.Success();
        }
    }
}
