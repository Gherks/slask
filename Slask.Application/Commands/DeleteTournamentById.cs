using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class DeleteTournamentById : CommandInterface
    {
        public Guid TournamentId { get; }

        public DeleteTournamentById(Guid tournamentId)
        {
            TournamentId = tournamentId;
        }
    }

    public sealed class DeleteTournamentByIdHandler : CommandHandlerInterface<DeleteTournamentById>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public DeleteTournamentByIdHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(DeleteTournamentById command)
        {
            _tournamentService.Save();
            return Result.Success();
        }
    }
}
