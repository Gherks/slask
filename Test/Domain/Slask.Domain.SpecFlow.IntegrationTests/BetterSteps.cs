using FluentAssertions;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.SpecFlow.IntegrationTests.GroupTests;
using Slask.Domain.Utilities.StandingsSolvers;
using Slask.TestCore;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.Domain.SpecFlow.IntegrationTests
{
    [Binding, Scope(Feature = "Better")]
    public class BetterSteps : BetterStepDefinitions
    {

    }

    public class BetterStepDefinitions : GroupStepDefinitions
    {
        [Given(@"betters places match bets")]
        [When(@"betters places match bets")]
        public void GivenBettersPlacesMatchBets(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                TestUtilities.ParseBetterMatchBetPlacements(row, out string betterName, out int roundIndex, out int groupIndex, out int matchIndex, out string playerName);

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

        [Given(@"better standings in tournament (.*) from first to last looks like this")]
        [When(@"better standings in tournament (.*) from first to last looks like this")]
        [Then(@"better standings in tournament (.*) from first to last looks like this")]
        public void GivenBetterStandingsInTournamentFromFirstToLastLooksLikeThis(int tournamentIndex, Table table)
        {
            Tournament tournament = createdTournaments[tournamentIndex];

            List<StandingsEntry<Better>> betterStandings = tournament.GetBetterStandings();

            betterStandings.Should().HaveCount(table.Rows.Count);

            for (int index = 0; index < table.Rows.Count; ++index)
            {
                TestUtilities.ParseBetterStandings(table.Rows[index], out string betterName, out int points);

                betterStandings[index].Object.User.Name.Should().Be(betterName);
                betterStandings[index].Points.Should().Be(points);
            }
        }
    }
}