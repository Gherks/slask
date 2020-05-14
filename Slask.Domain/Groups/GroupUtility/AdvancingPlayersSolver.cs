using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds.Bases;
using Slask.Domain.Utilities;
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

                return FilterAdvancingPlayers(group, playerStandings);
            }

            return new List<PlayerReference>();
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
