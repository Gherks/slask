using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class DeleteBetterFromTournamentByGuid : CommandInterface
    {
    }

    public sealed class DeleteBetterFromTournamentByGuidHandler : CommandHandlerInterface<DeleteBetterFromTournamentByGuid>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public DeleteBetterFromTournamentByGuidHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(DeleteBetterFromTournamentByGuid command)
        {
            return Result.Success();
        }
    }
}
