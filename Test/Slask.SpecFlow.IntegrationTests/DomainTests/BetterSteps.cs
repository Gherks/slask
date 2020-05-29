using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities.StandingsSolvers;
using Slask.SpecFlow.IntegrationTests.DomainTests.RoundTests;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests
{
    [Binding, Scope(Feature = "Better")]
    public class BetterSteps : BetterStepDefinitions
    {

    }

    public class BetterStepDefinitions : RoundStepDefinitions
    {
        [Given(@"betters places match bets")]
        [When(@"betters places match bets")]
        public void WhenBettersPlacesMatchBets(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                ParseBetterMatchBetPlacements(row, out string betterName, out int roundIndex, out int groupIndex, out int matchIndex, out string playerName);

                RoundBase round = createdRounds[roundIndex];
                GroupBase group = createdGroups[groupIndex];
                Match match = group.Matches[matchIndex];

                bool betterNameIsNotEmpty = betterName.Length > 0;
                bool playerNameIsNotEmpty = playerName.Length > 0;

                if (betterNameIsNotEmpty && playerNameIsNotEmpty)
                {
                    Better better = round.Tournament.GetBetterByName(betterName);
                    Player player = match.FindPlayer(playerName);

                    better.Should().NotBeNull();
                    player.Should().NotBeNull();

                    better.PlaceMatchBet(match, player).Should().BeTrue();
                }
            }
        }

        [Then(@"player standings in tournament (.*) from first to last looks like this")]
        public void ThenPlayerStandingsFromFirstToLastLooksLikeThis(int tournamentIndex, Table table)
        {
            Tournament tournament = createdTournaments[tournamentIndex];

            List<BetterStandingsEntry> betterStandings = BetterStandingsSolver.FetchFrom(tournament);

            betterStandings.Should().HaveCount(table.Rows.Count);

            for(int index = 0; index < table.Rows.Count; ++index)
            {
                ParseBetterStandings(table.Rows[index], out string betterName, out int points);

                betterStandings[index].Object.User.Name.Should().Be(betterName);
                betterStandings[index].Points.Should().Be(points);
            }
        }

        private void ParseBetterMatchBetPlacements(TableRow row, out string betterName, out int roundIndex, out int groupIndex, out int matchIndex, out string playerName)
        {
            betterName = "";
            roundIndex = -1;
            groupIndex = -1;
            matchIndex = -1;
            playerName = "";

            if (row.ContainsKey("Better name"))
            {
                betterName = row["Better name"];
            }

            if (row.ContainsKey("Round index"))
            {
                int.TryParse(row["Round index"], out roundIndex);
            }

            roundIndex.Should().BeGreaterOrEqualTo(0);

            if (row.ContainsKey("Group index"))
            {
                int.TryParse(row["Group index"], out groupIndex);
            }

            groupIndex.Should().BeGreaterOrEqualTo(0);

            if (row.ContainsKey("Match index"))
            {
                int.TryParse(row["Match index"], out matchIndex);
            }

            matchIndex.Should().BeGreaterOrEqualTo(0);

            if (row.ContainsKey("Player name"))
            {
                playerName = row["Player name"];
            }
        }

        private void ParseBetterStandings(TableRow row, out string betterName, out int points)
        {
            betterName = "";
            points = -1;

            if (row.ContainsKey("Better name"))
            {
                betterName = row["Better name"];
            }

            if (row.ContainsKey("Points"))
            {
                int.TryParse(row["Points"], out points);
            }

            points.Should().BeGreaterOrEqualTo(0);
        }
    }
}