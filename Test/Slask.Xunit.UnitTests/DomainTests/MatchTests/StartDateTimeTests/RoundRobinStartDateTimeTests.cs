using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Xunit.UnitTests.DomainTests.MatchTests.StartDateTimeTests
{
    public class RoundRobinStartDateTimeTests
    {
        private readonly Tournament tournament;
        private readonly TournamentIssueReporter tournamentIssueReporter;
        private readonly RoundRobinRound roundRobinRound;

        public RoundRobinStartDateTimeTests()
        {
            tournament = Tournament.Create("GSL 2019");
            tournamentIssueReporter = tournament.TournamentIssueReporter;
            roundRobinRound = tournament.AddRoundRobinRound() as RoundRobinRound;
            tournament.AddBracketRound();
        }

        [Fact]
        public void NoStartDateTimeRestrictionIsAppliedToMatches()
        {
            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain" };

            roundRobinRound.SetPlayersPerGroupCount(playerNames.Count);
            RoundRobinGroup roundRobinGroup = RegisterPlayers(playerNames);

            DateTime twoHoursLater = SystemTime.Now.AddHours(2);
            DateTime oneHourLater = SystemTime.Now.AddHours(1);
            DateTime fourHoursLater = SystemTime.Now.AddHours(4);
            DateTime threeHoursLater = SystemTime.Now.AddHours(3);
            DateTime twelveHoursLater = SystemTime.Now.AddHours(12);
            DateTime eightHoursLater = SystemTime.Now.AddHours(8);

            roundRobinGroup.Matches[0].SetStartDateTime(twoHoursLater);
            roundRobinGroup.Matches[1].SetStartDateTime(oneHourLater);
            roundRobinGroup.Matches[2].SetStartDateTime(fourHoursLater);
            roundRobinGroup.Matches[3].SetStartDateTime(threeHoursLater);
            roundRobinGroup.Matches[4].SetStartDateTime(twelveHoursLater);
            roundRobinGroup.Matches[5].SetStartDateTime(eightHoursLater);

            roundRobinGroup.Matches[0].StartDateTime.Should().Be(twoHoursLater);
            roundRobinGroup.Matches[1].StartDateTime.Should().Be(oneHourLater);
            roundRobinGroup.Matches[2].StartDateTime.Should().Be(fourHoursLater);
            roundRobinGroup.Matches[3].StartDateTime.Should().Be(threeHoursLater);
            roundRobinGroup.Matches[4].StartDateTime.Should().Be(twelveHoursLater);
            roundRobinGroup.Matches[5].StartDateTime.Should().Be(eightHoursLater);

            tournamentIssueReporter.Issues.Should().BeEmpty();
        }

        private RoundRobinGroup RegisterPlayers(List<string> playerNames)
        {
            foreach (string playerName in playerNames)
            {
                roundRobinRound.RegisterPlayerReference(playerName);
            }

            return roundRobinRound.Groups.First() as RoundRobinGroup;
        }
    }
}
