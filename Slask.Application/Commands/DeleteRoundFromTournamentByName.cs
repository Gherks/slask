using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class DeleteRoundFromTournamentByName : CommandInterface
    {
        public Guid TournamentId { get; }

        public DeleteRoundFromTournamentByName(Guid tournamentId)
        {
            TournamentId = tournamentId;
        }
    }

    public sealed class DeleteRoundFromTournamentByNameHandler : CommandHandlerInterface<DeleteRoundFromTournamentByName>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public DeleteRoundFromTournamentByNameHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(DeleteRoundFromTournamentByName command)
        {
            _tournamentService.Save();
            return Result.Success();
        }
    }
}
