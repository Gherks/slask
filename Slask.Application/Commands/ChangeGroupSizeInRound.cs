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
        public string TournamentIdentifier { get; }
        public string RoundIdentifier { get; }
        public int PlayersPerGroupCount { get; }

        public ChangeGroupSizeInRound(string tournamentIdentifier, string roundIdentifier, int playersPerGroupCount)
        {
            TournamentIdentifier = tournamentIdentifier;
            RoundIdentifier = roundIdentifier;
            PlayersPerGroupCount = playersPerGroupCount;
        }
    }

    public sealed class ChangeGroupSizeInRoundHandler : CommandHandlerInterface<ChangeGroupSizeInRound>
    {
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public ChangeGroupSizeInRoundHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(ChangeGroupSizeInRound command)
        {
            Tournament tournament;

            if (Guid.TryParse(command.TournamentIdentifier, out Guid tournamentId))
            {
                tournament = _tournamentRepository.GetTournament(tournamentId);
            }
            else
            {
                tournament = _tournamentRepository.GetTournament(command.TournamentIdentifier);
            }

            if (tournament == null)
            {
                return Result.Failure($"Could not change players per group count ({ command.PlayersPerGroupCount }) setting in round ({ command.RoundIdentifier }). Tournament ({ command.TournamentIdentifier }) not found.");
            }

            RoundBase round;

            if (Guid.TryParse(command.TournamentIdentifier, out Guid roundId))
            {
                round = tournament.GetRoundById(roundId);
            }
            else
            {
                round = tournament.GetRoundByName(command.RoundIdentifier);
            }

            if (round == null)
            {
                return Result.Failure($"Could not change players per group count ({ command.PlayersPerGroupCount }) setting in round ({ command.RoundIdentifier }). Round not found.");
            }

            bool changeSuccessful = _tournamentRepository.SetPlayersPerGroupCountInRound(round, command.PlayersPerGroupCount);

            if (!changeSuccessful)
            {
                return Result.Failure($"Could not change players per group count ({ command.PlayersPerGroupCount }) setting in round ({ command.RoundIdentifier }).");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
