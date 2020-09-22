using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class RenameTournament : CommandInterface
    {
    }

    public sealed class RenameTournamentHandler : CommandHandlerInterface<RenameTournament>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public RenameTournamentHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(RenameTournament command)
        {
            return Result.Success();
        }
    }
}
