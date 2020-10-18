using FluentAssertions;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.SpecFlow.IntegrationTests.GroupTests;
using Slask.Domain.Utilities.StandingsSolvers;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

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
            foreach (BetterPlacesMatchBet betterPlacesMatchBet in table.CreateSet<BetterPlacesMatchBet>())
            {
                RoundBase round = createdRounds[betterPlacesMatchBet.RoundIndex];
                GroupBase group = createdGroups[betterPlacesMatchBet.GroupIndex];
                Match match = group.Matches[betterPlacesMatchBet.MatchIndex];

                bool betterNameIsNotEmpty = betterPlacesMatchBet.BetterName.Length > 0;
                bool playerNameIsNotEmpty = betterPlacesMatchBet.PlayerName.Length > 0;

                if (betterNameIsNotEmpty && playerNameIsNotEmpty)
                {
                    Better better = round.Tournament.GetBetterByName(betterPlacesMatchBet.BetterName);
                    Player player = match.FindPlayer(betterPlacesMatchBet.PlayerName);

                    better.Should().NotBeNull();
                    player.Should().NotBeNull();

                    better.PlaceMatchBet(match, player).Should().NotBeNull();
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

            betterStandings.Should().HaveCount(table.RowCount);

            for (int index = 0; index < table.RowCount; ++index)
            {
                BetterStanding betterStanding = table.Rows[index].CreateInstance<BetterStanding>();

                betterStandings[index].Object.User.Name.Should().Be(betterStanding.BetterName);
                betterStandings[index].Points.Should().Be(betterStanding.Points);
            }
        }

        private sealed class BetterPlacesMatchBet
        {
            public string BetterName { get; set; }
            public int RoundIndex { get; set; }
            public int GroupIndex { get; set; }
            public int MatchIndex { get; set; }
            public string PlayerName { get; set; }
        }

        private sealed class BetterStanding
        {
            public string BetterName { get; set; }
            public int Points { get; set; }
        }
    }
}