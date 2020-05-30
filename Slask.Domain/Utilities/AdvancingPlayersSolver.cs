using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities.StandingsSolvers;
using System.Collections.Generic;

namespace Slask.Domain.Utilities
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
                PlayerStandingsSolver playerStandingsSolver = new PlayerStandingsSolver();
                List<StandingsEntry<PlayerReference>> playerStandings = playerStandingsSolver.FetchFrom(group);
                playerStandings = FilterAdvancingPlayers(group, playerStandings);

                if (group.HasProblematicTie())
                {
                    playerStandings = FilterTyingPlayers(group, playerStandings);
                    playerStandings.AddRange(group.ChoosenTyingPlayerEntries);
                }

                List<PlayerReference> advancingPlayers = new List<PlayerReference>();

                foreach (StandingsEntry<PlayerReference> entry in playerStandings)
                {
                    advancingPlayers.Add(entry.Object);
                }

                return advancingPlayers;
            }

            return new List<PlayerReference>();
        }

        private static List<StandingsEntry<PlayerReference>> FilterAdvancingPlayers(GroupBase group, List<StandingsEntry<PlayerReference>> playerStandings)
        {
            List<StandingsEntry<PlayerReference>> nonFilteredPlayers = new List<StandingsEntry<PlayerReference>>();

            for (int index = 0; index < group.Round.AdvancingPerGroupCount; ++index)
            {
                nonFilteredPlayers.Add(playerStandings[index]);
            }

            return nonFilteredPlayers;
        }

        private static List<StandingsEntry<PlayerReference>> FilterTyingPlayers(GroupBase group, List<StandingsEntry<PlayerReference>> playerStandings)
        {
            List<StandingsEntry<PlayerReference>> nonFilteredPlayers = new List<StandingsEntry<PlayerReference>>();

            foreach (StandingsEntry<PlayerReference> entry in group.FindProblematiclyTyingPlayers())
            {
                nonFilteredPlayers.Remove(entry);
            }

            return nonFilteredPlayers;
        }
    }
}
