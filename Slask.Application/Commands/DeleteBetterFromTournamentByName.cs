using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class DeleteBetterFromTournamentByName : CommandInterface
    {
        public Guid TournamentId { get; }

        public DeleteBetterFromTournamentByName(Guid tournamentId)
        {
            TournamentId = tournamentId;
        }
    }

    public sealed class DeleteBetterFromTournamentByNameHandler : CommandHandlerInterface<DeleteBetterFromTournamentByName>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public DeleteBetterFromTournamentByNameHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(DeleteBetterFromTournamentByName command)
        {
            _tournamentService.Save();
            return Result.Success();
        }
    }
}
