using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class AddPlayerToTournamentByName : CommandInterface
    {
        public string TournamentName { get; }
        public string PlayerName { get; }

        public AddPlayerToTournamentByName(string tournamentName, string playerName)
        {
            TournamentName = tournamentName;
            PlayerName = playerName;
        }
    }

    public sealed class AddPlayerToTournamentByNameHandler : CommandHandlerInterface<AddPlayerToTournamentByName>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public AddPlayerToTournamentByNameHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public Result Handle(AddPlayerToTournamentByName command)
        {
            Tournament tournament = _tournamentService.GetTournamentByName(command.TournamentName);

            if (tournament == null)
            {
                return Result.Failure($"Could not add new player ({ command.PlayerName }) to tournament. Tournament ({ command.TournamentName }) not found.");
            }

            PlayerReference playerReference = _tournamentService.AddPlayerReference(tournament, command.PlayerName);

            if (playerReference == null)
            {
                return Result.Failure($"Could not add new player ({ command.PlayerName }) to tournament.");
            }

            return Result.Success();
        }
    }
}
