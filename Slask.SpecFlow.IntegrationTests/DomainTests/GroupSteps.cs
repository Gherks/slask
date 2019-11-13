using FluentAssertions;
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

        [When(@"group is added to created round (.*)")]
        public void WhenGroupIsAddedToCreatedRound(int roundIndex)
        {
            createdGroups.Add(createdRounds[roundIndex].AddGroup());
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
        public void ThenGroupShouldBeOfType(int groupIndex, string roundType)
        {
            if(ParseRoundType(roundType, out RoundType type))
            {
                GroupBase group = createdGroups[groupIndex];

                if (type == RoundType.Bracket)
                {
                    CheckRoundValidity<BracketGroup>(group);
                }
                else if (type == RoundType.DualTournament)
                {
                    CheckRoundValidity<DualTournamentGroup>(group);
                }
                else if (type == RoundType.RoundRobin)
                {
                    CheckRoundValidity<RoundRobinGroup>(group);
                }
            }
        }

        protected static void CheckRoundValidity<GroupType>(GroupBase group)
        {
            group.Should().NotBeNull();
            group.Should().BeOfType<GroupType>();
            group.Id.Should().NotBeEmpty();
            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().BeEmpty();
            group.RoundId.Should().NotBeEmpty();
            group.Round.Should().NotBeNull();
        }
    }
}
