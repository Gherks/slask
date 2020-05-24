using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Xunit.UnitTests.DomainTests.MatchTests.StartDateTimeTests
{
    public class BracketStartDateTimeTests
    {
        private const string firstPlayerName = "Maru";
        private const string secondPlayerName = "Stork";

        private readonly Tournament tournament;
        private readonly TournamentIssueReporter tournamentIssueReporter;
        private readonly BracketRound bracketRound;

        public BracketStartDateTimeTests()
        {
            tournament = Tournament.Create("GSL 2019");
            tournamentIssueReporter = tournament.TournamentIssueReporter;
            bracketRound = tournament.AddBracketRound() as BracketRound;
        }

        [Fact]
        public void IssueIsReportedWhenStartDateTimeForMatchIsEarlierThanAnyParentMatch()
        {
            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };
            bracketRound.SetPlayersPerGroupCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                bracketRound.RegisterPlayerReference(playerName);
            }

            BracketGroup bracketGroup = bracketRound.Groups.First() as BracketGroup;
            BracketNode finalNode = bracketGroup.BracketNodeSystem.FinalNode;

            Match final = finalNode.Match;
            Match firstSemifinal = finalNode.Children[0].Match;
            Match secondSemifinal = finalNode.Children[1].Match;

            DateTime oneHourBeforeFirstSemifinal = firstSemifinal.StartDateTime.AddHours(-1);
            DateTime oneHourBeforeSecondSemifinal = secondSemifinal.StartDateTime.AddHours(-1);

            final.SetStartDateTime(oneHourBeforeFirstSemifinal);
            final.StartDateTime.Should().Be(oneHourBeforeFirstSemifinal);

            final.SetStartDateTime(oneHourBeforeSecondSemifinal);
            final.StartDateTime.Should().Be(oneHourBeforeSecondSemifinal);

            tournamentIssueReporter.Issues.Should().HaveCount(2);
        }

        [Fact]
        public void NoIssueIsReportedWhenStartDateTimeOnMatchesWithinATierIsUnordered()
        {
            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };
            bracketRound.SetPlayersPerGroupCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                bracketRound.RegisterPlayerReference(playerName);
            }

            BracketGroup bracketGroup = bracketRound.Groups.First() as BracketGroup;
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

            tournamentIssueReporter.Issues.Should().BeEmpty();
        }
    }
}
