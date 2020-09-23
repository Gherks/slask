﻿using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class ChangeStartTimeOfMatch : CommandInterface
    {
        public Guid TournamentId { get; }
        public Guid MatchId { get; }
        public DateTime StartDateTime { get; }

        public ChangeStartTimeOfMatch(Guid tournamentId, Guid matchId, DateTime startDateTime)
        {
            TournamentId = tournamentId;
            MatchId = matchId;
            StartDateTime = startDateTime;
        }
    }

    public sealed class ChangeStartTimeOfMatchHandler : CommandHandlerInterface<ChangeStartTimeOfMatch>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public ChangeStartTimeOfMatchHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(ChangeStartTimeOfMatch command)
        {
            Tournament tournament = _tournamentService.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not change start time ({ command.StartDateTime }) in match ({ command.MatchId }). Tournament ({ command.TournamentId }) not found.");
            }

            Match match = tournament.GetMatchById(command.MatchId);

            if (match == null)
            {
                return Result.Failure($"Could not change start time ({ command.StartDateTime }) in match ({ command.MatchId }). Match not found.");
            }

            bool changeSuccessful = _tournamentService.SetStartTimeForMatch(match, command.StartDateTime);

            if (!changeSuccessful)
            {
                return Result.Failure($"Could not change start time ({ command.StartDateTime }) in match ({ command.MatchId }).");
            }

            _tournamentService.Save();
            return Result.Success();
        }
    }
}
