using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests
{
    [Binding, Scope(Feature = "DualTournamentGroup")]
    public class DualTournamentGroupSteps : DualTournamentGroupStepDefinitions
    {

    }

    public class DualTournamentGroupStepDefinitions : GroupStepDefinitions
    {
        [Then(@"advancing players in created group (.*) is ""(.*)""")]
        public void ThenWinningPlayersInGroupIs(int groupIndex, string commaSeparatedPlayerNames)
        {
            GroupBase group = createdGroups[groupIndex];
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            List<PlayerReference> playerReferences = group.GetAdvancingPlayers();

            playerReferences.Should().NotBeEmpty();
            playerReferences.Should().HaveCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                playerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }

        protected override void PlayAvailableMatches(GroupBase group)
        {
            int winningScore = (int)Math.Ceiling(group.Round.BestOf / 2.0);

            foreach (Domain.Match match in group.Matches)
            {
                bool matchShouldHaveStarted = match.StartDateTime < SystemTime.Now;
                bool matchIsNotFinished = match.GetPlayState() != PlayState.IsFinished;

                if (matchShouldHaveStarted && matchIsNotFinished)
                {
                    match.Player1.IncreaseScore(winningScore);
                }
            }
        }
    }
}
