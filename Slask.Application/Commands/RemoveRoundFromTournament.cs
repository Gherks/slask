﻿using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using Slask.Domain.Rounds;

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
            Tournament tournament = CommandQueryUtilities.GetTournamentByIdentifier(_tournamentRepository, command.TournamentIdentifier);

            if (tournament == null)
            {
                return Result.Failure($"Could not remove round ({ command.RoundIdentifier }) from tournament ({ command.TournamentIdentifier }). Tournament not found.");
            }

            RoundBase round = CommandQueryUtilities.GetRoundByIdentifier(tournament, command.RoundIdentifier);

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
    }
}
