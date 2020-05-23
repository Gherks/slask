using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds.Bases;
using Slask.Domain.Utilities;
using Slask.Utilities.PlayerStandingsSolver;
using System;
using System.Collections.Generic;

namespace Slask.Domain.Groups.GroupUtility
{
    public static class AdvancingPlayersSolver
    {
        public static List<PlayerReference> FetchFrom(RoundBase round)
        {
            List<PlayerReference> advancingPlayers = new List<PlayerReference>();

            if (round.GetPlayState() == PlayState.Finished)
            {
                foreach (GroupBase group in round.Groups)
                {
                    advancingPlayers.AddRange(AdvancingPlayersSolver.FetchFrom(group));
                }
            }

            return advancingPlayers;
        }

        public static List<PlayerReference> FetchFrom(GroupBase group)
        {
            if (group.GetPlayState() == PlayState.Finished)
            {
                List<PlayerStandingEntry> playerStandings = PlayerStandingsSolver.FetchFrom(group);
                playerStandings = FilterAdvancingPlayers(group, playerStandings);

                if (group.HasProblematicTie())
                {
                    playerStandings = FilterTyingPlayers(group, playerStandings);
                    playerStandings.AddRange(group.ChoosenTyingPlayerEntries);
                }

                List<PlayerReference> advancingPlayers = new List<PlayerReference>();

                foreach (PlayerStandingEntry entry in playerStandings)
                {
                    advancingPlayers.Add(entry.PlayerReference);
                }

                return advancingPlayers;
            }

            return new List<PlayerReference>();
        }

        private static List<PlayerStandingEntry> FilterAdvancingPlayers(GroupBase group, List<PlayerStandingEntry> playerStandings)
        {
            List<PlayerStandingEntry> nonFilteredPlayers = new List<PlayerStandingEntry>();

            for (int index = 0; index < group.Round.AdvancingPerGroupCount; ++index)
            {
                nonFilteredPlayers.Add(playerStandings[index]);
            }

            return nonFilteredPlayers;
        }

        private static List<PlayerStandingEntry> FilterTyingPlayers(GroupBase group, List<PlayerStandingEntry> playerStandings)
        {
            List<PlayerStandingEntry> nonFilteredPlayers = new List<PlayerStandingEntry>();

            foreach(PlayerStandingEntry entry in group.FindProblematiclyTyingPlayers())
            {
                nonFilteredPlayers.Remove(entry);
            }

            return nonFilteredPlayers;
        }
    }
}
