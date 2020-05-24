namespace Slask.Domain.Utilities
{
    public class TournamentIssue : TournamentIssueInterface
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

        public bool IsTournamentIssue()
        {
            bool isNotRoundIssue = Round == -1;

            return isNotRoundIssue;
        }

        public bool IsRoundIssue()
        {
            bool isRoundIssue = Round > -1;
            bool isNotGroupIssue = Group == -1;

            return isRoundIssue && isNotGroupIssue;
        }

        public bool IsGroupIssue()
        {
            bool isGroupIssue = Group > -1;
            bool isNotMatchIssue = Match == -1;

            return isGroupIssue && isNotMatchIssue;
        }

        public bool IsMatchIssue()
        {
            bool isMatchIssue = Match > -1;

            return isMatchIssue;
        }
    }
}
