using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class RenameRoundInTournament : CommandInterface
    {
    }

    public sealed class RenameRoundInTournamentHandler : CommandHandlerInterface<RenameRoundInTournament>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public RenameRoundInTournamentHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(RenameRoundInTournament command)
        {
            return Result.Success();
        }
    }
}
