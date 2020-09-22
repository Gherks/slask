using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class ChangeAdvancingPerGroupCountInRound : CommandInterface
    {
    }

    public sealed class ChangeAdvancingPerGroupCountInRoundHandler : CommandHandlerInterface<ChangeAdvancingPerGroupCountInRound>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public ChangeAdvancingPerGroupCountInRoundHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(ChangeAdvancingPerGroupCountInRound command)
        {
            return Result.Success();
        }
    }
}
