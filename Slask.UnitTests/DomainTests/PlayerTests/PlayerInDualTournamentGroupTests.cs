﻿using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.PlayerTests
{
    public class PlayerInDualTournamentGroupTests
    {
        List<string> playerNames = new List<string> { "Maru", "Stork", "Taeja", "Rain" };

        private readonly Tournament tournament;
        private readonly DualTournamentRound round;
        private readonly DualTournamentGroup group;
        private readonly Match match;
        private readonly Player player;

        public PlayerInDualTournamentGroupTests()
        {
            tournament = Tournament.Create("GSL 2019");
            round = tournament.AddDualTournamentRound() as DualTournamentRound;
            round.SetBestOf(5);

            foreach (string playerName in playerNames)
            {
                round.RegisterPlayerReference(playerName);
            }

            round.RegisterPlayerReference("Stork");
            group = round.Groups.First() as DualTournamentGroup;
            match = group.Matches.First();
            player = match.Player1;
        }

        [Fact]
        public void CanCreatePlayer()
        {
            player.Should().NotBeNull();
            player.Id.Should().NotBeEmpty();
            player.PlayerReference.Name.Should().Be(playerNames.First());
            player.Score.Should().Be(0);
            player.MatchId.Should().Be(match.Id);
            player.Match.Should().Be(match);
        }

        [Fact]
        public void CanIncreaseScore()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            int score = 1;

            player.IncreaseScore(score);

            player.Score.Should().Be(score);
        }

        [Fact]
        public void CanDecrementPlayerScore()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            player.IncreaseScore(2);
            player.DecreaseScore(1);

            player.Score.Should().Be(1);
        }

        [Fact]
        public void CannotDecreasePlayerScoreBelowZero()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            player.DecreaseScore(1);

            player.Score.Should().Be(0);
        }
    }
}
