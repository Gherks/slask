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
        public void CanCreateRoundInTournament()
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
        public void CanCreateDualTournamentRound()
        {
            throw new NotImplementedException();
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
        public void FetchingWinningPlayersWithFirstRoundReturnsEmptyList()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_04_AddGroupToRoundRobinRound();

            List<Player> fetchedPlayers = group.Round.GetWinningPlayersOfPreviousRound();

            fetchedPlayers.Should().NotBeNull();
            fetchedPlayers.Count.Should().Be(0);
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
