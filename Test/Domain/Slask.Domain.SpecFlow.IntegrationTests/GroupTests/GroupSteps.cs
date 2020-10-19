using FluentAssertions;
using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Rounds;
using Slask.Domain.SpecFlow.IntegrationTests.RoundTests;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Slask.Domain.SpecFlow.IntegrationTests.GroupTests
{
    [Binding, Scope(Feature = "Group")]
    public sealed class GroupSteps : GroupStepDefinitions
    {
        [AfterScenario]
        public static void AfterScenario()
        {
            SystemTimeMocker.Reset();
        }
    }

    public class GroupStepDefinitions : RoundStepDefinitions
    {
        [Then(@"group (.*) should be valid of type ""(.*)""")]
        public void ThenGroupShouldBeValidOfType(int groupIndex, string roundType)
        {
            Enum.TryParse(roundType, out ContestTypeEnum contestType);

            GroupBase group = createdGroups[groupIndex];

            if (contestType == ContestTypeEnum.Bracket)
            {
                CheckGroupValidity<BracketGroup>(group);
            }
            else if (contestType == ContestTypeEnum.DualTournament)
            {
                CheckGroupValidity<DualTournamentGroup>(group);
            }
            else if (contestType == ContestTypeEnum.RoundRobin)
            {
                CheckGroupValidity<RoundRobinGroup>(group);
            }
        }

        [Then(@"minutes between matches in group (.*) should be (.*)")]
        public void ThenMinutesBetweenMatchesInGroupShouldBe(int groupIndex, int expectedMinutes)
        {
            GroupBase group = createdGroups[groupIndex];

            group.Matches.Should().HaveCountGreaterOrEqualTo(2);

            for (int matchIndex = 1; matchIndex < group.Matches.Count; ++matchIndex)
            {
                Domain.Match previousMatch = group.Matches[matchIndex - 1];
                Domain.Match currentMatch = group.Matches[matchIndex];

                DateTime previousDateTime = previousMatch.StartDateTime;
                DateTime currentDateTime = currentMatch.StartDateTime;

                int minuteDifference = (int)currentDateTime.Subtract(previousDateTime).TotalMinutes;
                minuteDifference.Should().Be(expectedMinutes);
            }
        }

        [Then(@"group (.*) should contain exactly these player references with names: ""(.*)""")]
        public void ThenGroupShouldContainExactlyThesePlayerReferencesWithNames(int groupIndex, string commaSeparatedPlayerNames)
        {
            GroupBase group = createdGroups[groupIndex];
            List<string> playerNames = commaSeparatedPlayerNames.ToStringList(",");

            List<PlayerReference> playerReferences = group.GetPlayerReferences();

            playerReferences.Should().HaveCount(playerNames.Count);
            foreach (string playerName in playerNames)
            {
                playerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }

        [Then(@"participating players in group (.*) should be mapped accordingly")]
        public void ThenParticipatingPlayersInGroupShouldBeMappedAccordingly(int groupIndex, Table table)
        {
            GroupBase group = createdGroups[groupIndex];

            foreach (BracketGroupMatchSetup setup in table.CreateSet<BracketGroupMatchSetup>())
            {
                group.Matches[setup.MatchIndex].Player1.GetName().Should().Be(setup.Player1Name);
                group.Matches[setup.MatchIndex].Player2.GetName().Should().Be(setup.Player2Name);
            }
        }

        [Then(@"play state of group (.*) is set to ""(.*)""")]
        public void ThenPlayStateOfGroupIsSetTo(int groupIndex, string playStateString)
        {
            GroupBase group = createdGroups[groupIndex];

            Enum.TryParse(playStateString, out PlayStateEnum playState);

            group.GetPlayState().Should().Be(playState);
        }

        [Then(@"advancing players from round (.*) should be exactly ""(.*)""")]
        public void ThenAdvancingPlayersFromRoundFromFirstToLastShouldBe(int roundIndex, string commaSeparatedPlayerNames)
        {
            RoundBase round = createdRounds[roundIndex];
            List<string> expectedPlayerNameOrder = commaSeparatedPlayerNames.ToStringList(",");

            List<PlayerReference> playerReferences = AdvancingPlayersSolver.FetchFrom(round);

            playerReferences.Should().HaveCount(expectedPlayerNameOrder.Count);
            for (int index = 0; index < playerReferences.Count; ++index)
            {
                playerReferences[index].Name.Should().Be(expectedPlayerNameOrder[index]);
            }
        }

        [Then(@"advancing players from group (.*) should be exactly ""(.*)""")]
        public void ThenAdvancingPlayersFromGroupFromFirstToLastShouldBeExactly(int groupIndex, string commaSeparatedPlayerNames)
        {
            GroupBase group = createdGroups[groupIndex];
            List<string> expectedPlayerNameOrder = commaSeparatedPlayerNames.ToStringList(",");

            List<PlayerReference> playerReferences = AdvancingPlayersSolver.FetchFrom(group);

            playerReferences.Should().HaveCount(expectedPlayerNameOrder.Count);
            for (int index = 0; index < playerReferences.Count; ++index)
            {
                playerReferences[index].Name.Should().Be(expectedPlayerNameOrder[index]);
            }
        }

        protected static void CheckGroupValidity<GroupType>(GroupBase group)
        {
            group.Should().NotBeNull();
            group.Should().BeOfType<GroupType>();
            group.Id.Should().NotBeEmpty();
            group.Matches.Should().NotBeEmpty();
            group.RoundId.Should().NotBeEmpty();
            group.Round.Should().NotBeNull();
        }

        private sealed class BracketGroupMatchSetup
        {
            public int MatchIndex { get; set; }
            public string Player1Name { get; set; }
            public string Player2Name { get; set; }
        }
    }
}