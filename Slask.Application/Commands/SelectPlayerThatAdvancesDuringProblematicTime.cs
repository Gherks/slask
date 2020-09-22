using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Commands
{
    public sealed class SelectPlayerThatAdvancesDuringProblematicTime : CommandInterface
    {
        public Guid TournamentId { get; }

        public SelectPlayerThatAdvancesDuringProblematicTime(Guid tournamentId)
        {
            TournamentId = tournamentId;
        }
    }

    public sealed class SelectPlayerThatAdvancesDuringProblematicTimeHandler : CommandHandlerInterface<SelectPlayerThatAdvancesDuringProblematicTime>
    {
        private readonly UserServiceInterface _userService;
        private readonly TournamentServiceInterface _tournamentService;

        public SelectPlayerThatAdvancesDuringProblematicTimeHandler(
                UserServiceInterface userService,
                TournamentServiceInterface tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public Result Handle(SelectPlayerThatAdvancesDuringProblematicTime command)
        {
            _tournamentService.Save();
            return Result.Success();
        }
    }
}
