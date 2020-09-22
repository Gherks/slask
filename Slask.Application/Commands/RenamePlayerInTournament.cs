using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class RenamePlayerInTournament : CommandInterface
    {
    }

    public sealed class RenamePlayerInTournamentHandler : CommandHandlerInterface<RenamePlayerInTournament>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public RenamePlayerInTournamentHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(RenamePlayerInTournament command)
        {
            return Result.Success();
        }
    }
}
