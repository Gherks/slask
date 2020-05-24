using Slask.Domain;
using Slask.Domain.Groups.Bases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain.Utilities
{
    public static class PlayerStandingsSolver
    {
        public static List<PlayerStandingEntry> FetchFrom(GroupBase group)
        {
            List<PlayerStandingEntry> playerStandings = CreateStandingsList(group);

            AggregateWinsForPlayerStandingEntries(playerStandings, group);

            return playerStandings.OrderByDescending(player => player.Wins).ToList();
        }

        private static List<PlayerStandingEntry> CreateStandingsList(GroupBase group)
        {
            List<PlayerStandingEntry> playerStandings = new List<PlayerStandingEntry>();

            foreach (PlayerReference participant in group.PlayerReferences)
            {
                playerStandings.Add(PlayerStandingEntry.Create(participant));
            }

            return playerStandings;
        }

        private static void AggregateWinsForPlayerStandingEntries(List<PlayerStandingEntry> playerStandings, GroupBase group)
        {
            foreach (Match match in group.Matches)
            {
                Player winner = match.GetWinningPlayer();

                if (winner == null)
                {
                    continue;
                }

                PlayerStandingEntry playerStandingEntry = playerStandings.Find(player => player.PlayerReference.Name == winner.PlayerReference.Name);

                if (playerStandingEntry == null)
                {
                    // LOG Error: Failed to find player reference when calculating player standings for some reason
                    throw new Exception("Failed to find player reference when calculating player standings for some reason");
                }

                playerStandingEntry.AddWin();
            }
        }
    }
}
