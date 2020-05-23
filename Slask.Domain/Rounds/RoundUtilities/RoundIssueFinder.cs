using Slask.Domain.Rounds.Bases;
using System;

namespace Slask.Domain.Rounds.RoundUtilities
{
    public static class RoundIssueFinder
    {
        public static void FindIssues(RoundBase round)
        {
            CheckWhetherRoundIsFilledUpToCapacityWithPlayers(round);
            CheckWhetherAdvancingAmountIsEqualOrGreaterThanPlayersPerGroup(round);

            if (round.IsLastRound())
            {
                CheckWhetherLastRoundContainsMoreThanOneGroup(round);
                CheckWhetherLastRoundHasMoreThanOneAdvancingPerGroupCount(round);
            }
        }

        private static void CheckWhetherRoundIsFilledUpToCapacityWithPlayers(RoundBase round)
        {
            int expectedParticipantCount = round.GetExpectedParticipantCount();
            int playerCapacityInRound = round.Groups.Count * round.PlayersPerGroupCount;

            bool doesNotFillAllGroupsEvenly = (expectedParticipantCount - playerCapacityInRound) != 0;

            if (doesNotFillAllGroupsEvenly)
            {
                string description;

                if (round.IsFirstRound())
                {
                    description = "Current player count does not fill all group(s) to capacity. Add more players or reduce group capacity.";
                }
                else
                {
                    description = "Round does not synergize with previous round. Advancing players from previous round will not fill the groups within the current round to capacity.";
                }

                round.Tournament.TournamentIssueReporter.Report(round, description);
            }
        }

        private static void CheckWhetherAdvancingAmountIsEqualOrGreaterThanPlayersPerGroup(RoundBase round)
        {
            bool advancingCountIsEqualOrGreaterThanPlayersPerGroupCount = round.AdvancingPerGroupCount >= round.PlayersPerGroupCount;

            if (advancingCountIsEqualOrGreaterThanPlayersPerGroupCount)
            {
                string description = "Round can't have advancing per group count equal or greather than players per group count.";
                round.Tournament.TournamentIssueReporter.Report(round, description);
            }
        }

        private static void CheckWhetherLastRoundContainsMoreThanOneGroup(RoundBase round)
        {
            bool lastRoundHasSeveralGroups = round.Groups.Count > 1;

            if (lastRoundHasSeveralGroups)
            {
                string description = "Last round should not contain more than one group. Increase group capacity until all players will fit into one group.";
                round.Tournament.TournamentIssueReporter.Report(round, description);
            }
        }

        private static void CheckWhetherLastRoundHasMoreThanOneAdvancingPerGroupCount(RoundBase round)
        {
            bool lastRoundHasSeveralAdvancingPlayers = round.AdvancingPerGroupCount > 1;

            if (lastRoundHasSeveralAdvancingPlayers)
            {
                string description = "Last round should not have more than one player that advances.";
                round.Tournament.TournamentIssueReporter.Report(round, description);
            }
        }
    }
}
