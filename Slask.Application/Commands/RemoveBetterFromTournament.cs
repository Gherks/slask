using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
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
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public RemoveBetterFromTournamentHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(RemoveBetterFromTournament command)
        {
            Tournament tournament = _tournamentRepository.GetTournament(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not remove better ({ command.BetterId }) from tournament ({ command.TournamentId }). Tournament not found.");
            }

            bool betterRemoved = _tournamentRepository.RemoveBetterFromTournament(tournament, command.BetterId);

            if (!betterRemoved)
            {
                return Result.Failure($"Could not remove better ({ command.BetterId }) from tournament ({ command.TournamentId }).");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
