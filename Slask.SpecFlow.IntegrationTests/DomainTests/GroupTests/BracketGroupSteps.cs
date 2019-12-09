using FluentAssertions;
using Slask.Domain;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests
{
    [Binding, Scope(Feature = "BracketGroup")]
    public class BracketGroupSteps : BracketGroupStepDefinitions
    {

    }

    public class BracketGroupStepDefinitions : GroupStepDefinitions
    {
        [Then(@"advancing players in created group (.*) is ""(.*)""")]
        public void ThenWinningPlayersInGroupIs(int groupIndex, string playerName)
        {
            GroupBase group = createdGroups[groupIndex];
            List<PlayerReference> playerReferences = group.GetAdvancingPlayers();

            playerReferences.Should().NotBeEmpty();
            playerReferences.Should().HaveCount(1);
            playerReferences.Single(playerReference => playerReference.Name == playerName).Should().NotBeNull();
        }
    }
}
