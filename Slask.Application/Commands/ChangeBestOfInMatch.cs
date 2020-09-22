using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class ChangeBestOfInMatch : CommandInterface
    {
        public Guid TournamentId { get; }
        public Guid MatchId { get; }
        public int BestOf { get; }

        public ChangeBestOfInMatch(Guid tournamentId, Guid matchId, int bestOf)
        {
            TournamentId = tournamentId;
            MatchId = matchId;
            BestOf = bestOf;
        }
    }

    public sealed class ChangeBestOfInMatchHandler : CommandHandlerInterface<ChangeBestOfInMatch>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public ChangeBestOfInMatchHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(ChangeBestOfInMatch command)
        {
            Tournament tournament = _tournamentService.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not change best of ({ command.BestOf }) setting in match ({ command.MatchId }). Tournament ({ command.TournamentId }) not found.");
            }

            Match match = tournament.GetMatchByMatchId(command.MatchId);

            if (match == null)
            {
                return Result.Failure($"Could not change best of ({ command.BestOf }) setting in match ({ command.MatchId }). Match not found.");
            }

            bool changeSuccessfull = _tournamentService.SetBestOfInMatch(match, command.BestOf);

            if (!changeSuccessfull)
            {
                return Result.Failure($"Could not change best of ({ command.BestOf }) setting in match ({ command.MatchId }).");
            }

            _tournamentService.Save();
            return Result.Success();
        }
    }
}
