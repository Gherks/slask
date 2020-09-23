using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using System;

namespace Slask.Application.Commands
{
    public sealed class BetterPlaceBetOnMatch : CommandInterface
    {
        public Guid TournamentId { get; }
        public Guid MatchId { get; }
        public string BetterName { get; }
        public string PlayerName { get; }

        public BetterPlaceBetOnMatch(Guid tournamentId, Guid matchId, string betterName, string playerName)
        {
            TournamentId = tournamentId;
            MatchId = matchId;
            BetterName = betterName;
            PlayerName = playerName;
        }
    }

    public sealed class BetterPlaceBetOnMatchHandler : CommandHandlerInterface<BetterPlaceBetOnMatch>
    {
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public BetterPlaceBetOnMatchHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(BetterPlaceBetOnMatch command)
        {
            bool betPlaced = _tournamentRepository.BetterPlacesMatchBetOnMatch(
                command.TournamentId,
                command.MatchId,
                command.BetterName,
                command.PlayerName);

            if (!betPlaced)
            {
                return Result.Failure($"Better ({ command.BetterName }) could not place match bet on player ({ command.PlayerName }) in match ({ command.MatchId }) within tournament ({ command.TournamentId })");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
