﻿using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    class RenamePlayerInTournamentCommand : RenamePlayerInTournamentCommandInterface
    {
        private readonly UserServiceInterface _userServiceInterface;
        private readonly TournamentServiceInterface _tournamentServiceInterface;

        public RenamePlayerInTournamentCommand(
                UserServiceInterface userServiceInterface,
                TournamentServiceInterface tournamentServiceInterface)
        {
            _userServiceInterface = userServiceInterface;
            _tournamentServiceInterface = tournamentServiceInterface;
        }

        public void Execute()
        {

        }
    }
}
