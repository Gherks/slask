using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.GroupTests.StartDateTimeTests
{
    public class BracketStartDateTimeTests : IDisposable
    {
        private const string firstPlayerName = "Maru";
        private const string secondPlayerName = "Stork";

        private readonly Tournament tournament;
        private readonly BracketRound bracketRound;
        private BracketGroup bracketGroup;
        //private Match match;
        //private PlayerReference firstPlayerReference;
        //private PlayerReference secondPlayerReference;

        public BracketStartDateTimeTests()
        {
            tournament = Tournament.Create("GSL 2019");
            bracketRound = tournament.AddBracketRound("Bracket round", 3) as BracketRound;
        }

        public void Dispose()
        {
            SystemTimeMocker.Reset();
        }

        [Fact]
        public void StartDateTimeOnMatchesWithinATierDoesNotHaveToBeInOrder()
        {
            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };
            bracketRound.SetPlayersPerGroupCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                bracketRound.RegisterPlayerReference(playerName);
            }

            bracketGroup = bracketRound.Groups.First() as BracketGroup;
            List<BracketNode> quarterfinalTier = bracketGroup.BracketNodeSystem.GetBracketNodesInTier(2);

            DateTime twoHoursEarlier = quarterfinalTier[0].Match.StartDateTime.AddHours(-2);
            DateTime threeHoursEarlier = quarterfinalTier[1].Match.StartDateTime.AddHours(-3);
            DateTime oneHourEarlier = quarterfinalTier[2].Match.StartDateTime.AddHours(-1);
            DateTime twoHoursLater = quarterfinalTier[3].Match.StartDateTime.AddHours(2);

            quarterfinalTier[0].Match.SetStartDateTime(twoHoursEarlier);
            quarterfinalTier[1].Match.SetStartDateTime(threeHoursEarlier);
            quarterfinalTier[2].Match.SetStartDateTime(oneHourEarlier);
            quarterfinalTier[3].Match.SetStartDateTime(twoHoursLater);

            quarterfinalTier[0].Match.StartDateTime.Should().Be(twoHoursEarlier);
            quarterfinalTier[1].Match.StartDateTime.Should().Be(threeHoursEarlier);
            quarterfinalTier[2].Match.StartDateTime.Should().Be(oneHourEarlier);
            quarterfinalTier[3].Match.StartDateTime.Should().Be(twoHoursLater);
        }

        [Fact]
        public void StartDateTimeForMatchesInACertainMatchTierMustAlwaysBeLaterThanLatestStartDateTimeOfPreviousTier()
        {
            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };
            bracketRound.SetPlayersPerGroupCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                bracketRound.RegisterPlayerReference(playerName);
            }

            bracketGroup = bracketRound.Groups.First() as BracketGroup;
            List<BracketNode> finalTier = bracketGroup.BracketNodeSystem.GetBracketNodesInTier(0);
            List<BracketNode> semifinalTier = bracketGroup.BracketNodeSystem.GetBracketNodesInTier(1);
            List<BracketNode> quarterfinalTier = bracketGroup.BracketNodeSystem.GetBracketNodesInTier(2);

            DateTime finalStartDateTimeBeforeChange = finalTier[0].Match.StartDateTime;
            DateTime quarterfinalStartDateTimeBeforeChange = quarterfinalTier[0].Match.StartDateTime;

            finalTier[0].Match.SetStartDateTime(semifinalTier[0].Match.StartDateTime.AddHours(-4));
            quarterfinalTier[0].Match.SetStartDateTime(semifinalTier[0].Match.StartDateTime.AddHours(4));

            finalTier[0].Match.StartDateTime.Should().Be(finalStartDateTimeBeforeChange);
            quarterfinalTier[0].Match.StartDateTime.Should().Be(quarterfinalStartDateTimeBeforeChange);
        }
    }
}
