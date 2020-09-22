using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Persistence.Services;
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
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public AddBetterToTournamentHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(AddBetterToTournament command)
        {
            Tournament tournament = _tournamentService.GetTournamentById(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not add better to tournament. Tournament ({ command.TournamentId }) not found.");
            }

            User user = _userService.GetUserById(command.UserId);

            if (user == null)
            {
                return Result.Failure($"Could not add better to tournament with given user ({ command.UserId }). Tournament ({ command.TournamentId }) not found.");
            }

            Better better = _tournamentService.AddBetterToTournament(tournament, user);

            if (better == null)
            {
                return Result.Failure($"Could not add better to tournament with given user ({ command.UserId }).");
            }

            _tournamentService.Save();
            return Result.Success();
        }
    }
}
