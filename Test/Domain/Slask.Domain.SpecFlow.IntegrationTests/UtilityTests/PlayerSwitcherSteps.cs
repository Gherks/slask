using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.SpecFlow.IntegrationTests.GroupTests;
using TechTalk.SpecFlow;

namespace Slask.Domain.SpecFlow.IntegrationTests.UtilityTests
{
    [Binding, Scope(Feature = "PlayerSwitcher")]
    public class PlayerSwitcherSteps : PlayerSwitcherStepDefinitions
    {

    }

    public class PlayerSwitcherStepDefinitions : GroupStepDefinitions
    {
        [Given(@"player ""(.*)"" in group (.*) and player ""(.*)"" in group (.*) switches matches")]
        [When(@"player ""(.*)"" in group (.*) and player ""(.*)"" in group (.*) switches matches")]
        public void PlayerInGroupAndPlayerInGroupSwitchesMatches(string player1Name, int group1Index, string player2Name, int group2Index)
        {
            GroupBase group1 = createdGroups[group1Index];
            GroupBase group2 = createdGroups[group2Index];

            Player player1 = FindPlayerInGroup(player1Name, group1);
            Player player2 = FindPlayerInGroup(player2Name, group2);

            PlayerSwitcher.SwitchMatchesOn(player1, player2);
        }

        private Player FindPlayerInGroup(string playerName, GroupBase group)
        {
            foreach (Match match in group.Matches)
            {
                if (match.Player1.GetName() == playerName)
                {
                    return match.Player1;
                }

                if (match.Player2.GetName() == playerName)
                {
                    return match.Player2;
                }
            }

            return null;
        }

    }
}
