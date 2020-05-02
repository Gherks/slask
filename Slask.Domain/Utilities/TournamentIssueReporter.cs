using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Slask.Domain.Utilities
{
    public class TournamentIssueReporter
    {
        public TournamentIssueReporter()
        {
            Issues = new List<TournamentIssue>();
        }

        public List<TournamentIssue> Issues { get; private set; }

        public void Report(Tournament tournament, string description)
        {
            Issues.Add(TournamentIssue.Create(-1, -1, -1, description));
        }

        public void Report(RoundBase round, string description)
        {
            int roundIndex = GetIndexOf(round);

            Issues.Add(TournamentIssue.Create(roundIndex, -1, -1, description));
        }

        public void Report(GroupBase group, string description)
        {
            int roundIndex = GetIndexOf(group.Round);
            int groupIndex = GetIndexOf(group);

            Issues.Add(TournamentIssue.Create(roundIndex, groupIndex, -1, description));
        }

        public void Report(Match match, string description)
        {
            int roundIndex = GetIndexOf(match.Group.Round);
            int groupIndex = GetIndexOf(match.Group);
            int matchIndex = GetIndexOf(match);

            Issues.Add(TournamentIssue.Create(roundIndex, groupIndex, matchIndex, description));
        }

        public void Clear()
        {
            Issues.Clear();
        }

        int GetIndexOf(RoundBase round)
        {
            for (int index = 0; index < round.Tournament.Rounds.Count; ++index)
            {
                if (round.Id == round.Tournament.Rounds[index].Id)
                {
                    return index;
                }
            }

            // LOGG Error: Could not find given round within tournament when reporting issue.
            return -1;
        }

        int GetIndexOf(GroupBase group)
        {
            for (int index = 0; index < group.Round.Groups.Count; ++index)
            {
                if (group.Id == group.Round.Groups[index].Id)
                {
                    return index;
                }
            }

            // LOGG Error: Could not find given group within tournament when reporting issue.
            return -1;
        }

        int GetIndexOf(Match match)
        {
            for (int index = 0; index < match.Group.Matches.Count; ++index)
            {
                if (match.Id == match.Group.Matches[index].Id)
                {
                    return index;
                }
            }

            // LOGG Error: Could not find given match within tournament when reporting issue.
            return -1;
        }
    }
}
