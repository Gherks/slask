using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.TestCore;
using System;
using System.Linq;
using Xunit;

namespace Slask.IntegrationTests.Tests
{
    [Collection("Integration test collection")]
    public class TournamentServiceTests
    {
        [Fact]
        public void CanDetermineMatchStatusCorrectly()
        {

        }

        [Fact]
        public void CanDetermineLoserOfMatch()
        {

        }

        [Fact]
        public void CanAddBetterToTournamentWithUserService()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedBetterInTournament();

            tournament.Betters.First().Should().NotBeNull();
            tournament.Betters.First().User.Should().Be(services.UserService.GetUserByName("Stålberto"));
        }

        [Fact]
        public void CanOnlyAddUserAsBetterOncePerTournament()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedBetterInTournament();
            Better createdBetter = tournament.Betters.First();

            Better duplicateBetter = services.TournamentService.AddBetter(createdBetter.User);

            duplicateBetter.Should().NotBeNull();
        }

        [Fact]
        public void CannotAddSamePlayerTwiceToMatch()
        {
            TournamentServiceContext services = GivenServices();
            Group group = services.WhenAddedGroupToTournament();
            Match match = group.AddMatch("Maru", "Maru", DateTimeHelper.Now.AddSeconds(1));

            match.Should().BeNull();
        }

        [Fact]
        public void BothPlayersMustHaveANameWhenAddingPlayersToMatch()
        {
            TournamentServiceContext services = GivenServices();
            Group group = services.WhenAddedGroupToTournament();
            Match match = group.AddMatch("Maru", "", DateTimeHelper.Now.AddSeconds(1));

            match.Should().BeNull();
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new IntegrationTestSlaskContextCreator());
        }
    }
}
