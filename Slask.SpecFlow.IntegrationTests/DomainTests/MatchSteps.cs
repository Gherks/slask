using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Utilities;
using Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests;
using System;
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
                            "Advancing count"});

            roundTable.AddRow(new string[] {
                            "Round robin",
                            "Round robin round",
                            "3",
                            "3"});

            GivenCreatedTournamentAddsRounds(0, roundTable);
            GivenPlayersIsRegisteredToTournament("First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth", 0);
        }

        [Then(@"match (.*) in created group (.*) should be in state ""(.*)""")]
        public void ThenCreatedMatchInCreatedGroupShouldBeInState(int matchIndex, int createdGroupIndex, string playStateString)
        {
            PlayState playState = ParsePlayStateString(playStateString);

            GroupBase group = createdGroups[createdGroupIndex];
            Match match = group.Matches[matchIndex];

            match.GetPlayState().Should().Be(playState);
        }

        [Then(@"winning player can be fetched from match (.*) in created group (.*)")]
        public void ThenWinningPlayerCanBeFetchedFromMatchInCreatedGroup(int matchIndex, int createdGroupIndex)
        {
            GroupBase group = createdGroups[createdGroupIndex];
            Match match = group.Matches[matchIndex];

            match.GetWinningPlayer().Should().NotBeNull();
        }

        [Then(@"losing player can be fetched from match (.*) in created group (.*)")]
        public void ThenLosingPlayerCanBeFetchedFromMatchInCreatedGroup(int matchIndex, int createdGroupIndex)
        {
            GroupBase group = createdGroups[createdGroupIndex];
            Match match = group.Matches[matchIndex];

            match.GetLosingPlayer().Should().NotBeNull();
        }

        [Then(@"winning player cannot be fetched from match (.*) in created group (.*)")]
        public void ThenWinningPlayerCannotBeFetchedFromMatchInCreatedGroup(int matchIndex, int createdGroupIndex)
        {
            GroupBase group = createdGroups[createdGroupIndex];
            Match match = group.Matches[matchIndex];

            match.GetWinningPlayer().Should().BeNull();
        }

        [Then(@"losing player cannot be fetched from match (.*) in created group (.*)")]
        public void ThenLosingPlayerCannotBeFetchedFromMatchInCreatedGroup(int matchIndex, int createdGroupIndex)
        {
            GroupBase group = createdGroups[createdGroupIndex];
            Match match = group.Matches[matchIndex];

            match.GetLosingPlayer().Should().BeNull();
        }
    }
}
