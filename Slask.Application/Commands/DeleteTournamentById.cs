using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class DeleteTournamentById : CommandInterface
    {
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
            return Result.Success();
        }
    }
}
