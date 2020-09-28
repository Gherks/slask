using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;

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
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public CreateTournamentHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(CreateTournament command)
        {
            Tournament tournament = _tournamentRepository.CreateTournament(command.Name);

            if (tournament == null)
            {
                return Result.Failure($"Could not create tournament ({command.Name})");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
