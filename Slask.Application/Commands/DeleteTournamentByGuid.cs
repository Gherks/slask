using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class DeleteTournamentByGuid : CommandInterface
    {
    }

    public sealed class DeleteTournamentByGuidHandler : CommandHandlerInterface<DeleteTournamentByGuid>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public DeleteTournamentByGuidHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(DeleteTournamentByGuid command)
        {
            return Result.Success();
        }
    }
}
