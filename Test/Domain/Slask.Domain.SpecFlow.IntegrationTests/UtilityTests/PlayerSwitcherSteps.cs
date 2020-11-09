using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.SpecFlow.IntegrationTests.GroupTests;
using System;
using System.Collections.Generic;
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

            List<PlayerReference> list = group1.Round.Tournament.PlayerReferences;

            MatchPlayerReferencePair matchPlayerReferencePair1 = FindPlayerInGroup(player1Name, group1);
            MatchPlayerReferencePair matchPlayerReferencePair2 = FindPlayerInGroup(player2Name, group2);

            PlayerSwitcher.SwitchMatchesOn(
                matchPlayerReferencePair1.Match,
                matchPlayerReferencePair1.PlayerReferenceId,
                matchPlayerReferencePair2.Match,
                matchPlayerReferencePair2.PlayerReferenceId);
        }

        private MatchPlayerReferencePair FindPlayerInGroup(string playerName, GroupBase group)
        {
            foreach (Match match in group.Matches)
            {
                if (match.GetPlayer1Name() == playerName)
                {
                    return new MatchPlayerReferencePair { Match = match, PlayerReferenceId = match.PlayerReference1Id };
                }

                if (match.GetPlayer2Name() == playerName)
                {
                    return new MatchPlayerReferencePair { Match = match, PlayerReferenceId = match.PlayerReference2Id };
                }
            }

            return null;
        }

        private class MatchPlayerReferencePair
        {
            public Match Match { get; set; }
            public Guid PlayerReferenceId { get; set; }
        }
    }
}
