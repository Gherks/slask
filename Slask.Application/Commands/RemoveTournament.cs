using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using System;

namespace Slask.Application.Commands
{
    public sealed class RemoveTournament : CommandInterface
    {
        public string TournamentIdentifier { get; }

        public RemoveTournament(string tournamentIdentifier)
        {
            TournamentIdentifier = tournamentIdentifier;
        }
    }

    public sealed class RemoveTournamentHandler : CommandHandlerInterface<RemoveTournament>
    {
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public RemoveTournamentHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(RemoveTournament command)
        {
            bool tournamentRemoved;

            if (Guid.TryParse(command.TournamentIdentifier, out Guid tournamentId))
            {
                tournamentRemoved = _tournamentRepository.RemoveTournament(tournamentId);
            }
            else
            {
                tournamentRemoved = _tournamentRepository.RemoveTournament(command.TournamentIdentifier);
            }

            if (!tournamentRemoved)
            {
                return Result.Failure($"Could remove tournament ({ command.TournamentIdentifier }). Tournament not found.");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
