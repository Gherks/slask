using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using System;

namespace Slask.Application.Commands
{
    public sealed class RemoveRoundFromTournament : CommandInterface
    {
        public Guid TournamentId { get; }
        public Guid RoundId { get; }

        public RemoveRoundFromTournament(Guid tournamentId, Guid roundId)
        {
            TournamentId = tournamentId;
            RoundId = roundId;
        }
    }

    public sealed class RemoveRoundFromTournamentHandler : CommandHandlerInterface<RemoveRoundFromTournament>
    {
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public RemoveRoundFromTournamentHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(RemoveRoundFromTournament command)
        {
            Tournament tournament = _tournamentRepository.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not remove round ({ command.RoundId }) from tournament ({ command.TournamentId }). Tournament not found.");
            }

            bool roundRemoved = _tournamentRepository.RemoveRoundFromTournament(tournament, command.RoundId);

            if (!roundRemoved)
            {
                return Result.Failure($"Could not remove round ({ command.RoundId }) from tournament ({ command.TournamentId }).");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
