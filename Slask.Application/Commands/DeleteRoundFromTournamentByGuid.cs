using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class DeleteRoundFromTournamentByGuid : CommandInterface
    {
    }

    public sealed class DeleteRoundFromTournamentByGuidHandler : CommandHandlerInterface<DeleteRoundFromTournamentByGuid>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public DeleteRoundFromTournamentByGuidHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(DeleteRoundFromTournamentByGuid command)
        {
            return Result.Success();
        }
    }
}
