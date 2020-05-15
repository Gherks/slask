using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests.RoundTests.RoundTypeTests
{
    [Binding, Scope(Feature = "RoundRobinRound")]
    public class RoundRobinRoundSteps : RoundRobinRoundStepDefinitions
    {

    }

    public class RoundRobinRoundStepDefinitions : RoundStepDefinitions
    {
        [Then(@"group (.*) has a problematic tie")]
        public void ThenGroupHasAProblematicTie(int roundIndex)
        {
            RoundRobinRound round = createdRounds[roundIndex] as RoundRobinRound;

            round.HasProblematicTie().Should().BeTrue();
        }

        [When(@"tie in group (.*) is solved by choosing ""(.*)""")]
        public void WhenTieInGroupIsSolvedByChoosing(int groupIndex, string commaSeparatedPlayerNames)
        {
            RoundRobinGroup group = createdGroups[groupIndex] as RoundRobinGroup;
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");
            List<PlayerReference> playerReferences = new List<PlayerReference>();

            foreach (string playerName in playerNames)
            {
                foreach (Match match in group.Matches)
                {
                    Player player = match.FindPlayer(playerName);
                    bool playerFound = player != null;
                    
                    if(playerFound)
                    {
                        playerReferences.Add(player.PlayerReference);
                        break;
                    }
                }
            }

            group.SolveTieByChoosing(playerReferences);

            List<PlayerReference> advancingPlayerReferences = AdvancingPlayersSolver.FetchFrom(group);

            advancingPlayerReferences.Should().BeEquivalentTo(playerReferences);
        }
    }
}
