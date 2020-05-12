﻿using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.RoundTests.RoundTypeTests
{
    public class RoundRobinRoundTests
    {
        private readonly Tournament tournament;

        public RoundRobinRoundTests()
        {
            tournament = Tournament.Create("GSL 2019");
        }

        [Fact]
        public void CanCreateRoundRobinRound()
        {
            RoundRobinRound round = RoundRobinRound.Create(tournament);

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be("Round A");
            round.PlayersPerGroupCount.Should().Be(2);
            round.BestOf.Should().Be(3);
            round.AdvancingPerGroupCount.Should().Be(1);
            round.Groups.Should().HaveCount(0);
            round.TournamentId.Should().Be(tournament.Id);
            round.Tournament.Should().Be(tournament);
        }
	}
}
