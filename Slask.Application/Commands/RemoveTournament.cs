using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class RemoveTournament : CommandInterface
    {
        public Guid TournamentId { get; }

        public RemoveTournament(Guid tournamentId)
        {
            TournamentId = tournamentId;
        }
    }

    public sealed class RemoveTournamentHandler : CommandHandlerInterface<RemoveTournament>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public RemoveTournamentHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(RemoveTournament command)
        {
            bool tournamentRemoved = _tournamentService.RemoveTournament(command.TournamentId);

            if (!tournamentRemoved)
            {
                return Result.Failure($"Could remove tournament ({ command.TournamentId }). Tournament not found.");
            }

            _tournamentService.Save();
            return Result.Success();
        }
    }
}
