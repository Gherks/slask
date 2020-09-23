using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class RenameTournament : CommandInterface
    {
        public Guid TournamentId { get; }
        public string TournamentName { get; }

        public RenameTournament(Guid tournamentId, string tournamentName)
        {
            TournamentId = tournamentId;
            TournamentName = tournamentName;
        }
    }

    public sealed class RenameTournamentHandler : CommandHandlerInterface<RenameTournament>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public RenameTournamentHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(RenameTournament command)
        {
            bool renameSuccessful = _tournamentService.RenameTournament(command.TournamentId, command.TournamentName);

            if (!renameSuccessful)
            {
                return Result.Failure($"Could not rename tournament ({ command.TournamentId }) to \"({ command.TournamentName })\"");
            }

            _tournamentService.Save();
            return Result.Success();
        }
    }
}
