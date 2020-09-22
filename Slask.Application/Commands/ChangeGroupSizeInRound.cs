using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class ChangeGroupSizeInRound : CommandInterface
    {
    }

    public sealed class ChangeGroupSizeInRoundHandler : CommandHandlerInterface<ChangeGroupSizeInRound>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public ChangeGroupSizeInRoundHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(ChangeGroupSizeInRound command)
        {
            return Result.Success();
        }
    }
}
