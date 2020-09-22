using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class RenamePlayerInTournament : CommandInterface
    {
        public Guid TournamentId { get; }

        public RenamePlayerInTournament(Guid tournamentId)
        {
            TournamentId = tournamentId;
        }
    }

    public sealed class RenamePlayerInTournamentHandler : CommandHandlerInterface<RenamePlayerInTournament>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public RenamePlayerInTournamentHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(RenamePlayerInTournament command)
        {
            _tournamentService.Save();
            return Result.Success();
        }
    }
}
