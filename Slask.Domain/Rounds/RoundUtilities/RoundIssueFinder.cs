namespace Slask.Domain.Rounds.RoundUtilities
{
    public static class RoundIssueFinder
    {
        public static void FindIssues(RoundBase round, int participatingPlayersCount)
        {
            if (round.IsFirstRound())
            {
                FindIssuesInFirstRound(round, participatingPlayersCount);
            }
            else if (round.IsLastRound())
            {
                FindIssuesInLastRound(round, participatingPlayersCount);
            }
            else
            {
                FindIssuesInGeneralRounds(round, participatingPlayersCount);
            }
        }

        private static void FindIssuesInFirstRound(RoundBase round, int participatingPlayersCount)
        {
            int playerCapacityInRound = round.Groups.Count * round.PlayersPerGroupCount;

            bool doesNotFillAllGroupsEvenly = (participatingPlayersCount - playerCapacityInRound) != 0;

            if (doesNotFillAllGroupsEvenly)
            {
                // LOGG Issue: Current player amount does not fill all group(s) to capacity. Add more players or reduce group capacity.
            }
        }

        private static void FindIssuesInGeneralRounds(RoundBase round, int participatingPlayersCount)
        {
            int playerCapacityInRound = round.Groups.Count * round.PlayersPerGroupCount;

            bool doesNotFillAllGroupsEvenly = (participatingPlayersCount - playerCapacityInRound) != 0;

            if (doesNotFillAllGroupsEvenly)
            {
                // LOGG Issue: Round does not synergize with previous round. Advancing players from previous round will not fill the groups within the current round to capacity.
            }
        }

        private static void FindIssuesInLastRound(RoundBase round, int participatingPlayersCount)
        {
            int playerCapacityInRound = round.Groups.Count * round.PlayersPerGroupCount;

            bool doesNotFillAllGroupsEvenly = (participatingPlayersCount - playerCapacityInRound) != 0;

            if (doesNotFillAllGroupsEvenly)
            {
                // LOGG Issue: Round does not synergize with previous round. Advancing players from previous round will not fill the groups within the current round to capacity.
            }

            bool lastRoundHasSeveralGroups = round.Groups.Count > 1;

            if(lastRoundHasSeveralGroups)
            {
                // LOGG Issue: Last round should not contain more than one group. Increase group capacity until all players will fit into one group.
            }

            bool lastRoundHasSeveralAdvancingPlayers = round.AdvancingPerGroupCount > 1;

            if (lastRoundHasSeveralAdvancingPlayers)
            {
                // LOGG Issue: Last round should have more than one player that advances. This value will be set to zero for this round when this tournament is created.
            }
        }
    }
}
