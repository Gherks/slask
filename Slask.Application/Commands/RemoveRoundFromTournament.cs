using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using Slask.Domain.Rounds;
using System;

namespace Slask.Application.Commands
{
    public sealed class RemoveRoundFromTournament : CommandInterface
    {
        public string TournamentIdentifier { get; }
        public string RoundIdentifier { get; }

        public RemoveRoundFromTournament(string tournamentIdentifier, string roundIdentifier)
        {
            TournamentIdentifier = tournamentIdentifier;
            RoundIdentifier = roundIdentifier;
        }
    }

    public sealed class RemoveRoundFromTournamentHandler : CommandHandlerInterface<RemoveRoundFromTournament>
    {
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public RemoveRoundFromTournamentHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(RemoveRoundFromTournament command)
        {
            Tournament tournament = GetTournamentFromIdentifer(command.TournamentIdentifier);

            if (tournament == null)
            {
                return Result.Failure($"Could not remove round ({ command.RoundIdentifier }) from tournament ({ command.TournamentIdentifier }). Tournament not found.");
            }

            RoundBase round = GetRoundFromIdentifer(tournament, command.RoundIdentifier);

            if (round == null)
            {
                return Result.Failure($"Could not remove round ({ command.RoundIdentifier }) from tournament ({ command.TournamentIdentifier }). Round not found.");
            }

            bool roundRemoved = _tournamentRepository.RemoveRoundFromTournament(tournament, round.Id);

            if (!roundRemoved)
            {
                return Result.Failure($"Could not remove round ({ command.RoundIdentifier }) from tournament ({ command.TournamentIdentifier }).");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }

        private Tournament GetTournamentFromIdentifer(string tournamentIdentifier)
        {
            if (Guid.TryParse(tournamentIdentifier, out Guid tournamentId))
            {
                return _tournamentRepository.GetTournament(tournamentId);
            }
            else
            {
                return _tournamentRepository.GetTournament(tournamentIdentifier);
            }
        }

        private RoundBase GetRoundFromIdentifer(Tournament tournament, string roundIdentifier)
        {
            if (Guid.TryParse(roundIdentifier, out Guid roundId))
            {
                return tournament.GetRoundById(roundId);
            }
            else
            {
                return tournament.GetRoundByName(roundIdentifier);
            }
        }
    }
}
