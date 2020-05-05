using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.MatchTests.StartDateTimeTests
{
    public class DualTournamentStartDateTimeTests
    {
        private readonly Tournament tournament;
        private readonly TournamentIssueReporter tournamentIssueReporter;
        private readonly DualTournamentRound dualTournamentRound;

        public DualTournamentStartDateTimeTests()
        {
            tournament = Tournament.Create("GSL 2019");
            tournamentIssueReporter = tournament.TournamentIssueReporter;
            dualTournamentRound = tournament.AddDualTournamentRound("Dual tournament round", 3) as DualTournamentRound;
            tournament.AddBracketRound("Bracket round", 3);
        }

        [Fact]
        public void StartDateTimeForMatchesMustBeInMatchOrder()
        {
            DualTournamentGroup dualTournamentGroup = RegisterPlayers(new List<string> { "Maru", "Stork", "Taeja", "Rain" });

            List<DateTime> dateTimesBeforeChange = new List<DateTime>();

            foreach (Match match in dualTournamentGroup.Matches)
            {
                dateTimesBeforeChange.Add(match.StartDateTime);
            }

            DateTime oneHourLater = SystemTime.Now.AddHours(1);
            DateTime twoHoursLater = SystemTime.Now.AddHours(2);
            DateTime threeHoursLater = SystemTime.Now.AddHours(3);
            DateTime fourHoursLater = SystemTime.Now.AddHours(4);
            DateTime fiveHoursLater = SystemTime.Now.AddHours(5);

            dualTournamentGroup.Matches[0].SetStartDateTime(twoHoursLater); // IS GOOD
            dualTournamentGroup.Matches[1].SetStartDateTime(fiveHoursLater); // IS GOOD
            dualTournamentGroup.Matches[2].SetStartDateTime(fourHoursLater); // IS BAD
            dualTournamentGroup.Matches[3].SetStartDateTime(threeHoursLater); // IS BAD
            dualTournamentGroup.Matches[4].SetStartDateTime(oneHourLater); // IS BAD

            dualTournamentGroup.Matches[0].StartDateTime.Should().Be(twoHoursLater);
            dualTournamentGroup.Matches[1].StartDateTime.Should().Be(fiveHoursLater);
            dualTournamentGroup.Matches[2].StartDateTime.Should().Be(fourHoursLater);
            dualTournamentGroup.Matches[3].StartDateTime.Should().Be(threeHoursLater);
            dualTournamentGroup.Matches[4].StartDateTime.Should().Be(oneHourLater);

            tournamentIssueReporter.Issues.Should().HaveCount(3);

            ConfirmIssueIsAsExpected(tournamentIssueReporter.Issues[0], 0, 0, 2);
            ConfirmIssueIsAsExpected(tournamentIssueReporter.Issues[1], 0, 0, 3);
            ConfirmIssueIsAsExpected(tournamentIssueReporter.Issues[2], 0, 0, 4);
        }

        private DualTournamentGroup RegisterPlayers(List<string> playerNames)
        {
            foreach (string playerName in playerNames)
            {
                dualTournamentRound.RegisterPlayerReference(playerName);
            }

            return dualTournamentRound.Groups.First() as DualTournamentGroup;
        }

        private void ConfirmIssueIsAsExpected(TournamentIssue issue, int roundIndex, int groupIndex, int matchIndex)
        {
            issue.Round.Should().Be(roundIndex);
            issue.Group.Should().Be(groupIndex);
            issue.Match.Should().Be(matchIndex);
        }
    }
}
