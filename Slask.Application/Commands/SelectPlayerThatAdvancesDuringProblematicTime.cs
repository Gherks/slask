using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using Slask.Domain.Groups;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Application.Commands
{
    public sealed class SelectPlayerThatAdvancesDuringProblematicTime : CommandInterface
    {
        public Guid TournamentId { get; }
        public Guid GroupId { get; }
        public string PlayerName { get; }

        public SelectPlayerThatAdvancesDuringProblematicTime(Guid tournamentId)
        {
            TournamentId = tournamentId;
        }
    }

    public sealed class SelectPlayerThatAdvancesDuringProblematicTimeHandler : CommandHandlerInterface<SelectPlayerThatAdvancesDuringProblematicTime>
    {
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public SelectPlayerThatAdvancesDuringProblematicTimeHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(SelectPlayerThatAdvancesDuringProblematicTime command)
        {
            Tournament tournament = _tournamentRepository.GetTournament(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not select advancing player ({ command.PlayerName }) in group ({ command.GroupId }) during problematic tie. Tournament ({ command.TournamentId }) not found.");
            }

            GroupBase group = tournament.GetGroupById(command.GroupId);

            if (group == null)
            {
                return Result.Failure($"Could not select advancing player ({ command.PlayerName }) in group ({ command.GroupId }) during problematic tie. Group not found.");
            }

            List<PlayerReference> playerReferences = group.GetPlayerReferences();
            PlayerReference playerReference = playerReferences.FirstOrDefault(playerReference => playerReference.Name == command.PlayerName);

            if (playerReference == null)
            {
                return Result.Failure($"Could not select advancing player ({ command.PlayerName }) in group ({ command.GroupId }) during problematic tie. Player not found.");
            }

            bool playerChosenSuccessfully = _tournamentRepository.SolveTieByChoosingPlayerInGroup(group, playerReference);

            if (!playerChosenSuccessfully)
            {
                return Result.Failure($"Could not select advancing player ({ command.PlayerName }) in group ({ command.GroupId }) during problematic tie.");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
