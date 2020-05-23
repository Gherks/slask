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
                            "Advancing per group count",
                            "Players per group count"});

            roundTable.AddRow(new string[] {
                            "Round robin",
                            "Round 1",
                            "3",
                            "3",
                            "4"});

            roundTable.AddRow(new string[] {
                            "Bracket",
                            "Round 2",
                            "3",
                            "1",
                            "6"});

            GivenTournamentAddsRounds(0, roundTable);
            GivenPlayersIsRegisteredToRound("First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth", 0);
        }

        [Then(@"match (.*) in group (.*) should be in state ""(.*)""")]
        public void ThenMatchInGroupShouldBeInState(int matchIndex, int createdGroupIndex, string playStateString)
        {
            PlayState playState = ParsePlayStateString(playStateString);

            GroupBase group = createdGroups[createdGroupIndex];
            Match match = group.Matches[matchIndex];

            match.GetPlayState().Should().Be(playState);
        }

        [Then(@"winning player can be fetched from match (.*) in group (.*)")]
        public void ThenWinningPlayerCanBeFetchedFromMatchInGroup(int matchIndex, int createdGroupIndex)
        {
            GroupBase group = createdGroups[createdGroupIndex];
            Match match = group.Matches[matchIndex];

            match.GetWinningPlayer().Should().NotBeNull();
        }

        [Then(@"losing player can be fetched from match (.*) in group (.*)")]
        public void ThenLosingPlayerCanBeFetchedFromMatchInGroup(int matchIndex, int createdGroupIndex)
        {
            GroupBase group = createdGroups[createdGroupIndex];
            Match match = group.Matches[matchIndex];

            match.GetLosingPlayer().Should().NotBeNull();
        }

        [Then(@"winning player cannot be fetched from match (.*) in group (.*)")]
        public void ThenWinningPlayerCannotBeFetchedFromMatchInGroup(int matchIndex, int createdGroupIndex)
        {
            GroupBase group = createdGroups[createdGroupIndex];
            Match match = group.Matches[matchIndex];

            match.GetWinningPlayer().Should().BeNull();
        }

        [Then(@"losing player cannot be fetched from match (.*) in group (.*)")]
        public void ThenLosingPlayerCannotBeFetchedFromMatchInGroup(int matchIndex, int createdGroupIndex)
        {
            GroupBase group = createdGroups[createdGroupIndex];
            Match match = group.Matches[matchIndex];

            match.GetLosingPlayer().Should().BeNull();
        }
    }
}
