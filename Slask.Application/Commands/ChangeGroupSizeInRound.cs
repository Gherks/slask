using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using Slask.Domain.Rounds;
using System;

namespace Slask.Application.Commands
{
    public sealed class ChangeGroupSizeInRound : CommandInterface
    {
        public Guid TournamentId { get; }
        public Guid RoundId { get; }
        public int PlayersPerGroupCount { get; }

        public ChangeGroupSizeInRound(Guid tournamentId, Guid roundId, int playersPerGroupCount)
        {
            TournamentId = tournamentId;
            RoundId = roundId;
            PlayersPerGroupCount = playersPerGroupCount;
        }
    }

    public sealed class ChangeGroupSizeInRoundHandler : CommandHandlerInterface<ChangeGroupSizeInRound>
    {
        private readonly TournamentRepositoryInterface tournamentRepository;

        public ChangeGroupSizeInRoundHandler(TournamentRepositoryInterface tournamentRepository)
        {
            tournamentRepository = tournamentRepository;
        }

        public Result Handle(ChangeGroupSizeInRound command)
        {
            Tournament tournament = tournamentRepository.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not change players per group count ({ command.PlayersPerGroupCount }) setting in round ({ command.RoundId }). Tournament ({ command.TournamentId }) not found.");
            }

            RoundBase round = tournament.GetRoundById(command.RoundId);

            if (round == null)
            {
                return Result.Failure($"Could not change players per group count ({ command.PlayersPerGroupCount }) setting in round ({ command.RoundId }). Round not found.");
            }

            bool changeSuccessful = tournamentRepository.SetPlayersPerGroupCountInRound(round, command.PlayersPerGroupCount);

            if (!changeSuccessful)
            {
                return Result.Failure($"Could not change players per group count ({ command.PlayersPerGroupCount }) setting in round ({ command.RoundId }).");
            }

            tournamentRepository.Save();
            return Result.Success();
        }
    }
}
