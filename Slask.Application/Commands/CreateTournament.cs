using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class CreateTournament : CommandInterface
    {
        public string Name { get; }

        public CreateTournament(string name)
        {
            Name = name;
        }
    }

    public sealed class CreateTournamentHandler : CommandHandlerInterface<CreateTournament>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public CreateTournamentHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(CreateTournament command)
        {
            Tournament tournament = _tournamentService.CreateTournament(command.Name);

            if (tournament == null)
            {
                return Result.Failure($"Could not create tournament ({command.Name})");
            }

            _tournamentService.Save();
            return Result.Success();
        }
    }
}
