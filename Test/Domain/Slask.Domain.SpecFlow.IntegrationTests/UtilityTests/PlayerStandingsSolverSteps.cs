using FluentAssertions;
using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.SpecFlow.IntegrationTests.GroupTests;
using Slask.Domain.Utilities.StandingsSolvers;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.Domain.SpecFlow.IntegrationTests.UtilityTests
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


            PlayerStandingsSolver playerStandingsSolver = new PlayerStandingsSolver();
            List<StandingsEntry<PlayerReference>> playerStandings = playerStandingsSolver.FetchFrom(group);

            playerStandings.Should().HaveCount(expectedPlayerNameOrder.Count);

            for (int index = 0; index < playerStandings.Count; ++index)
            {
                playerStandings[index].Object.Name.Should().Be(expectedPlayerNameOrder[index]);
            }
        }
    }
}
