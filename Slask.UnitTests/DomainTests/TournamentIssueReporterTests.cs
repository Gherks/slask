using Slask.Domain;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Slask.UnitTests.DomainTests
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
            round = BracketRound.Create("Bracket round", 3, 3, tournament);
            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            group = round.Groups.First();
            match = group.Matches.First();

            tournamentIssueReporter = new TournamentIssueReporter();
        }

        [Fact]
        public void CanReportOnTournamentIssue()
        {
            tournamentIssueReporter.Report(tournament, "An round issue");

            tournamentIssueReporter.Issues.First().round.Should().Be(0);
            tournamentIssueReporter.Issues.First().group.Should().Be(-1);
            tournamentIssueReporter.Issues.First().match.Should().Be(-1);
        }

        [Fact]
        public void CanReportOnRoundIssue()
        {
            tournamentIssueReporter.Report(round, "An round issue");

            tournamentIssueReporter.Issues.First().round.Should().Be(0);
            tournamentIssueReporter.Issues.First().group.Should().Be(-1);
            tournamentIssueReporter.Issues.First().match.Should().Be(-1);
        }

        [Fact]
        public void CanReportOnGroupIssue()
        {
            tournamentIssueReporter.Report(group, "An group issue");

            tournamentIssueReporter.Issues.First().round.Should().Be(0);
            tournamentIssueReporter.Issues.First().group.Should().Be(0);
            tournamentIssueReporter.Issues.First().match.Should().Be(-1);
        }

        [Fact]
        public void CanReportOnMatchIssue()
        {
            tournamentIssueReporter.Report(match, "An match issue");

            tournamentIssueReporter.Issues.First().round.Should().Be(0);
            tournamentIssueReporter.Issues.First().group.Should().Be(0);
            tournamentIssueReporter.Issues.First().match.Should().Be(-1);
        }

        [Fact]
        public void CanClearReports()
        {
            tournamentIssueReporter.Report(round, "An round issue");
            tournamentIssueReporter.Report(group, "An group issue");
            tournamentIssueReporter.Report(match, "An match issue");

            tournamentIssueReporter.Clear();

            tournamentIssueReporter.Issues.Should().Empty();
        }
    }
}
