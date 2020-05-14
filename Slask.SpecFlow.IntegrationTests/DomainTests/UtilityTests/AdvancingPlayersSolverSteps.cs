using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds.Bases;
using Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests
{
    [Binding, Scope(Feature = "AdvancingPlayersSolver")]
    public class AdvancingPlayersSolverSteps : AdvancingPlayersSolverStepDefinitions
    {

    }

    public class AdvancingPlayersSolverStepDefinitions : GroupStepDefinitions
    {
        [Then(@"advancing players from round (.*) should be ""(.*)""")]
        public void ThenAdvancingPlayersFromRoundFromFirstToLastShouldBe(int roundIndex, string commaSeparatedPlayerNames)
        {
            RoundBase round = createdRounds[roundIndex];
            List<string> expectedPlayerNameOrder = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            List<PlayerReference> playerStandings = AdvancingPlayersSolver.FetchFrom(round);

            playerStandings.Should().HaveCount(expectedPlayerNameOrder.Count);

            for (int index = 0; index < playerStandings.Count; ++index)
            {
                playerStandings[index].Name.Should().Be(expectedPlayerNameOrder[index]);
            }
        }

        [Then(@"advancing players from group (.*) should be ""(.*)""")]
        public void ThenAdvancingPlayersFromGroupFromFirstToLastShouldBe(int groupIndex, string commaSeparatedPlayerNames)
        {
            GroupBase group = createdGroups[groupIndex];
            List<string> expectedPlayerNameOrder = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            List<PlayerReference> playerStandings = AdvancingPlayersSolver.FetchFrom(group);

            playerStandings.Should().HaveCount(expectedPlayerNameOrder.Count);

            for (int index = 0; index < playerStandings.Count; ++index)
            {
                playerStandings[index].Name.Should().Be(expectedPlayerNameOrder[index]);
            }
        }
    }
}
