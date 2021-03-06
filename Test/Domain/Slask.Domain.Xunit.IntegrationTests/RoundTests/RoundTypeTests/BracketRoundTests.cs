﻿using FluentAssertions;
using Slask.Domain.Rounds.RoundTypes;
using System.Linq;
using Xunit;

namespace Slask.Domain.Xunit.IntegrationTests.RoundTests.RoundTypeTests
{
    public class BracketRoundTests
    {
        private readonly Tournament tournament;

        public BracketRoundTests()
        {
            tournament = Tournament.Create("GSL 2019");
        }

        [Fact]
        public void CanCreateBracketRound()
        {
            BracketRound round = tournament.AddBracketRound();

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be("Round A");
            round.PlayersPerGroupCount.Should().Be(2);
            round.AdvancingPerGroupCount.Should().Be(1);
            round.Groups.Should().HaveCount(1);
            round.TournamentId.Should().Be(tournament.Id);
            round.Tournament.Should().Be(tournament);
        }

        [Fact]
        public void AdvancingCountInBracketRoundsCannotBeAnythingOtherThanOne()
        {
            BracketRound round = tournament.AddBracketRound();

            for (int advancingPerGroupCount = -5; advancingPerGroupCount < 16; ++advancingPerGroupCount)
            {
                round.SetAdvancingPerGroupCount(advancingPerGroupCount);
                round.AdvancingPerGroupCount.Should().Be(1);
            }
        }

        [Fact]
        public void CannotChangePlayersPerGroupSize()
        {
            BracketRound round = tournament.AddBracketRound();

            round.Groups.First().Matches.Should().HaveCount(1);
            round.PlayersPerGroupCount.Should().Be(2);

            round.SetPlayersPerGroupCount(8);

            round.Groups.First().Matches.Should().HaveCount(7);
            round.PlayersPerGroupCount.Should().Be(8);
        }

        [Fact]
        public void CannotSetPlayersPerGroupSizeToAnythingLessThanTwo()
        {
            BracketRound round = tournament.AddBracketRound();

            round.Groups.First().Matches.Should().HaveCount(1);
            round.PlayersPerGroupCount.Should().Be(2);

            round.SetPlayersPerGroupCount(1);
            round.SetPlayersPerGroupCount(0);
            round.SetPlayersPerGroupCount(-1);

            round.Groups.First().Matches.Should().HaveCount(1);
            round.PlayersPerGroupCount.Should().Be(2);
        }
    }
}
