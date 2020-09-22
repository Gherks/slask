using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class BetterPlaceBetOnMatch : CommandInterface
    {
    }

    public sealed class BetterPlaceBetOnMatchHandler : CommandHandlerInterface<BetterPlaceBetOnMatch>
    { 
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public BetterPlaceBetOnMatchHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(BetterPlaceBetOnMatch command)
        {
            return Result.Success();
        }
    }
}
