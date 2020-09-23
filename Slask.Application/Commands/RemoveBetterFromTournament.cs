using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class RemoveBetterFromTournament : CommandInterface
    {
        public Guid TournamentId { get; }
        public Guid BetterId { get; }

        public RemoveBetterFromTournament(Guid tournamentId, Guid betterId)
        {
            TournamentId = tournamentId;
            BetterId = betterId;
        }
    }

    public sealed class RemoveBetterFromTournamentHandler : CommandHandlerInterface<RemoveBetterFromTournament>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public RemoveBetterFromTournamentHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(RemoveBetterFromTournament command)
        {
            Tournament tournament = _tournamentService.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not remove better ({ command.BetterId }) from tournament ({ command.TournamentId }). Tournament not found.");
            }

            bool betterRemoved = _tournamentService.RemoveBetterFromTournamentById(tournament, command.BetterId);

            if (!betterRemoved)
            {
                return Result.Failure($"Could not remove better ({ command.BetterId }) from tournament ({ command.TournamentId }).");
            }

            _tournamentService.Save();
            return Result.Success();
        }
    }
}
