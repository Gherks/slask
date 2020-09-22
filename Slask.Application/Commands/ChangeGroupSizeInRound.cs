using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.Persistence.Services;
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
        private readonly TournamentServiceInterface _tournamentService;

        public ChangeGroupSizeInRoundHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(ChangeGroupSizeInRound command)
        {
            Tournament tournament = _tournamentService.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not change players per group count ({ command.PlayersPerGroupCount }) setting in round ({ command.RoundId }). Tournament ({ command.TournamentId }) not found.");
            }

            RoundBase round = tournament.GetRoundByRoundId(command.RoundId);

            if (round == null)
            {
                return Result.Failure($"Could not change players per group count ({ command.PlayersPerGroupCount }) setting in round ({ command.RoundId }). Round not found.");
            }

            bool changeSuccessfull = _tournamentService.SetPlayersPerGroupCountInRound(round, command.PlayersPerGroupCount);

            if (!changeSuccessfull)
            {
                return Result.Failure($"Could not change players per group count ({ command.PlayersPerGroupCount }) setting in round ({ command.RoundId }).");
            }

            _tournamentService.Save();
            return Result.Success();
        }
    }
}
