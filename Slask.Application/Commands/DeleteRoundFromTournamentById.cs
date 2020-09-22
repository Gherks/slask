using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class DeleteRoundFromTournamentById : CommandInterface
    {
    }

    public sealed class DeleteRoundFromTournamentByIdHandler : CommandHandlerInterface<DeleteRoundFromTournamentById>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public DeleteRoundFromTournamentByIdHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(DeleteRoundFromTournamentById command)
        {
            return Result.Success();
        }
    }
}
