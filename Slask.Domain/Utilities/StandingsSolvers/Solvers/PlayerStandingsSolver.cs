using Slask.Domain.Groups;
using System;
using System.Collections.Generic;

namespace Slask.Domain.Utilities.StandingsSolvers
{
    public class PlayerStandingsSolver : StandingsSolverBase<GroupBase, PlayerReference>
    {
        protected override List<StandingsEntry<PlayerReference>> CreateStandingsList(GroupBase group)
        {
            List<StandingsEntry<PlayerReference>> playerStandings = new List<StandingsEntry<PlayerReference>>();
            List<PlayerReference> playerReferences = group.GetPlayerReferences();

            foreach (PlayerReference participant in playerReferences)
            {
                playerStandings.Add(StandingsEntry<PlayerReference>.Create(participant));
            }

            return playerStandings;
        }

        protected override void AggregatePointsForStandingEntries(GroupBase group, List<StandingsEntry<PlayerReference>> playerStandings)
        {
            foreach (Match match in group.Matches)
            {
                Player winner = match.GetWinningPlayer();

                if (winner == null)
                {
                    continue;
                }

                string winnerName = winner.GetName();

                StandingsEntry<PlayerReference> playerStandingEntry = playerStandings.Find(player => player.Object.Name == winnerName);

                if (playerStandingEntry == null)
                {
                    // LOG Error: Failed to find player reference when calculating player standings for some reason
                    throw new Exception("Failed to find player reference when calculating player standings for some reason");
                }

                playerStandingEntry.AddPoint();
            }
        }
    }
}
