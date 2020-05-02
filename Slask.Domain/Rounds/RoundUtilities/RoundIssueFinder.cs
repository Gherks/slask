using Slask.Domain.Rounds.Bases;

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
                string description = "Current player count does not fill all group(s) to capacity. Add more players or reduce group capacity.";
                round.Tournament.TournamentIssueReporter.Report(round, description);
            }
        }

        private static void FindIssuesInGeneralRounds(RoundBase round, int participatingPlayersCount)
        {
            int playerCapacityInRound = round.Groups.Count * round.PlayersPerGroupCount;

            bool doesNotFillAllGroupsEvenly = (participatingPlayersCount - playerCapacityInRound) != 0;

            if (doesNotFillAllGroupsEvenly)
            {
                string description = "Round does not synergize with previous round. Advancing players from previous round will not fill the groups within the current round to capacity.";
                round.Tournament.TournamentIssueReporter.Report(round, description);
            }
        }

        private static void FindIssuesInLastRound(RoundBase round, int participatingPlayersCount)
        {
            int playerCapacityInRound = round.Groups.Count * round.PlayersPerGroupCount;

            bool doesNotFillAllGroupsEvenly = (participatingPlayersCount - playerCapacityInRound) != 0;

            if (doesNotFillAllGroupsEvenly)
            {
                string description = "Round does not synergize with previous round. Advancing players from previous round will not fill the groups within the current round to capacity.";
                round.Tournament.TournamentIssueReporter.Report(round, description);
            }

            bool lastRoundHasSeveralGroups = round.Groups.Count > 1;

            if(lastRoundHasSeveralGroups)
            {
                string description = "Last round should not contain more than one group. Increase group capacity until all players will fit into one group.";
                round.Tournament.TournamentIssueReporter.Report(round, description);
            }

            bool lastRoundHasSeveralAdvancingPlayers = round.AdvancingPerGroupCount > 1;

            if (lastRoundHasSeveralAdvancingPlayers)
            {
                string description = "Last round should have more than one player that advances. This value will be set to zero for this round when this tournament is created.";
                round.Tournament.TournamentIssueReporter.Report(round, description);
            }
        }
    }
}
