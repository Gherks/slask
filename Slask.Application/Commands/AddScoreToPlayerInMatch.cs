using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
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
        private readonly TournamentRepositoryInterface tournamentRepository;

        public AddScoreToPlayerInMatchHandler(TournamentRepositoryInterface tournamentRepository)
        {
            tournamentRepository = tournamentRepository;
        }

        public Result Handle(AddScoreToPlayerInMatch command)
        {
            Tournament tournament = tournamentRepository.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could add score ({ command.Score }) to player ({ command.PlayerId }) in match ({ command.MatchId }). Tournament ({ command.TournamentId }) not found.");
            }

            bool scoreAdded = tournamentRepository.AddScoreToPlayerInMatch(tournament, command.MatchId, command.PlayerId, command.Score);

            if (!scoreAdded)
            {
                return Result.Failure($"Could add score ({ command.Score }) to player ({ command.PlayerId }) in match ({ command.MatchId }).");
            }

            tournamentRepository.Save();
            return Result.Success();
        }
    }
}
