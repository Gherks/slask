using FluentAssertions;
using Slask.Domain.Groups;
using Slask.Domain.SpecFlow.IntegrationTests.GroupTests;
using Slask.Domain.Utilities;
using TechTalk.SpecFlow;

namespace Slask.Domain.SpecFlow.IntegrationTests
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
            GivenATournamentNamedHasBeenCreatedWithUsersAddedToIt("GSL 2019", "Stålberto, Bönis, Guggelito");

            Table roundTable = new Table(new string[] {
                            "Round type",
                            "Best of",
                            "Advancing per group count",
                            "Players per group count"});

            roundTable.AddRow(new string[] {
                            "Round robin",
                            "3",
                            "3",
                            "4"});

            roundTable.AddRow(new string[] {
                            "Bracket",
                            "3",
                            "1",
                            "6"});

            GivenTournamentAddsRounds(0, roundTable);
            GivenPlayersIsRegisteredToRound("First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth", 0);
        }

        [When(@"best of in match (.*) in group (.*) is set to (.*)")]
        public void WhenBestOfInMatchInGroupIsSetTo(int matchIndex, int groupIndex, int bestOf)
        {
            GroupBase group = createdGroups[groupIndex];
            Match match = group.Matches[matchIndex];

            match.SetBestOf(bestOf);
        }

        [Then(@"best of in match (.*) in group (.*) should be (.*)")]
        public void ThenBestOfInMatchInGroupShouldBe(int matchIndex, int groupIndex, int bestOf)
        {
            GroupBase group = createdGroups[groupIndex];
            Match match = group.Matches[matchIndex];

            match.BestOf.Should().Be(bestOf);
        }

        [Then(@"match (.*) in group (.*) should be in state ""(.*)""")]
        public void ThenMatchInGroupShouldBeInState(int matchIndex, int groupIndex, string playStateString)
        {
            PlayState playState = ParsePlayStateString(playStateString);

            GroupBase group = createdGroups[groupIndex];
            Match match = group.Matches[matchIndex];

            match.GetPlayState().Should().Be(playState);
        }

        [Then(@"winning player can be fetched from match (.*) in group (.*)")]
        public void ThenWinningPlayerCanBeFetchedFromMatchInGroup(int matchIndex, int groupIndex)
        {
            GroupBase group = createdGroups[groupIndex];
            Match match = group.Matches[matchIndex];

            match.GetWinningPlayer().Should().NotBeNull();
        }

        [Then(@"losing player can be fetched from match (.*) in group (.*)")]
        public void ThenLosingPlayerCanBeFetchedFromMatchInGroup(int matchIndex, int groupIndex)
        {
            GroupBase group = createdGroups[groupIndex];
            Match match = group.Matches[matchIndex];

            match.GetLosingPlayer().Should().NotBeNull();
        }

        [Then(@"winning player cannot be fetched from match (.*) in group (.*)")]
        public void ThenWinningPlayerCannotBeFetchedFromMatchInGroup(int matchIndex, int groupIndex)
        {
            GroupBase group = createdGroups[groupIndex];
            Match match = group.Matches[matchIndex];

            match.GetWinningPlayer().Should().BeNull();
        }

        [Then(@"losing player cannot be fetched from match (.*) in group (.*)")]
        public void ThenLosingPlayerCannotBeFetchedFromMatchInGroup(int matchIndex, int groupIndex)
        {
            GroupBase group = createdGroups[groupIndex];
            Match match = group.Matches[matchIndex];

            match.GetLosingPlayer().Should().BeNull();
        }
    }
}
