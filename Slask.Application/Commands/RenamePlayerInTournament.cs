using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
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
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public RenamePlayerInTournamentHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(RenamePlayerInTournament command)
        {
            Tournament tournament = _tournamentRepository.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not rename player ({ command.CurrentPlayerName }) to { command.NewPlayerName } in tournament ({ command.TournamentId }). Tournament not found.");
            }

            PlayerReference playerReference = tournament.GetPlayerReferenceByName(command.CurrentPlayerName);

            if (playerReference == null)
            {
                return Result.Failure($"Could not rename player ({ command.CurrentPlayerName }) to { command.NewPlayerName } in tournament ({ command.TournamentId }). Player not found.");
            }

            bool renameSuccessful = _tournamentRepository.RenamePlayerReferenceInTournament(playerReference, command.NewPlayerName);

            if (!renameSuccessful)
            {
                return Result.Failure($"Could not rename player ({ command.CurrentPlayerName }) to { command.NewPlayerName } in tournament ({ command.TournamentId }).");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
