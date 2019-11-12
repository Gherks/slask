using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using System;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class RoundTests
    {
        // ADVANCING PLAYERS MUST ALWAYS BE EQUAL OR LESS THAN NUMBER OF GROUPS IN ROUND

        [Fact]
        public void CanCreateRoundRobinRound()
        {
            TournamentServiceContext services = GivenServices();
            Round round = HomestoryCupSetup.Part03AddRoundRobinRound(services);

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be("Round Robin Round");
            round.Type.Should().Be(RoundType.RoundRobin);
            round.BestOf.Should().Be(3);
            round.AdvancingPerGroupAmount.Should().Be(4);
            round.Groups.Should().BeEmpty();
            round.TournamentId.Should().NotBeEmpty();
            round.Tournament.Should().NotBeNull();
        }

        [Fact]
        public void CanCreateDualTournamentRound()
        {
            TournamentServiceContext services = GivenServices();
            Round round = BHAOpenSetup.Part03AddDualTournamentRound(services);

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be("Dual Tournament Round");
            round.Type.Should().Be(RoundType.DualTournament);
            round.BestOf.Should().Be(3);
            round.AdvancingPerGroupAmount.Should().Be(2);
            round.Groups.Should().BeEmpty();
            round.TournamentId.Should().NotBeEmpty();
            round.Tournament.Should().NotBeNull();
        }

        [Fact]
        public void CanCreateBracketRound()
        {
            TournamentServiceContext services = GivenServices();
            Round round = HomestoryCupSetup.Part10AddBracketRound(services);

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be("Bracket Round");
            round.Type.Should().Be(RoundType.Bracket);
            round.BestOf.Should().Be(5);
            round.AdvancingPerGroupAmount.Should().Be(1);
            round.Groups.Should().BeEmpty();
            round.TournamentId.Should().NotBeEmpty();
            round.Tournament.Should().NotBeNull();
        }

        [Fact]
        public void CannotCreateRoundsWithoutName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part01CreateTournament(services);
            Round roundRobinRound = tournament.AddRound(RoundType.RoundRobin, "", 3, 1);
            Round dualTournamentRound = tournament.AddRound(RoundType.DualTournament, "", 3);
            Round bracketRound = tournament.AddRound(RoundType.Bracket, "", 3);

            roundRobinRound.Should().BeNull();
            dualTournamentRound.Should().BeNull();
            bracketRound.Should().BeNull();
        }

        [Fact]
        public void CannotCreateRoundsWithZeroAdvancers()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part01CreateTournament(services);
            Round roundRobinRound = tournament.AddRound(RoundType.RoundRobin, "Round Robin Round", 3, 0);
            Round dualTournamentRound = tournament.AddRound(RoundType.DualTournament, "Dual Tournament Round", 3);
            Round bracketRound = tournament.AddRound(RoundType.Bracket, "Bracket Round", 3);

            roundRobinRound.Should().BeNull();
            dualTournamentRound.AdvancingPerGroupAmount.Should().NotBe(0);
            bracketRound.AdvancingPerGroupAmount.Should().NotBe(0);
        }

        [Fact]
        public void CannotCreateRoundsWithEvenBestOfs()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part01CreateTournament(services);

            for (int bestOf = 0; bestOf < 21; bestOf += 2)
            {
                tournament.AddRound(RoundType.RoundRobin, "Round Robin Round", bestOf, 4).Should().BeNull();
                tournament.AddRound(RoundType.DualTournament, "Dual Tournament Round", bestOf).Should().BeNull();
                tournament.AddRound(RoundType.Bracket, "Round Robin Round", bestOf).Should().BeNull();
            }
        }

        [Fact]
        public void ReturnsNullWhenFetchingPreviousRoundWithFirstRound()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part04AddedGroupToRoundRobinRound(services);

            Round round = group.Round.GetPreviousRound();

            round.Should().BeNull();
        }

        [Fact]
        public void CanFetchPreviousRoundFromRoundWithRoundPredecessor()
        {
            TournamentServiceContext services = GivenServices();
            Round currentRound = HomestoryCupSetup.Part10AddBracketRound(services);
            Tournament tournament = currentRound.Tournament;

            Round previousRound = currentRound.GetPreviousRound();

            previousRound.Should().NotBeNull();
            previousRound.Should().Be(tournament.Rounds.First());
        }

        [Fact]
        public void OnlyWinningPlayersCanAdvanceToNextRound()
        {
            TournamentServiceContext services = GivenServices();
            BracketGroup group = HomestoryCupSetup.Part12AddWinningPlayersToBracketGroup(services);

            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == "Taeja").Should().NotBeNull();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == "FanTaSy").Should().NotBeNull();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == "Thorzain").Should().NotBeNull();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == "Rain").Should().NotBeNull();
        }

        [Fact]
        public void SolveRoundRobinTies()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CannotAddGroupsToRoundRobinRoundThatDoesNotMatchByType()
        {
            TournamentServiceContext services = GivenServices();
            Round round = HomestoryCupSetup.Part03AddRoundRobinRound(services);
            round.AddGroup();

            RoundRobinGroup group = round.Groups.First() as RoundRobinGroup;

            round.Groups.Should().HaveCount(1);
            group.Should().NotBeOfType<DualTournamentGroup>();
            group.Should().NotBeOfType<BracketGroup>();
            group.Should().BeOfType<RoundRobinGroup>();
        }

        [Fact]
        public void CannotAddGroupsToDualTournamentRoundThatDoesNotMatchByType()
        {
            TournamentServiceContext services = GivenServices();
            Round round = BHAOpenSetup.Part03AddDualTournamentRound(services);
            round.AddGroup();

            DualTournamentGroup group = round.Groups.First() as DualTournamentGroup;

            round.Groups.Should().HaveCount(1);
            group.Should().NotBeOfType<RoundRobinGroup>();
            group.Should().NotBeOfType<BracketGroup>();
            group.Should().BeOfType<DualTournamentGroup>();
        }

        [Fact]
        public void CannotAddGroupsToBracketRoundThatDoesNotMatchByType()
        {
            TournamentServiceContext services = GivenServices();
            Round round = HomestoryCupSetup.Part10AddBracketRound(services);
            round.AddGroup();

            BracketGroup group = round.Groups.First() as BracketGroup;

            round.Groups.Should().HaveCount(1);
            group.Should().NotBeOfType<RoundRobinGroup>();
            group.Should().NotBeOfType<DualTournamentGroup>();
            group.Should().BeOfType<BracketGroup>();
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
