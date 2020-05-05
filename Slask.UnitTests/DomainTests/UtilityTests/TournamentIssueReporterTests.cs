using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.UtilityTests
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
            round = tournament.AddBracketRound("Bracket round", 3);
            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            group = round.Groups.First();
            match = group.Matches.First();

            tournamentIssueReporter = new TournamentIssueReporter();
        }

        [Fact]
        public void CanReportOnTournamentIssue()
        {
            string description = "An round issue";

            tournamentIssueReporter.Report(tournament, description);

            tournamentIssueReporter.Issues.First().Round.Should().Be(-1);
            tournamentIssueReporter.Issues.First().Group.Should().Be(-1);
            tournamentIssueReporter.Issues.First().Match.Should().Be(-1);
            tournamentIssueReporter.Issues.First().Description.Should().Be(description);
        }

        [Fact]
        public void CanReportOnRoundIssue()
        {
            string description = "An round issue";

            tournamentIssueReporter.Report(round, description);

            tournamentIssueReporter.Issues.First().Round.Should().Be(0);
            tournamentIssueReporter.Issues.First().Group.Should().Be(-1);
            tournamentIssueReporter.Issues.First().Match.Should().Be(-1);
            tournamentIssueReporter.Issues.First().Description.Should().Be(description);
        }

        [Fact]
        public void CanReportOnGroupIssue()
        {
            string description = "An group issue";

            tournamentIssueReporter.Report(group, description);

            tournamentIssueReporter.Issues.First().Round.Should().Be(0);
            tournamentIssueReporter.Issues.First().Group.Should().Be(0);
            tournamentIssueReporter.Issues.First().Match.Should().Be(-1);
            tournamentIssueReporter.Issues.First().Description.Should().Be(description);
        }

        [Fact]
        public void CanReportOnMatchIssue()
        {
            string description = "An match issue";

            tournamentIssueReporter.Report(match, description);

            tournamentIssueReporter.Issues.First().Round.Should().Be(0);
            tournamentIssueReporter.Issues.First().Group.Should().Be(0);
            tournamentIssueReporter.Issues.First().Match.Should().Be(0);
            tournamentIssueReporter.Issues.First().Description.Should().Be(description);
        }

        [Fact]
        public void CanClearReports()
        {
            tournamentIssueReporter.Report(round, "An round issue");
            tournamentIssueReporter.Report(group, "An group issue");
            tournamentIssueReporter.Report(match, "An match issue");

            tournamentIssueReporter.Clear();

            tournamentIssueReporter.Issues.Should().BeEmpty();
        }
    }
}
