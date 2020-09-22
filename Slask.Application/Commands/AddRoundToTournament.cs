using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class AddRoundToTournament : CommandInterface
    {
    }

    public sealed class AddRoundToTournamentHandler : CommandHandlerInterface<AddRoundToTournament>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public AddRoundToTournamentHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(AddRoundToTournament command)
        {
            return Result.Success();
        }
    }
}
