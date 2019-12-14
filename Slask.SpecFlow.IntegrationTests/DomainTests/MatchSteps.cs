using FluentAssertions;
using Slask.Domain;
using Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests
{
    [Binding, Scope(Feature = "Match")]
    public class MatchSteps : MatchStepDefinitions
    {

    }

    public class MatchStepDefinitions : RoundRobinGroupStepDefinitions
    {
        [Given(@"a round robin tournament with users and players has been created")]
        [When(@"a round robin tournament with users and players has been created")]
        public void GivenARoundRobinTournamentWithUsersAndPlayersHasBeenCreated()
        {
            GivenATournamentNamedWithUsersAddedToIt("GSL 2019", "Stålberto, Bönis, Guggelito");

            Table roundTable = new Table(new string[] {
                            "Round type",
                            "Round name",
                            "Best of",
                            "Advancing amount"});

            roundTable.AddRow(new string[] {
                            "Round robin",
                            "Round robin round",
                            "3",
                            "3"});

            GivenCreatedTournamentAddsRounds(0, roundTable);
            GivenGroupIsAddedToCreatedRound(0);
            GivenPlayersIsAddedToCreatedGroup("First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth", 0);
        }

        [Then(@"match (.*) in created group (.*) should be in state ""(.*)""")]
        public void ThenCreatedMatchInCreatedGroupShouldBeInState(int matchIndex, int createdGroupIndex, string playStateString)
        {
            PlayState playState = GetPlayStateFromString(playStateString);

            GroupBase group = createdGroups[createdGroupIndex];
            Match match = group.Matches[matchIndex];

            match.GetPlayState().Should().Be(playState);
        }

        private PlayState GetPlayStateFromString(string playStateString)
        {
            if (playStateString == null)
            {
                throw new ArgumentNullException(nameof(playStateString));
            }

            if (playStateString == "NotBegun")
            {
                return PlayState.NotBegun;
            }
            else if(playStateString == "IsPlaying")
            {
                return PlayState.IsPlaying;
            }
            else if(playStateString == "IsFinished")
            {
                return PlayState.IsFinished;
            }
            else
            {
                throw new Exception("Invalid playStateString given, could not map it to existing play states");
            }
        }
    }
}
