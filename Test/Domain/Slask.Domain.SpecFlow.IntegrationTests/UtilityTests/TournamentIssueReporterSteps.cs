using FluentAssertions;
using Slask.Domain.SpecFlow.IntegrationTests.GroupTests;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Slask.Domain.SpecFlow.IntegrationTests.UtilityTests
{
    [Binding, Scope(Feature = "TournamentIssueReporter")]
    public class TournamentIssueReporterSteps : TournamentIssueReporterStepDefinitions
    {

    }

    public class TournamentIssueReporterStepDefinitions : GroupStepDefinitions
    {
        [Given(@"tournament (.*) reports issues")]
        [When(@"tournament (.*) reports issues")]
        [Then(@"tournament (.*) reports issues")]
        public void ThenTournamentReportsIssues(int tournamentIndex, Table table)
        {
            if (createdTournaments.Count <= tournamentIndex)
            {
                throw new IndexOutOfRangeException("Given tournament index is out of bounds");
            }

            Tournament tournament = createdTournaments[tournamentIndex];

            tournament.TournamentIssueReporter.Issues.Should().HaveCount(table.Rows.Count);

            for (int index = 0; index < table.Rows.Count; ++index)
            {
                TournamentIssueType tournamentIssueType = table.Rows[index].CreateInstance<TournamentIssueType>();
                string typeName = tournamentIssueType.IssueType;

                if (typeName.Length > 0)
                {
                    if (typeName.ToUpper() == "TOURNAMENT")
                    {
                        tournament.TournamentIssueReporter.Issues[index].IsTournamentIssue().Should().BeTrue();
                    }
                    else if (typeName.ToUpper() == "ROUND")
                    {
                        tournament.TournamentIssueReporter.Issues[index].IsRoundIssue().Should().BeTrue();
                    }
                    else if (typeName.ToUpper() == "GROUP")
                    {
                        tournament.TournamentIssueReporter.Issues[index].IsGroupIssue().Should().BeTrue();
                    }
                    else if (typeName.ToUpper() == "MATCH")
                    {
                        tournament.TournamentIssueReporter.Issues[index].IsMatchIssue().Should().BeTrue();
                    }
                }
            }
        }

        private sealed class TournamentIssueType
        {
            public string IssueType { get; set; }
        }
    }
}
