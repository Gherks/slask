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
        public void TournamentCanAddRound()
        {
            TournamentServiceContext services = GivenServices();
            Round round = services.HomestoryCup_03_AddRoundRobinRound();

            round.Should().NotBeNull();
            round.Name.Should().Be("Round Robin Round");
            (round.BestOf % 2).Should().NotBe(0);

            // COMPLETE
        }

        [Fact]
        public void CanCreateDualTournamentRound()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CanCreateBracketRound()
        {
            TournamentServiceContext services = GivenServices();
            BracketGroup group = services.HomestoryCup_12_AddGroupToBracketRound();

            group.Should().NotBeNull();
            group.Id.Should().NotBeEmpty();
            group.IsReady.Should().BeFalse();
            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().BeEmpty();
            group.RoundId.Should().Be(group.Round.Id);
            group.Round.Should().Be(group.Round);
        }

        [Fact]
        public void OnlyWinningPlayersCanAdvanceToNextRound()
        {
            TournamentServiceContext services = GivenServices();
            BracketGroup group = services.HomestoryCup_13_AddWinningPlayersToBracketGroup();

            group.ParticipatingPlayers.Where(playerReference => playerReference.Name == "").FirstOrDefault().Should().NotBeNull();
            group.ParticipatingPlayers.Where(playerReference => playerReference.Name == "").FirstOrDefault().Should().NotBeNull();
            group.ParticipatingPlayers.Where(playerReference => playerReference.Name == "").FirstOrDefault().Should().NotBeNull();
            group.ParticipatingPlayers.Where(playerReference => playerReference.Name == "").FirstOrDefault().Should().NotBeNull();
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
