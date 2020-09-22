using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class DeletePlayerFromTournamentByName : CommandInterface
    {
        public Guid TournamentId { get; }

        public DeletePlayerFromTournamentByName(Guid tournamentId)
        {
            TournamentId = tournamentId;
        }
    }

    public sealed class DeletePlayerFromTournamentByNameHandler : CommandHandlerInterface<DeletePlayerFromTournamentByName>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public DeletePlayerFromTournamentByNameHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(DeletePlayerFromTournamentByName command)
        {
            _tournamentService.Save();
            return Result.Success();
        }
    }
}
