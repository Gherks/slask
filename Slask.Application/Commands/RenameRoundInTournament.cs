using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class RenameRoundInTournament : CommandInterface
    {
        public Guid TournamentId { get; }

        public RenameRoundInTournament(Guid tournamentId)
        {
            TournamentId = tournamentId;
        }
    }

    public sealed class RenameRoundInTournamentHandler : CommandHandlerInterface<RenameRoundInTournament>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public RenameRoundInTournamentHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(RenameRoundInTournament command)
        {
            _tournamentService.Save();
            return Result.Success();
        }
    }
}
