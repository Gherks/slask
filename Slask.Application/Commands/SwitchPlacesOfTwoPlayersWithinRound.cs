using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class SwitchPlacesOfTwoPlayersWithinRound : CommandInterface
    {
    }

    public sealed class SwitchPlacesOfTwoPlayersWithinRoundHandler : CommandHandlerInterface<SwitchPlacesOfTwoPlayersWithinRound>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public SwitchPlacesOfTwoPlayersWithinRoundHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(SwitchPlacesOfTwoPlayersWithinRound command)
        {
            return Result.Success();
        }
    }
}
