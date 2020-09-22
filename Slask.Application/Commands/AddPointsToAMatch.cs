using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class AddPointsToAMatch : CommandInterface
    {
    }

    public sealed class AddPointsToAMatchHandler : CommandHandlerInterface<AddPointsToAMatch>
    { 
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public AddPointsToAMatchHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(AddPointsToAMatch command)
        {
            return Result.Success();
        }
    }
}
