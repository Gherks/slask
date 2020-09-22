using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class ChangeStartTimeOfMatch : CommandInterface
    {
    }

    public sealed class ChangeStartTimeOfMatchHandler : CommandHandlerInterface<ChangeStartTimeOfMatch>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public ChangeStartTimeOfMatchHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(ChangeStartTimeOfMatch command)
        {
            return Result.Success();
        }
    }
}
