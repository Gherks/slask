using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using System;

namespace Slask.Application.Commands
{
    public sealed class RenameTournament : CommandInterface
    {
        public string TournamentIdentifier { get; }
        public string TournamentName { get; }

        public RenameTournament(string tournamentIdentifier, string tournamentName)
        {
            TournamentIdentifier = tournamentIdentifier;
            TournamentName = tournamentName;
        }
    }

    public sealed class RenameTournamentHandler : CommandHandlerInterface<RenameTournament>
    {
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public RenameTournamentHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(RenameTournament command)
        {
            bool renameSuccessful;

            if (Guid.TryParse(command.TournamentIdentifier, out Guid tournamentId))
            {
                renameSuccessful = _tournamentRepository.RenameTournament(tournamentId, command.TournamentName);
            }
            else
            {
                renameSuccessful = _tournamentRepository.RenameTournament(command.TournamentIdentifier, command.TournamentName);
            }

            if (!renameSuccessful)
            {
                return Result.Failure($"Could not rename tournament ({ command.TournamentIdentifier }) to ({ command.TournamentName })");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
