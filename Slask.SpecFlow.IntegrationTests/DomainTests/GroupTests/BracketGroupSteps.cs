﻿using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using System;
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
