using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Slask.UnitTests.DomainTests.GroupTests.StartDateTimeTests
{
    public class DualTournamentStartDateTimeTests : IDisposable
    {
        private readonly Tournament tournament;
        private readonly DualTournamentRound dualTournamentRound;

        public DualTournamentStartDateTimeTests()
        {
            tournament = Tournament.Create("GSL 2019");
            dualTournamentRound = tournament.AddDualTournamentRound("Dual tournament round", 3) as DualTournamentRound;
        }

        public void Dispose()
        {
            SystemTimeMocker.Reset();
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

            DateTime twoHoursLater = dualTournamentGroup.Matches[0].StartDateTime.AddHours(2);
            DateTime threeHoursEarlier = dualTournamentGroup.Matches[1].StartDateTime.AddHours(-3);
            DateTime fourHoursLater = dualTournamentGroup.Matches[2].StartDateTime.AddHours(4);
            DateTime threeHoursLater = dualTournamentGroup.Matches[3].StartDateTime.AddHours(3);
            DateTime nineHoursEarlier = dualTournamentGroup.Matches[4].StartDateTime.AddHours(-9);

            dualTournamentGroup.Matches[0].SetStartDateTime(twoHoursLater);
            dualTournamentGroup.Matches[1].SetStartDateTime(threeHoursEarlier);
            dualTournamentGroup.Matches[2].SetStartDateTime(fourHoursLater);
            dualTournamentGroup.Matches[3].SetStartDateTime(threeHoursLater);
            dualTournamentGroup.Matches[4].SetStartDateTime(nineHoursEarlier);

            for (int matchIndex = 0; matchIndex < dualTournamentGroup.Matches.Count; ++matchIndex)
            {
                dualTournamentGroup.Matches[matchIndex].StartDateTime.Should().Be(dateTimesBeforeChange[matchIndex]);
            }
        }

        private DualTournamentGroup RegisterPlayers(List<string> playerNames)
        {
            foreach (string playerName in playerNames)
            {
                dualTournamentRound.RegisterPlayerReference(playerName);
            }

            return dualTournamentRound.Groups.First() as DualTournamentGroup;
        }
    }
}
