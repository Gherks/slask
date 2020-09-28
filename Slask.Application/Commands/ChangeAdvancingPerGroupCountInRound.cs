using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using Slask.Domain.Rounds;
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
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public ChangeAdvancingPerGroupCountInRoundHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(ChangeAdvancingPerGroupCountInRound command)
        {
            Tournament tournament = _tournamentRepository.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not change advancing per group count ({ command.AdvancingPerGroupCount }) setting in round ({ command.RoundId }). Tournament ({ command.TournamentId }) not found.");
            }

            RoundBase round = tournament.GetRoundById(command.RoundId);

            if (round == null)
            {
                return Result.Failure($"Could not change advancing per group count ({ command.AdvancingPerGroupCount }) setting in round ({ command.RoundId }). Round not found.");
            }

            bool changeSuccessful = _tournamentRepository.SetAdvancingPerGroupCountInRound(round, command.AdvancingPerGroupCount);

            if (!changeSuccessful)
            {
                return Result.Failure($"Could not change advancing per group count ({ command.AdvancingPerGroupCount }) setting in round ({ command.RoundId }).");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
