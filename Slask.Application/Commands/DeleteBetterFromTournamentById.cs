using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class DeleteBetterFromTournamentById : CommandInterface
    {
        public Guid TournamentId { get; }

        public DeleteBetterFromTournamentById(Guid tournamentId)
        {
            TournamentId = tournamentId;
        }
    }

    public sealed class DeleteBetterFromTournamentByIdHandler : CommandHandlerInterface<DeleteBetterFromTournamentById>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public DeleteBetterFromTournamentByIdHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(DeleteBetterFromTournamentById command)
        {
            _tournamentService.Save();
            return Result.Success();
        }
    }
}
