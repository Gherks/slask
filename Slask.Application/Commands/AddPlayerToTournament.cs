using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using System;

namespace Slask.Application.Commands
{
    public sealed class AddPlayerToTournament : CommandInterface
    {
        public Guid TournamentId { get; }
        public string PlayerName { get; }

        public AddPlayerToTournament(Guid tournamentId, string playerName)
        {
            TournamentId = tournamentId;
            PlayerName = playerName;
        }
    }

    public sealed class AddPlayerToTournamentHandler : CommandHandlerInterface<AddPlayerToTournament>
    {
        private readonly TournamentRepositoryInterface tournamentRepository;

        public AddPlayerToTournamentHandler(TournamentRepositoryInterface tournamentRepository)
        {
            tournamentRepository = tournamentRepository;
        }

        public Result Handle(AddPlayerToTournament command)
        {
            Tournament tournament = tournamentRepository.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not add new player ({ command.PlayerName }) to tournament. Tournament ({ command.TournamentId }) not found.");
            }

            PlayerReference playerReference = tournamentRepository.AddPlayerReference(tournament, command.PlayerName);

            if (playerReference == null)
            {
                return Result.Failure($"Could not add new player ({ command.PlayerName }) to tournament.");
            }

            tournamentRepository.Save();
            return Result.Success();
        }
    }
}
