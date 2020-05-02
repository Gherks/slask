using System;
using System.Collections.Generic;
using System.Text;

namespace Slask.Domain.Utilities
{
    public class TournamentIssue
    {
        private TournamentIssue()
        {

        }

        public int Round { get; private set; }
        public int Group { get; private set; }
        public int Match { get; private set; }
        public string Description { get; private set; }

        public static TournamentIssue Create(int round, int group, int match, string description)
        {
            return new TournamentIssue()
            {
                Round = round,
                Group = group,
                Match = match,
                Description = description
            };
        }
    }
}
