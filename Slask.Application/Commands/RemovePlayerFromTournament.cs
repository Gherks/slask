using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using System;

namespace Slask.Application.Commands
{
    public sealed class RemovePlayerFromTournament : CommandInterface
    {
        public Guid TournamentId { get; }
        public string PlayerName { get; }

        public RemovePlayerFromTournament(Guid tournamentId, string playerName)
        {
            TournamentId = tournamentId;
            PlayerName = playerName;
        }
    }

    public sealed class RemovePlayerFromTournamentHandler : CommandHandlerInterface<RemovePlayerFromTournament>
    {
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public RemovePlayerFromTournamentHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(RemovePlayerFromTournament command)
        {
            Tournament tournament = _tournamentRepository.GetTournament(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not player ({ command.PlayerName}) from tournament ({ command.TournamentId }). Tournament not found.");
            }

            bool playerRemoved = _tournamentRepository.RemovePlayerReferenceFromTournament(tournament, command.PlayerName);

            if (!playerRemoved)
            {
                return Result.Failure($"Could not player ({ command.PlayerName}) from tournament ({ command.TournamentId }).");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
