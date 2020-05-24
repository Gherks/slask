using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests;
using System;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests.UtilityTests
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
                ParseTournamentIssueTable(table.Rows[index], out string type);

                if (type.Length > 0)
                {
                    type = GetIssueType(type);

                    if (type == "TOURNAMENT")
                    {
                        tournament.TournamentIssueReporter.Issues[index].IsTournamentIssue().Should().BeTrue();
                    }
                    else if (type == "ROUND")
                    {
                        tournament.TournamentIssueReporter.Issues[index].IsRoundIssue().Should().BeTrue();
                    }
                    else if (type == "GROUP")
                    {
                        tournament.TournamentIssueReporter.Issues[index].IsGroupIssue().Should().BeTrue();
                    }
                    else if (type == "MATCH")
                    {
                        tournament.TournamentIssueReporter.Issues[index].IsMatchIssue().Should().BeTrue();
                    }
                }
            }
        }

        protected static void ParseTournamentIssueTable(TableRow row, out string typeName)
        {
            typeName = "";

            if (row.ContainsKey("Issue type"))
            {
                typeName = row["Issue type"];
            }
        }

        protected static string GetIssueType(string type)
        {
            type = StringUtility.ToUpperNoSpaces(type);

            if (type.Contains("TOURNAMENT", StringComparison.CurrentCulture))
            {
                return "TOURNAMENT";
            }
            else if (type.Contains("ROUND", StringComparison.CurrentCulture))
            {
                return "ROUND";
            }
            else if (type.Contains("GROUP", StringComparison.CurrentCulture))
            {
                return "GROUP";
            }
            else if (type.Contains("MATCH", StringComparison.CurrentCulture))
            {
                return "MATCH";
            }

            return "";
        }
    }
}
