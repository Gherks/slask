using Slask.Domain.Groups;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds.Bases;
using Slask.Domain.Utilities;
using Slask.Utilities.PlayerStandingsSolver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain.Rounds
{
    public class RoundRobinRound : ResizableRound
    {
        private RoundRobinRound()
        {
        }

        public static RoundRobinRound Create(Tournament tournament)
        {
            bool givenTournamentIsInvalid = tournament == null;

            if (givenTournamentIsInvalid)
            {
                return null;
            }

            RoundRobinRound round = new RoundRobinRound
            {
                Id = Guid.NewGuid(),
                PlayersPerGroupCount = 2,
                BestOf = 3,
                AdvancingPerGroupCount = 1,
                TournamentId = tournament.Id,
                Tournament = tournament
            };

            round.AssignDefaultName();

            return round;
        }

        public bool HasProblematicTie()
        {
            bool isFinished = GetPlayState() == PlayState.Finished;

            if (isFinished)
            {
                foreach (GroupBase group in Groups)
                {
                    List<PlayerStandingEntry> playerStandings = PlayerStandingsSolver.FetchFrom(group);

                    PlayerStandingEntry playerJustAboveAdvancingThreshold = playerStandings[AdvancingPerGroupCount - 1];
                    PlayerStandingEntry playerJustBelowAdvancingThreshold = playerStandings[AdvancingPerGroupCount];

                    bool hasProblematicTieInGroup = playerJustAboveAdvancingThreshold.Wins == playerJustBelowAdvancingThreshold.Wins;

                    return hasProblematicTieInGroup;
                }
            }

            return false;
        }

        protected override GroupBase AddGroup()
        {
            return RoundRobinGroup.Create(this);
        }
    }
}
