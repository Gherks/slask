using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
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
        private readonly TournamentRepositoryInterface tournamentRepository;

        public RemoveTournamentHandler(TournamentRepositoryInterface tournamentRepository)
        {
            tournamentRepository = tournamentRepository;
        }

        public Result Handle(RemoveTournament command)
        {
            bool tournamentRemoved = tournamentRepository.RemoveTournament(command.TournamentId);

            if (!tournamentRemoved)
            {
                return Result.Failure($"Could remove tournament ({ command.TournamentId }). Tournament not found.");
            }

            tournamentRepository.Save();
            return Result.Success();
        }
    }
}
