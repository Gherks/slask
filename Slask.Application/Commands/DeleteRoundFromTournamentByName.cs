using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class DeleteRoundFromTournamentByName : CommandInterface
    {
    }

    public sealed class DeleteRoundFromTournamentByNameHandler : CommandHandlerInterface<DeleteRoundFromTournamentByName>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public DeleteRoundFromTournamentByNameHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(DeleteRoundFromTournamentByName command)
        {
            return Result.Success();
        }
    }
}
