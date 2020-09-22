using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class DeleteTournamentByName : CommandInterface
    {
        public Guid TournamentId { get; }

        public DeleteTournamentByName(Guid tournamentId)
        {
            TournamentId = tournamentId;
        }
    }

    public sealed class DeleteTournamentByNameHandler : CommandHandlerInterface<DeleteTournamentByName>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public DeleteTournamentByNameHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(DeleteTournamentByName command)
        {
            _tournamentService.Save();
            return Result.Success();
        }
    }
}
