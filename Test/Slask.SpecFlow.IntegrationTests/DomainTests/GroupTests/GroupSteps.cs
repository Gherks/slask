using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using Slask.SpecFlow.IntegrationTests.DomainTests.RoundTests;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests
{
    [Binding, Scope(Feature = "Group")]
    public class GroupSteps : GroupStepDefinitions
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
            string type = GetRoundType(roundType);

            GroupBase group = createdGroups[groupIndex];

            if (type == "BRACKET")
            {
                CheckGroupValidity<BracketGroup>(group);
            }
            else if (type == "DUALTOURNAMENT")
            {
                CheckGroupValidity<DualTournamentGroup>(group);
            }
            else if (type == "ROUNDROBIN")
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
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            group.PlayerReferences.Should().HaveCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                group.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }

        [Then(@"participating players in group (.*) should be mapped accordingly")]
        public void ThenParticipatingPlayersInGroupShouldBeMappedAccordingly(int groupIndex, Table table)
        {
            GroupBase group = createdGroups[groupIndex];

            foreach (TableRow row in table.Rows)
            {
                row["Match index"].Should().NotBeNullOrEmpty();
                row["Player 1 name"].Should().NotBeNull();
                row["Player 2 name"].Should().NotBeNull();

                ParseBracketGroupMatchSetup(row, out int matchIndex, out string player1Name, out string player2Name);

                if (player1Name.Length > 0)
                {
                    group.Matches[matchIndex].Player1.Name.Should().Be(player1Name);
                }
                else
                {
                    group.Matches[matchIndex].Player1.Should().BeNull();
                }

                if (player2Name.Length > 0)
                {
                    group.Matches[matchIndex].Player2.Name.Should().Be(player2Name);
                }
                else
                {
                    group.Matches[matchIndex].Player2.Should().BeNull();
                }
            }
        }

        [Then(@"play state of group (.*) is set to ""(.*)""")]
        public void ThenPlayStateOfGroupIsSetTo(int groupIndex, string playStateString)
        {
            GroupBase group = createdGroups[groupIndex];

            PlayState playState = ParsePlayStateString(playStateString);

            group.GetPlayState().Should().Be(playState);
        }

        [Then(@"advancing players from round (.*) should be exactly ""(.*)""")]
        public void ThenAdvancingPlayersFromRoundFromFirstToLastShouldBe(int roundIndex, string commaSeparatedPlayerNames)
        {
            RoundBase round = createdRounds[roundIndex];
            List<string> expectedPlayerNameOrder = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            List<PlayerReference> playerStandings = AdvancingPlayersSolver.FetchFrom(round);

            playerStandings.Should().HaveCount(expectedPlayerNameOrder.Count);

            for (int index = 0; index < playerStandings.Count; ++index)
            {
                playerStandings[index].Name.Should().Be(expectedPlayerNameOrder[index]);
            }
        }

        [Then(@"advancing players from group (.*) should be exactly ""(.*)""")]
        public void ThenAdvancingPlayersFromGroupFromFirstToLastShouldBeExactly(int groupIndex, string commaSeparatedPlayerNames)
        {
            GroupBase group = createdGroups[groupIndex];
            List<string> expectedPlayerNameOrder = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            List<PlayerReference> playerStandings = AdvancingPlayersSolver.FetchFrom(group);

            playerStandings.Should().HaveCount(expectedPlayerNameOrder.Count);

            for (int index = 0; index < playerStandings.Count; ++index)
            {
                playerStandings[index].Name.Should().Be(expectedPlayerNameOrder[index]);
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

        protected void ParseBracketGroupMatchSetup(TableRow row, out int matchIndex, out string player1Name, out string player2Name)
        {
            matchIndex = -1;
            player1Name = "";
            player2Name = "";

            if (row.ContainsKey("Match index"))
            {
                int.TryParse(row["Match index"], out matchIndex);
            }

            matchIndex.Should().BeGreaterOrEqualTo(0);

            if (row.ContainsKey("Player 1 name"))
            {
                player1Name = row["Player 1 name"];
            }

            if (row.ContainsKey("Player 2 name"))
            {
                player2Name = row["Player 2 name"];
            }
        }
    }
}