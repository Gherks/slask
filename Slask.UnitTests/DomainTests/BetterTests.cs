using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Bets;
using Slask.TestCore;
using System;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class BetterTests
    {
        [Fact]
        public void CanCreateBetter()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part02BettersAddedToTournament(services);

            Better better = tournament.Betters.First();

            better.Id.Should().NotBeEmpty();
            better.User.Should().NotBeNull();
            better.Bets.Should().NotBeEmpty();
            better.TournamentId.Should().NotBeEmpty();
            better.Tournament.Should().NotBeNull();
        }

        [Fact]
        public void ReturnNullIfMissingUserWhenCreatingBetter()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part01CreateTournament(services);

            Better better = Better.Create(null, tournament);

            better.Should().BeNull();
        }

        [Fact]
        public void ReturnNullIfMissingTournamentWhenCreatingBetter()
        {
            TournamentServiceContext services = GivenServices();
            HomestoryCupSetup.Part01CreateTournament(services);
            services.WhenCreatedUsers();

            User user = services.UserService.GetUserByName("Stålberto");
            Better better = Better.Create(user, null);

            better.Should().BeNull();
        }

        [Fact]
        public void BetterCanPlaceMatchBet()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part06StartDateTimeSetToMatchesInRoundRobinGroup(services);

            Tournament tournament = group.Round.Tournament;
            Better better = tournament.Betters.First();
            Match match = group.Matches.First();

            bool result = better.PlaceMatchBet(match, match.Player1);

            result.Should().BeTrue();
            better.Bets.Should().HaveCount(1);

            MatchBet bet = better.Bets.First() as MatchBet;
            bet.Match.Should().Be(match);
            bet.PlayerId.Should().Be(match.Player1.Id);
        }

        [Fact]
        public void BetterCannotPlaceAMatchBetWithoutMatch()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part06StartDateTimeSetToMatchesInRoundRobinGroup(services);

            Tournament tournament = group.Round.Tournament;
            Better better = tournament.Betters.First();
            Match match = group.Matches.First();

            bool result = better.PlaceMatchBet(null, match.Player1);

            result.Should().BeFalse();
            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void BetterCannotPlaceAMatchBetWithoutPlayer()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part06StartDateTimeSetToMatchesInRoundRobinGroup(services);

            Tournament tournament = group.Round.Tournament;
            Better better = tournament.Betters.First();
            Match match = group.Matches.First();

            bool result = better.PlaceMatchBet(match, null);

            result.Should().BeFalse();
            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void BetterCannotPlaceAMatchBetOnMatchThatIsNotReady()
        {
            TournamentServiceContext services = GivenServices();
            throw new NotImplementedException();
        }

        [Fact]
        public void BetterCanFindExistingMatchBet()
        {
            TournamentServiceContext services = GivenServices();
            throw new NotImplementedException();
        }

        [Fact]
        public void ReturnsNullWhenTryingToFindNonexistingMatchBet()
        {
            TournamentServiceContext services = GivenServices();
            throw new NotImplementedException();
        }

        [Fact]
        public void CanCreateMatchBetWhenPlacingBetOnNewMatch()
        {
            TournamentServiceContext services = GivenServices();
            throw new NotImplementedException();
        }

        [Fact]
        public void CanUpdateMatchBetWhenPlacingBetOnExistingMatchBet()
        {
            TournamentServiceContext services = GivenServices();
            throw new NotImplementedException();
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
