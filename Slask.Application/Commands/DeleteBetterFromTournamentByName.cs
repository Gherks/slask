using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class DeleteBetterFromTournamentByName : CommandInterface
    {
    }

    public sealed class DeleteBetterFromTournamentByNameHandler : CommandHandlerInterface<DeleteBetterFromTournamentByName>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public DeleteBetterFromTournamentByNameHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(DeleteBetterFromTournamentByName command)
        {
            return Result.Success();
        }
    }
}
