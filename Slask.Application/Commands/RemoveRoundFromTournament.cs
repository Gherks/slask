using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Persistence.Services;
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
        private readonly TournamentServiceInterface _tournamentService;

        public RemoveRoundFromTournamentHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(RemoveRoundFromTournament command)
        {
            Tournament tournament = _tournamentService.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not remove round ({ command.RoundId }) from tournament ({ command.TournamentId }). Tournament not found.");
            }

            bool roundRemoved = _tournamentService.RemoveRoundFromTournament(tournament, command.RoundId);

            if (!roundRemoved)
            {
                return Result.Failure($"Could not remove round ({ command.RoundId }) from tournament ({ command.TournamentId }).");
            }

            _tournamentService.Save();
            return Result.Success();
        }
    }
}
