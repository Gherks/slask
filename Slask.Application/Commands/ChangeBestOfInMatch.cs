using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class ChangeBestOfInMatch : CommandInterface
    {
    }

    public sealed class ChangeBestOfInMatchHandler : CommandHandlerInterface<ChangeBestOfInMatch>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public ChangeBestOfInMatchHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(ChangeBestOfInMatch command)
        {
            return Result.Success();
        }
    }
}
