using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
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
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public ChangeBestOfInMatchHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(ChangeBestOfInMatch command)
        {
            Tournament tournament = _tournamentRepository.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not change best of ({ command.BestOf }) setting in match ({ command.MatchId }). Tournament ({ command.TournamentId }) not found.");
            }

            Match match = tournament.GetMatchById(command.MatchId);

            if (match == null)
            {
                return Result.Failure($"Could not change best of ({ command.BestOf }) setting in match ({ command.MatchId }). Match not found.");
            }

            bool changeSuccessful = _tournamentRepository.SetBestOfInMatch(match, command.BestOf);

            if (!changeSuccessful)
            {
                return Result.Failure($"Could not change best of ({ command.BestOf }) setting in match ({ command.MatchId }).");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
