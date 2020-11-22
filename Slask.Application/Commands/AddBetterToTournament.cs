using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using System;

namespace Slask.Application.Commands
{
    public sealed class AddBetterToTournament : CommandInterface
    {
        public string TournamentIdentifier { get; }
        public string UserIdentifier { get; }

        public AddBetterToTournament(string tournamentIdentifier, string userIdentifier)
        {
            TournamentIdentifier = tournamentIdentifier;
            UserIdentifier = userIdentifier;
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
            Tournament tournament = CommandQueryUtilities.GetTournamentByIdentifier(_tournamentRepository, command.TournamentIdentifier);

            if (tournament == null)
            {
                return Result.Failure($"Could not add better to tournament. Tournament ({ command.TournamentIdentifier }) not found.");
            }

            User user = CommandQueryUtilities.GetUserByIdentifier(_userRepository, command.UserIdentifier);

            if (user == null)
            {
                return Result.Failure($"Could not add better to tournament with given user ({ command.UserIdentifier }). Tournament ({ command.TournamentIdentifier }) not found.");
            }

            Better better = _tournamentRepository.AddBetterToTournament(tournament, user);

            if (better == null)
            {
                return Result.Failure($"Could not add better to tournament with given user ({ command.UserIdentifier }).");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
