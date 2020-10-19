using FluentAssertions;
using Slask.Common;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Rounds;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.Domain.SpecFlow.IntegrationTests.RoundTests.RoundTypeTests
{
    [Binding, Scope(Feature = "RoundRobinRound")]
    public class RoundRobinRoundSteps : RoundRobinRoundStepDefinitions
    {

    }

    public class RoundRobinRoundStepDefinitions : RoundStepDefinitions
    {
        [When(@"tie in group (.*) is solved by choosing ""(.*)""")]
        public void WhenTieInGroupIsSolvedByChoosing(int groupIndex, string commaSeparatedPlayerNames)
        {
            RoundRobinGroup group = createdGroups[groupIndex] as RoundRobinGroup;
            List<string> playerNames = commaSeparatedPlayerNames.ToStringList(",");
            List<PlayerReference> playerReferences = new List<PlayerReference>();

            foreach (string playerName in playerNames)
            {
                foreach (Match match in group.Matches)
                {
                    Player player = match.FindPlayer(playerName);
                    bool playerFound = player != null;

                    if (playerFound)
                    {
                        group.SolveTieByChoosing(player.PlayerReferenceId);
                        break;
                    }
                }
            }
        }

        [Then(@"round (.*) has (.*) problematic tie\(s\)")]
        public void ThenRoundHasProblematicTieS(int roundIndex, int problematicTieCount)
        {
            RoundBase round = createdRounds[roundIndex];

            round.HasProblematicTie().Should().BeTrue();
        }
    }
}
