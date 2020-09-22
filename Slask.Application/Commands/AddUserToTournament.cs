using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class AddUserToTournament : CommandInterface
    {
    }

    public sealed class AddUserToTournamentHandler : CommandHandlerInterface<AddUserToTournament>
    { 
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public AddUserToTournamentHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(AddUserToTournament command)
        {
            return Result.Success();
        }
    }
}
