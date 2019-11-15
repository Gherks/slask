using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests
{
    [Binding, Scope(Feature = "Group")]
    public class GroupSteps : GroupStepDefinitions
    {

    }

    public class GroupStepDefinitions : RoundStepDefinitions
    {
        protected readonly List<GroupBase> createdGroups;

        public GroupStepDefinitions()
        {
            createdGroups = new List<GroupBase>();
        }

        [When(@"created rounds (.*) to (.*) creates (.*) groups each")]
        public void WhenCreatedRoundsToCreatesGroupsEach(int roundStartIndex, int roundEndIndex, int groupAmount)
        {
            for (int roundIndex = roundStartIndex; roundIndex < roundEndIndex; ++roundIndex)
            {
                for (int groupCounter = 0; groupCounter < groupAmount; ++groupCounter)
                {
                    createdRounds[roundIndex].AddGroup();
                }
            }
        }

        [Given(@"group is added to created round (.*)")]
        [When(@"group is added to created round (.*)")]
        public void GivenGroupIsAddedToCreatedRound(int roundIndex)
        {
            createdGroups.Add(createdRounds[roundIndex].AddGroup());
        }

        [When(@"players ""(.*)"" is added to created group (.*)")]
        public void WhenPlayersIsAddedToCreatedGroup(string commaSeparatedPlayerNames, int groupIndex)
        {
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");
            GroupBase group = createdGroups[groupIndex];

            foreach (string playerName in playerNames)
            {
                group.AddPlayerReference(playerName);
            }
        }

        [Then(@"created rounds (.*) to (.*) should contain (.*) groups each")]
        public void ThenCreatedRoundsToShouldContainGroupsEach(int roundStartIndex, int roundEndIndex, int groupAmount)
        {
            for (int roundIndex = roundStartIndex; roundIndex < roundEndIndex; ++roundIndex)
            {
                for (int groupCounter = 0; groupCounter < groupAmount; ++groupCounter)
                {
                    createdRounds[roundIndex].Groups.Should().HaveCount(groupAmount);
                }
            }
        }

        [Then(@"group (.*) should be valid of type ""(.*)""")]
        public void ThenGroupShouldBeValidOfType(int groupIndex, string roundType)
        {
            string type = GetRoundType(roundType);

            GroupBase group = createdGroups[groupIndex];

            if (type == "BRACKET")
            {
                CheckGroupValidity<BracketGroup>(group, 0);
            }
            else if (type == "DUALTOURNAMENT")
            {
                CheckGroupValidity<DualTournamentGroup>(group, 5);
            }
            else if (type == "ROUNDROBIN")
            {
                CheckGroupValidity<RoundRobinGroup>(group, 0);
            }
        }

        [Then(@"minutes between matches in created group (.*) should be (.*)")]
        public void ThenMinutesBetweenMatchesInCreatedGroupShouldBe(int groupIndex, int expectedMinutes)
        {
            GroupBase group = createdGroups[groupIndex];

            group.Matches.Should().HaveCountGreaterOrEqualTo(2);

            for (int matchIndex = 1; matchIndex < group.Matches.Count; ++matchIndex)
            {
                Match previousMatch = group.Matches[matchIndex - 1];
                Match currentMatch = group.Matches[matchIndex];

                DateTime previousDateTime = previousMatch.StartDateTime;
                DateTime currentDateTime = currentMatch.StartDateTime;

                int minuteDifference = (int)currentDateTime.Subtract(previousDateTime).TotalMinutes;
                minuteDifference.Should().Be(expectedMinutes);
            }
        }

        protected static void CheckGroupValidity<GroupType>(GroupBase group, int matchesUponCreation)
        {
            group.Should().NotBeNull();
            group.Should().BeOfType<GroupType>();
            group.Id.Should().NotBeEmpty();
            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().HaveCount(matchesUponCreation);
            group.RoundId.Should().NotBeEmpty();
            group.Round.Should().NotBeNull();
        }
    }
}
