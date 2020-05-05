using Slask.Common;
using Slask.Domain.Rounds.Bases;
using System;

namespace Slask.Domain.Utilities
{
    public static class MatchStartDateTimeValidator
    {
        public static bool Validate(Match match, DateTime newStartDateTime)
        {
            bool newStartDateTimeIsInThePast = !ConfirmNewStartDateTimeIsFutureDateTime(match, newStartDateTime);

            if (newStartDateTimeIsInThePast)
            {
                return false;
            }

            ConfirmNewStartDateTimeIsAfterAnyMatchInPreviousRound(match, newStartDateTime);
            ConfirmNewStartDateTimeCompliesWithGroupRules(match, newStartDateTime);

            return true;
        }

        private static bool ConfirmNewStartDateTimeIsFutureDateTime(Match match, DateTime newStartDateTime)
        {
            bool newStartDateTimeIsInThePast = newStartDateTime < SystemTime.Now;

            if (newStartDateTimeIsInThePast)
            {
                TournamentIssueReporter tournamentIssueReporter = match.Group.Round.Tournament.TournamentIssueReporter;
                tournamentIssueReporter.Report(match, "Start date time must be a future date time");
                return false;
            }

            return true;
        }

        private static void ConfirmNewStartDateTimeIsAfterAnyMatchInPreviousRound(Match match, DateTime newStartDateTime)
        {
            RoundBase previousRound = match.Group.Round.GetPreviousRound();
            bool previousRoundExist = previousRound != null;

            if (previousRoundExist)
            {
                bool newStartDateTimeIsEarlierThanMatchInPreviousRound = newStartDateTime < previousRound.GetLastMatch().StartDateTime;

                if (newStartDateTimeIsEarlierThanMatchInPreviousRound)
                {
                    TournamentIssueReporter tournamentIssueReporter = match.Group.Round.Tournament.TournamentIssueReporter;
                    tournamentIssueReporter.Report(match, "Start date time can't be earlier than any match in previous round.");
                }
            }
        }

        private static void ConfirmNewStartDateTimeCompliesWithGroupRules(Match match, DateTime newStartDateTime)
        {
            bool newStartDateTimeDoesNotComplyWithGroupRules = !match.Group.NewDateTimeIsValid(match, newStartDateTime);

            if (newStartDateTimeDoesNotComplyWithGroupRules)
            {
                TournamentIssueReporter tournamentIssueReporter = match.Group.Round.Tournament.TournamentIssueReporter;
                tournamentIssueReporter.Report(match, "Start date time does not work with group rules");
            }
        }
    }
}
