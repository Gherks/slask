using Slask.Domain.Rounds.Bases;
using Slask.Domain.Utilities;
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
                if (round.IsFirstRound())
                {
                    round.Tournament.TournamentIssueReporter.Report(round, TournamentIssues.NotFillingAllGroupsWithPlayers);
                }
                else
                {
                    round.Tournament.TournamentIssueReporter.Report(round, TournamentIssues.RoundDoesNotSynergizeWithPreviousRound);
                }
            }
        }

        private static void CheckWhetherAdvancingAmountIsEqualOrGreaterThanPlayersPerGroup(RoundBase round)
        {
            bool advancingCountIsEqualOrGreaterThanPlayersPerGroupCount = round.AdvancingPerGroupCount >= round.PlayersPerGroupCount;

            if (advancingCountIsEqualOrGreaterThanPlayersPerGroupCount)
            {
                round.Tournament.TournamentIssueReporter.Report(round, TournamentIssues.AdvancersCountInRoundIsGreaterThanParticipantCount);
            }
        }

        private static void CheckWhetherLastRoundContainsMoreThanOneGroup(RoundBase round)
        {
            bool lastRoundHasSeveralGroups = round.Groups.Count > 1;

            if (lastRoundHasSeveralGroups)
            {
                round.Tournament.TournamentIssueReporter.Report(round, TournamentIssues.LastRoundContainsMoreThanOneGroup);
            }
        }

        private static void CheckWhetherLastRoundHasMoreThanOneAdvancingPerGroupCount(RoundBase round)
        {
            bool lastRoundHasSeveralAdvancingPlayers = round.AdvancingPerGroupCount > 1;

            if (lastRoundHasSeveralAdvancingPlayers)
            {
                round.Tournament.TournamentIssueReporter.Report(round, TournamentIssues.LastRoundHasMoreThanOneAdvancers);
            }
        }
    }
}
