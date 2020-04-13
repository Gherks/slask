using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slask.Domain.Groups
{
    // CREATE TESTS
    public static class PlayerStandingsCalculator
    {
        public static List<PlayerReference> GetAdvancingPlayers(RoundBase round)
        {
            List<PlayerReference> advancingPlayers = new List<PlayerReference>();

            foreach (GroupBase group in round.Groups)
            {
                advancingPlayers.AddRange(PlayerStandingsCalculator.GetAdvancingPlayers(group));
            }

            return advancingPlayers;
        }

        public static List<PlayerReference> GetAdvancingPlayers(GroupBase group)
        {
            if (group.GetPlayState() == PlayState.IsFinished)
            {
                List<PlayerStandingEntry> playerStandings = GetPlayerStandings(group);

                return FilterAdvancingPlayers(group, playerStandings);
            }

            return new List<PlayerReference>();
        }

        public static List<PlayerStandingEntry> GetPlayerStandings(GroupBase group)
        {
            List<PlayerStandingEntry> playerStandings = CalculatePlayerStandings(group);

            return playerStandings.OrderByDescending(player => player.Wins).ToList();
        }

        private static List<PlayerStandingEntry> CalculatePlayerStandings(GroupBase group)
        {
            List<PlayerStandingEntry> playerStandings = new List<PlayerStandingEntry>();

            foreach (PlayerReference participant in group.GetPlayerReferences())
            {
                playerStandings.Add(PlayerStandingEntry.Create(participant));
            }

            foreach (Match match in group.Matches)
            {
                PlayerReference winner = match.GetWinningPlayer().PlayerReference;
                PlayerStandingEntry playerStandingEntry = playerStandings.Find(player => player.PlayerReference.Name == winner.Name);

                if (playerStandingEntry == null)
                {
                    // LOG Error: Failed to find player reference when calculating player standings for some reason
                    throw new Exception("Failed to find player reference when calculating player standings for some reason");
                }

                playerStandingEntry.AddWin();
            }

            return playerStandings;
        }

        private static List<PlayerReference> FilterAdvancingPlayers(GroupBase group, List<PlayerStandingEntry> playerStandings)
        {
            List<PlayerReference> advancingPlayers = new List<PlayerReference>();

            for (int index = 0; index < group.Round.AdvancingPerGroupCount; ++index)
            {
                advancingPlayers.Add(playerStandings[index].PlayerReference);
            }

            return advancingPlayers;
        }
    }
}
