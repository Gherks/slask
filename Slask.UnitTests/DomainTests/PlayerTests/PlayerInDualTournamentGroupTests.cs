﻿using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.PlayerTests
{
    public class PlayerInDualTournamentGroupTests
    {
        private Tournament tournament;
        private DualTournamentRound round;
        private DualTournamentGroup group;
        private PlayerReference playerReference;
        private Match match;

        public PlayerInDualTournamentGroupTests()
        {
            tournament = Tournament.Create("GSL 2019");
            round = tournament.AddDualTournamentRound("Bracket round", 7) as DualTournamentRound;
            playerReference = round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            group = round.Groups.First() as DualTournamentGroup;
            match = group.Matches.First();
        }

        [Fact]
        public void CanCreatePlayer()
        {
            match.Player1.Should().NotBeNull();
            match.Player1.Id.Should().NotBeEmpty();
            match.Player1.PlayerReference.Should().Be(playerReference);
            match.Player1.Score.Should().Be(0);
            match.Player1.MatchId.Should().Be(match.Id);
            match.Player1.Match.Should().Be(match);
        }

        [Fact]
        public void CanIncreaseScore()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            int score = 1;

            match.Player1.IncreaseScore(score);

            match.Player1.Score.Should().Be(score);
        }

        [Fact]
        public void CanDecrementPlayerScore()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            match.Player1.IncreaseScore(2);
            match.Player1.DecreaseScore(1);

            match.Player1.Score.Should().Be(1);
        }

        [Fact]
        public void CannotDecreasePlayerScoreBelowZero()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            match.Player1.DecreaseScore(1);

            match.Player1.Score.Should().Be(0);
        }
    }
}
