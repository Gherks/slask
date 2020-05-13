using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Groups.GroupUtility;
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
        [Given(@"score is added to players in given matches in created groups")]
        [When(@"score is added to players in given matches in created groups")]
        public void WhenScoreIsAddedToPlayersInGivenMatchesInCreatedGroups(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            foreach (TableRow row in table.Rows)
            {
                ParseSoreAddedToMatchPlayer(row, out int createdGroupIndex, out int matchIndex, out string scoringPlayer, out int scoreAdded);

                GroupBase group = createdGroups[createdGroupIndex];
                Match match = group.Matches[matchIndex];

                SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

                Player player = match.FindPlayer(scoringPlayer);

                if (player != null)
                {
                    player.IncreaseScore(scoreAdded);
                }
                else
                {
                    throw new Exception("Invalid player name in given match within given created group");
                }
            }
        }

        [Then(@"created rounds (.*) to (.*) should contain (.*) groups each")]
        public void ThenCreatedRoundsToShouldContainGroupsEach(int roundStartIndex, int roundEndIndex, int groupCount)
        {
            for (int roundIndex = roundStartIndex; roundIndex < roundEndIndex; ++roundIndex)
            {
                for (int groupCounter = 0; groupCounter < groupCount; ++groupCounter)
                {
                    createdRounds[roundIndex].Groups.Should().HaveCount(groupCount);
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

        [Then(@"minutes between matches in created group (.*) should be (.*)")]
        public void ThenMinutesBetweenMatchesInCreatedGroupShouldBe(int groupIndex, int expectedMinutes)
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

        [Then(@"created group (.*) should contain exactly these player references with names: ""(.*)""")]
        public void ThenCreatedGroupShouldContainExactlyThesePlayerReferencesWithNames(int groupIndex, string commaSeparatedPlayerNames)
        {
            GroupBase group = createdGroups[groupIndex];
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            group.PlayerReferences.Should().HaveCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                group.PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }


        [Then(@"advancing players in created group (.*) is exactly ""(.*)""")]
        public void ThenWinningPlayersInGroupIs(int groupIndex, string commaSeparatedPlayerNames)
        {
            GroupBase group = createdGroups[groupIndex];
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            List<PlayerReference> playerReferences = AdvancingPlayersSolver.FetchFrom(group);

            playerReferences.Should().HaveCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                playerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }


        [Then(@"participating players in group (.*) should be mapped accordingly")]
        public void ThenParticipatingPlayersInCreatedGroupShouldBeMappedAccordingly(int createdGroupIndex, Table table)
        {
            GroupBase group = createdGroups[createdGroupIndex];

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

        protected static void CheckGroupValidity<GroupType>(GroupBase group)
        {
            group.Should().NotBeNull();
            group.Should().BeOfType<GroupType>();
            group.Id.Should().NotBeEmpty();
            group.Matches.Should().NotBeEmpty();
            group.RoundId.Should().NotBeEmpty();
            group.Round.Should().NotBeNull();
        }

        protected static void ParseSoreAddedToMatchPlayer(TableRow row, out int createdGroupIndex, out int matchIndex, out string scoringPlayer, out int scoreAdded)
        {
            createdGroupIndex = 0;
            matchIndex = 0;
            scoringPlayer = "";
            scoreAdded = 0;

            if (row.ContainsKey("Created group index"))
            {
                int.TryParse(row["Created group index"], out createdGroupIndex);
            }

            if (row.ContainsKey("Match index"))
            {
                int.TryParse(row["Match index"], out matchIndex);
            }

            if (row.ContainsKey("Scoring player"))
            {
                scoringPlayer = row["Scoring player"];
            }

            if (row.ContainsKey("Added score"))
            {
                int.TryParse(row["Added score"], out scoreAdded);
            }
        }

        protected static void ParseBracketGroupMatchSetup(TableRow row, out int matchIndex, out string player1Name, out string player2Name)
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