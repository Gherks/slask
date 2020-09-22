using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class AddScoreToPlayerInMatch : CommandInterface
    {
    }

    public sealed class AddScoreToPlayerInMatchHandler : CommandHandlerInterface<AddScoreToPlayerInMatch>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public AddScoreToPlayerInMatchHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(AddScoreToPlayerInMatch command)
        {
            return Result.Success();
        }
    }
}
