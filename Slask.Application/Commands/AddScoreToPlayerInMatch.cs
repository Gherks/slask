using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class AddScoreToPlayerInMatch : CommandInterface
    {
        public Guid TournamentId { get; }
        public Guid MatchId { get; }
        public Guid PlayerId { get; }
        public int Score { get; }

        public AddScoreToPlayerInMatch(Guid tournamentId, Guid matchId, Guid playerId, int score)
        {
            TournamentId = tournamentId;
            MatchId = matchId;
            PlayerId = playerId;
            Score = score;
        }
    }

    public sealed class AddScoreToPlayerInMatchHandler : CommandHandlerInterface<AddScoreToPlayerInMatch>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public AddScoreToPlayerInMatchHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(AddScoreToPlayerInMatch command)
        {
            Tournament tournament = _tournamentService.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could add score ({ command.Score }) to player ({ command.PlayerId }) in match ({ command.MatchId }). Tournament ({ command.TournamentId }) not found.");
            }

            bool scoreAdded = _tournamentService.AddScoreToPlayerInMatch(tournament, command.MatchId, command.PlayerId, command.Score);

            if (!scoreAdded)
            {
                return Result.Failure($"Could add score ({ command.Score }) to player ({ command.PlayerId }) in match ({ command.MatchId }).");
            }

            return Result.Success();
        }
    }
}
