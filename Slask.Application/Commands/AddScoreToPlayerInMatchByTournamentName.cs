using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class AddScoreToPlayerInMatchByTournamentName : CommandInterface
    {
        public string TournamentName { get; }
        public Guid MatchId { get; }
        public Guid PlayerId { get; }
        public int Score { get; }

        public AddScoreToPlayerInMatchByTournamentName(string tournamentName, Guid matchId, Guid playerId, int score)
        {
            TournamentName = tournamentName;
            MatchId = matchId;
            PlayerId = playerId;
            Score = score;
        }
    }

    public sealed class AddScoreToPlayerInMatchByTournamentNameHandler : CommandHandlerInterface<AddScoreToPlayerInMatchByTournamentName>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public AddScoreToPlayerInMatchByTournamentNameHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(AddScoreToPlayerInMatchByTournamentName command)
        {
            Tournament tournament = _tournamentService.GetTournamentByName(command.TournamentName);

            if (tournament == null)
            {
                return Result.Failure($"Could add score ({ command.Score }) to player ({ command.PlayerId }) in match ({ command.MatchId }). Tournament ({ command.TournamentName }) not found.");
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
