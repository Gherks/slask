using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;
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
        private readonly TournamentServiceInterface _tournamentService;

        public BetterPlaceBetOnMatchHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(BetterPlaceBetOnMatch command)
        {
            bool betPlaced = _tournamentService.BetterPlacesMatchBetOnMatch(
                command.TournamentId,
                command.MatchId,
                command.BetterName,
                command.PlayerName);

            if (!betPlaced)
            {
                return Result.Failure($"Better ({ command.BetterName }) could not place match bet on player ({ command.PlayerName }) in match ({ command.MatchId }) within tournament ({ command.TournamentId })");
            }

            return Result.Success();
        }
    }
}
