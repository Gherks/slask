using FluentAssertions;
using Slask.Common;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Groups.GroupUtility;
using Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests
{
    [Binding, Scope(Feature = "PlayerStandingsSolver")]
    public class PlayerStandingsSolverSteps : PlayerStandingsSolverStepDefinitions
    {

    }

    public class PlayerStandingsSolverStepDefinitions : GroupStepDefinitions
    {
        [Then(@"player standings in group (.*) from first to last should be ""(.*)""")]
        public void ThenPlayerStandingsInGroupFromFirstToLastShouldBe(int groupIndex, string commaSeparatedPlayerNames)
        {
            GroupBase group = createdGroups[groupIndex];
            List<string> expectedPlayerNameOrder = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            List<PlayerStandingEntry> playerStandings = PlayerStandingsSolver.FetchFrom(group);

            playerStandings.Should().HaveCount(expectedPlayerNameOrder.Count);

            for (int index = 0; index < playerStandings.Count; ++index)
            {
                playerStandings[index].PlayerReference.Name.Should().Be(expectedPlayerNameOrder[index]);
            }
        }
    }
}
