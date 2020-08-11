using FluentAssertions;
using Slask.Common;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Domain.Xunit.UnitTests.UtilityTests
{
    public class MatchStartDateTimeValidatorTests
    {
        private const string firstPlayerName = "Maru";
        private const string secondPlayerName = "Stork";

        private readonly Tournament tournament;
        private readonly TournamentIssueReporter tournamentIssueReporter;
        private readonly BracketRound bracketRound;

        public MatchStartDateTimeValidatorTests()
        {
            tournament = Tournament.Create("GSL 2019");
            tournamentIssueReporter = tournament.TournamentIssueReporter;
            bracketRound = tournament.AddBracketRound() as BracketRound;
            bracketRound.RegisterPlayerReference(firstPlayerName);
            bracketRound.RegisterPlayerReference(secondPlayerName);
        }

        [Fact]
        public void MatchStartDateTimeCannotBeChangedToSometimeInThePast()
        {
            BracketGroup bracketGroup = bracketRound.Groups.First() as BracketGroup;
            Match match = bracketGroup.Matches.First();
            DateTime oneHourInThePast = SystemTime.Now.AddSeconds(-1);

            bool validationResult = MatchStartDateTimeValidator.Validate(match, oneHourInThePast);

            validationResult.Should().BeFalse();
            tournamentIssueReporter.Issues.Should().HaveCount(1);
        }

        [Fact]
        public void IssueIsReportedWhenStartDateTimeForMatchIsSetEarlierThanAnyMatchInPreviousRound()
        {
            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };
            bracketRound.SetPlayersPerGroupCount(4);
            BracketRound secondBracketRound = tournament.AddBracketRound() as BracketRound;

            foreach (string playerName in playerNames)
            {
                bracketRound.RegisterPlayerReference(playerName);
            }

            BracketGroup bracketGroup = bracketRound.Groups.First() as BracketGroup;
            BracketNode finalNodeFromFirstRound = bracketGroup.BracketNodeSystem.FinalNode;

            BracketGroup bracketGroupFromSecondRound = secondBracketRound.Groups.First() as BracketGroup;
            BracketNode finalNodeFromSecondRound = bracketGroupFromSecondRound.BracketNodeSystem.FinalNode;

            Match finalFromFirstRound = finalNodeFromFirstRound.Match;
            Match finalFromSecondRound = finalNodeFromSecondRound.Match;
            DateTime oneHourBeforeFinalFromFirstRound = finalFromFirstRound.StartDateTime.AddHours(-1);

            bool validationResult = MatchStartDateTimeValidator.Validate(finalFromSecondRound, oneHourBeforeFinalFromFirstRound);

            validationResult.Should().BeTrue();
            tournamentIssueReporter.Issues.Should().HaveCount(1);
        }
    }
}
