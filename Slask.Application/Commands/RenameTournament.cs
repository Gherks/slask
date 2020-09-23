﻿using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using System;

namespace Slask.Application.Commands
{
    public sealed class RenameTournament : CommandInterface
    {
        public Guid TournamentId { get; }
        public string TournamentName { get; }

        public RenameTournament(Guid tournamentId, string tournamentName)
        {
            TournamentId = tournamentId;
            TournamentName = tournamentName;
        }
    }

    public sealed class RenameTournamentHandler : CommandHandlerInterface<RenameTournament>
    {
        private readonly TournamentRepositoryInterface tournamentRepository;

        public RenameTournamentHandler(TournamentRepositoryInterface tournamentRepository)
        {
            tournamentRepository = tournamentRepository;
        }

        public Result Handle(RenameTournament command)
        {
            bool renameSuccessful = tournamentRepository.RenameTournament(command.TournamentId, command.TournamentName);

            if (!renameSuccessful)
            {
                return Result.Failure($"Could not rename tournament ({ command.TournamentId }) to \"({ command.TournamentName })\"");
            }

            tournamentRepository.Save();
            return Result.Success();
        }
    }
}
