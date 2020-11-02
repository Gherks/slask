using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using System;

namespace Slask.Application.Commands
{
    public sealed class AddBetterToTournament : CommandInterface
    {
        public Guid TournamentId { get; }
        public Guid UserId { get; }

        public AddBetterToTournament(Guid tournamentId, Guid userId)
        {
            TournamentId = tournamentId;
            UserId = userId;
        }
    }

    public sealed class AddBetterToTournamentHandler : CommandHandlerInterface<AddBetterToTournament>
    {
        private readonly UserRepositoryInterface _userRepository;
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public AddBetterToTournamentHandler(
                UserRepositoryInterface userRepository,
                TournamentRepositoryInterface tournamentRepository)
        {
            _userRepository = userRepository;
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(AddBetterToTournament command)
        {
            Tournament tournament = _tournamentRepository.GetTournament(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not add better to tournament. Tournament ({ command.TournamentId }) not found.");
            }

            User user = _userRepository.GetUserById(command.UserId);

            if (user == null)
            {
                return Result.Failure($"Could not add better to tournament with given user ({ command.UserId }). Tournament ({ command.TournamentId }) not found.");
            }

            Better better = _tournamentRepository.AddBetterToTournament(tournament, user);

            if (better == null)
            {
                return Result.Failure($"Could not add better to tournament with given user ({ command.UserId }).");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
