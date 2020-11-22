using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using Slask.Domain.Rounds;
using System;

namespace Slask.Application.Commands
{
    public sealed class RenameRoundInTournament : CommandInterface
    {
        public Guid TournamentId { get; }
        public Guid RoundId { get; }
        public string NewRoundName { get; }

        public RenameRoundInTournament(Guid tournamentId, Guid roundId, string newRoundName)
        {
            TournamentId = tournamentId;
            RoundId = roundId;
            NewRoundName = newRoundName;
        }
    }

    public sealed class RenameRoundInTournamentHandler : CommandHandlerInterface<RenameRoundInTournament>
    {
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public RenameRoundInTournamentHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(RenameRoundInTournament command)
        {
            Tournament tournament = _tournamentRepository.GetTournament(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not rename round ({ command.RoundId }) to { command.NewRoundName } in tournament ({ command.TournamentId }). Tournament not found.");
            }

            RoundBase round = tournament.GetRound(command.RoundId);

            if (round == null)
            {
                return Result.Failure($"Could not rename round ({ command.RoundId }) to { command.NewRoundName } in tournament ({ command.TournamentId }). Round not found.");
            }

            bool renameSuccessful = _tournamentRepository.RenameRoundInTournament(round, command.NewRoundName);

            if (!renameSuccessful)
            {
                return Result.Failure($"Could not rename round ({ command.RoundId }) to { command.NewRoundName } in tournament ({ command.TournamentId }).");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
