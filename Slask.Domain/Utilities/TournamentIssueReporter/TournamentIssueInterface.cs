using System;
using System.Collections.Generic;

namespace Slask.Domain.Utilities
{
    public interface TournamentIssueInterface
    {
        public int Round { get; }
        public int Group { get; }
        public int Match { get; }
        public string Description { get; }

        public bool IsTournamentIssue();
        public bool IsRoundIssue();
        public bool IsGroupIssue();
        public bool IsMatchIssue();
    }
}
