using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class ChangeAdvancingPerGroupCountInRound : CommandInterface
    {
        public Guid TournamentId { get; }
        public Guid RoundId { get; }
        public int AdvancingPerGroupCount { get; }

        public ChangeAdvancingPerGroupCountInRound(Guid tournamentId, Guid roundId, int advancingPerGroupCount)
        {
            TournamentId = tournamentId;
            RoundId = roundId;
            AdvancingPerGroupCount = advancingPerGroupCount;
        }
}

    public sealed class ChangeAdvancingPerGroupCountInRoundHandler : CommandHandlerInterface<ChangeAdvancingPerGroupCountInRound>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public ChangeAdvancingPerGroupCountInRoundHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(ChangeAdvancingPerGroupCountInRound command)
        {
            Tournament tournament = _tournamentService.GetTournamentById(command.TournamentId);

            if(tournament == null)
            {
                return Result.Failure($"Could not change advancing per group count ({ command.AdvancingPerGroupCount }) setting in round ({ command.RoundId }). Tournament ({ command.TournamentId }) not found.");
            }

            RoundBase round = tournament.GetRoundByRoundId(command.RoundId);

            if (round == null)
            {
                return Result.Failure($"Could not change advancing per group count ({ command.AdvancingPerGroupCount }) setting in round ({ command.RoundId }). Round not found.");
            }

            bool changeSuccessfull = _tournamentService.SetAdvancingPerGroupCountInRound(round, command.AdvancingPerGroupCount);

            if (!changeSuccessfull)
            {
                return Result.Failure($"Could not change advancing per group count ({ command.AdvancingPerGroupCount }) setting in round ({ command.RoundId }).");
            }

            _tournamentService.Save();
            return Result.Success();
        }
    }
}
