using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds.Bases;
using System;
using System.Collections.Generic;

namespace Slask.Domain.Utilities
{
    public class TournamentIssueReporter
    {
        public TournamentIssueReporter()
        {
            Issues = new List<TournamentIssueInterface>();
        }

        public List<TournamentIssueInterface> Issues { get; private set; }

        public void Report(Tournament tournament, TournamentIssues issue)
        {
            string description = GetDescriptionFor(issue);

            Issues.Add(TournamentIssue.Create(-1, -1, -1, description));
        }

        public void Report(RoundBase round, TournamentIssues issue)
        {
            int roundIndex = GetIndexOf(round);
            string description = GetDescriptionFor(issue);

            Issues.Add(TournamentIssue.Create(roundIndex, -1, -1, description));
        }

        public void Report(GroupBase group, TournamentIssues issue)
        {
            int roundIndex = GetIndexOf(group.Round);
            int groupIndex = GetIndexOf(group);
            string description = GetDescriptionFor(issue);

            Issues.Add(TournamentIssue.Create(roundIndex, groupIndex, -1, description));
        }

        public void Report(Match match, TournamentIssues issue)
        {
            int roundIndex = GetIndexOf(match.Group.Round);
            int groupIndex = GetIndexOf(match.Group);
            int matchIndex = GetIndexOf(match);
            string description = GetDescriptionFor(issue);

            Issues.Add(TournamentIssue.Create(roundIndex, groupIndex, matchIndex, description));
        }

        public void Clear()
        {
            Issues.Clear();
        }

        private int GetIndexOf(RoundBase round)
        {
            for (int index = 0; index < round.Tournament.Rounds.Count; ++index)
            {
                if (round.Id == round.Tournament.Rounds[index].Id)
                {
                    return index;
                }
            }

            // LOG Error: Could not find given round within tournament when reporting issue.
            return -1;
        }

        private int GetIndexOf(GroupBase group)
        {
            for (int index = 0; index < group.Round.Groups.Count; ++index)
            {
                if (group.Id == group.Round.Groups[index].Id)
                {
                    return index;
                }
            }

            // LOG Error: Could not find given group within tournament when reporting issue.
            return -1;
        }

        private int GetIndexOf(Match match)
        {
            for (int index = 0; index < match.Group.Matches.Count; ++index)
            {
                if (match.Id == match.Group.Matches[index].Id)
                {
                    return index;
                }
            }

            // LOG Error: Could not find given match within tournament when reporting issue.
            return -1;
        }

        private string GetDescriptionFor(TournamentIssues issue)
        {
            switch (issue)
            {
                case TournamentIssues.NotFillingAllGroupsWithPlayers:
                    return "Current player count does not fill all group(s) to capacity. Add more players or reduce group capacity.";
                case TournamentIssues.RoundDoesNotSynergizeWithPreviousRound:
                    return "Round does not synergize with previous round. Advancing players from previous round will not fill the groups within the current round to capacity.";
                case TournamentIssues.AdvancersCountInRoundIsGreaterThanParticipantCount:
                    return "Round can't have advancing per group count equal or greather than players per group count.";
                case TournamentIssues.LastRoundContainsMoreThanOneGroup:
                    return "Last round should not contain more than one group. Increase group capacity until all players will fit into one group.";
                case TournamentIssues.LastRoundHasMoreThanOneAdvancers:
                    return "Last round should not have more than one player that advances.";
                case TournamentIssues.StartDateTimeIsInThePast:
                    return "Start date time must be a future date";
                case TournamentIssues.StartDateTimeIncompatibleWithPreviousRound:
                    return "Start date time can't be earlier than any match in previous round.";
                case TournamentIssues.StartDateTimeIncompatibleWithGroupRules:
                    return "Start date time does not work with group rules";
            }

            // LOG Error: Invalid issue given when fetching issue description
            return "N/A";
        }
    }
}
