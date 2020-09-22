using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class AddPlayerToTournament : CommandInterface
    {
    }

    public sealed class AddPlayerToTournamentHandler : CommandHandlerInterface<AddPlayerToTournament>
    { 
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public AddPlayerToTournamentHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(AddPlayerToTournament command)
        {
            return Result.Success();
        }
    }
}
