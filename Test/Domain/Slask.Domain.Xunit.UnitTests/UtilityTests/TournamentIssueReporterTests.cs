using FluentAssertions;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using System.Linq;
using Xunit;

namespace Slask.Domain.Xunit.UnitTests.UtilityTests
{
    public class TournamentIssueReporterTests
    {
        private readonly Tournament tournament;
        private readonly RoundBase round;
        private readonly GroupBase group;
        private readonly Match match;
        private readonly TournamentIssueReporter tournamentIssueReporter;

        public TournamentIssueReporterTests()
        {
            tournament = Tournament.Create("GSL 2019");
            round = tournament.AddBracketRound();
            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            group = round.Groups.First();
            match = group.Matches.First();

            tournamentIssueReporter = new TournamentIssueReporter();
        }

        [Fact]
        public void CanReportOnTournamentIssue()
        {
            tournamentIssueReporter.Report(tournament, TournamentIssues.StartDateTimeIsInThePast);

            tournamentIssueReporter.Issues.First().Round.Should().Be(-1);
            tournamentIssueReporter.Issues.First().Group.Should().Be(-1);
            tournamentIssueReporter.Issues.First().Match.Should().Be(-1);
            tournamentIssueReporter.Issues.First().Description.Should().Be("Start date time must be a future date");
        }

        [Fact]
        public void CanReportOnRoundIssue()
        {
            tournamentIssueReporter.Report(round, TournamentIssues.StartDateTimeIsInThePast);

            tournamentIssueReporter.Issues.First().Round.Should().Be(0);
            tournamentIssueReporter.Issues.First().Group.Should().Be(-1);
            tournamentIssueReporter.Issues.First().Match.Should().Be(-1);
            tournamentIssueReporter.Issues.First().Description.Should().Be("Start date time must be a future date");
        }

        [Fact]
        public void CanReportOnGroupIssue()
        {
            tournamentIssueReporter.Report(group, TournamentIssues.StartDateTimeIsInThePast);

            tournamentIssueReporter.Issues.First().Round.Should().Be(0);
            tournamentIssueReporter.Issues.First().Group.Should().Be(0);
            tournamentIssueReporter.Issues.First().Match.Should().Be(-1);
            tournamentIssueReporter.Issues.First().Description.Should().Be("Start date time must be a future date");
        }

        [Fact]
        public void CanReportOnMatchIssue()
        {
            tournamentIssueReporter.Report(match, TournamentIssues.StartDateTimeIsInThePast);

            tournamentIssueReporter.Issues.First().Round.Should().Be(0);
            tournamentIssueReporter.Issues.First().Group.Should().Be(0);
            tournamentIssueReporter.Issues.First().Match.Should().Be(0);
            tournamentIssueReporter.Issues.First().Description.Should().Be("Start date time must be a future date");
        }

        [Fact]
        public void CanClearReports()
        {
            tournamentIssueReporter.Report(round, TournamentIssues.NotFillingAllGroupsWithPlayers);
            tournamentIssueReporter.Report(group, TournamentIssues.RoundDoesNotSynergizeWithPreviousRound);
            tournamentIssueReporter.Report(match, TournamentIssues.AdvancersCountInRoundIsGreaterThanParticipantCount);

            tournamentIssueReporter.Clear();

            tournamentIssueReporter.Issues.Should().BeEmpty();
        }
    }
}
