using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class RoundTests
    {
        [Fact]
        public void CanCreateRoundRobinRound()
        {
            TournamentServiceContext services = GivenServices();
            Round round = services.HomestoryCup_03_AddRoundRobinRound();

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be("Round Robin Round");
            round.Type.Should().Be(RoundType.RoundRobin);
            round.BestOf.Should().Be(3);
            round.AdvanceAmount.Should().Be(4);
            round.Groups.Should().BeEmpty();
            round.TournamentId.Should().NotBeEmpty();
            round.Tournament.Should().NotBeNull();
        }

        [Fact]
        public void CanCreateDualTournamentRound()
        {
            TournamentServiceContext services = GivenServices();
            Round round = services.BHAOpen_03_AddDualTournamentRound();

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be("Dual Tournament Round");
            round.Type.Should().Be(RoundType.DualTournament);
            round.BestOf.Should().Be(3);
            round.AdvanceAmount.Should().Be(2);
            round.Groups.Should().BeEmpty();
            round.TournamentId.Should().NotBeEmpty();
            round.Tournament.Should().NotBeNull();
        }

        [Fact]
        public void CanCreateBracketRound()
        {
            TournamentServiceContext services = GivenServices();
            Round round = services.HomestoryCup_11_AddBracketRound();

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be("Bracket Round");
            round.Type.Should().Be(RoundType.Bracket);
            round.BestOf.Should().Be(5);
            round.AdvanceAmount.Should().Be(1);
            round.Groups.Should().BeEmpty();
            round.TournamentId.Should().NotBeEmpty();
            round.Tournament.Should().NotBeNull();
        }

        [Fact]
        public void CannotCreateRoundsWithEvenBestOfs()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.HomestoryCup_01_CreateTournament();

            for (int bestOf = 0; bestOf < 21; bestOf += 2)
            {
                tournament.AddRoundRobinRound("Round Robin Round", bestOf, 4).Should().BeNull();
                tournament.AddDualTournamentRound("Dual Tournament Round", bestOf).Should().BeNull();
                tournament.AddBracketRound("Round Robin Round", bestOf).Should().BeNull();
            }
        }

        [Fact]
        public void ReturnsNullWhenFetchingPreviousRoundWithFirstRound()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_04_AddGroupToRoundRobinRound();

            Round round = group.Round.GetPreviousRound();

            round.Should().BeNull();
        }

        [Fact]
        public void CanFetchPreviousRoundFromRoundWithRoundPredecessor()
        {
            TournamentServiceContext services = GivenServices();
            Round currentRound = services.HomestoryCup_11_AddBracketRound();

            Round previousRound = currentRound.GetPreviousRound();

            previousRound.Should().NotBeNull();
        }

        [Fact]
        public void OnlyWinningPlayersCanAdvanceToNextRound()
        {
            TournamentServiceContext services = GivenServices();
            BracketGroup group = services.HomestoryCup_13_AddWinningPlayersToBracketGroup();

            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == "").Should().NotBeNull();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == "").Should().NotBeNull();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == "").Should().NotBeNull();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == "").Should().NotBeNull();
        }

        [Fact]
        public void CannotAddGroupsToRoundThatDoesNotMatchByType()
        {
            TournamentServiceContext services = GivenServices();
            Round round = services.HomestoryCup_03_AddRoundRobinRound();

            round.AddGroup();

            round.Groups.Should().NotBeNull();
            round.Groups.Count.Should().Be(0);
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
