using FluentAssertions;
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
        [Then(@"pariticpating players in created group (.*) should be mapped accordingly")]
        public void ThenPariticpatingPlayersInCreatedGroupShouldBeMappedAccordingly(int createdGroupIndex, Table table)
        {
            GroupBase group = createdGroups[createdGroupIndex];

            foreach (TableRow row in table.Rows)
            {
                row["Match index"].Should().NotBeNullOrEmpty();
                row["Player 1 name"].Should().NotBeNull();
                row["Player 2 name"].Should().NotBeNull();

                ParseBracketGroupMatchSetup(row, out int matchIndex, out string player1Name, out string player2Name);

                group.Matches[matchIndex].Player1.Name.Should().Be(player1Name);
                group.Matches[matchIndex].Player2.Name.Should().Be(player2Name);
            }
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
