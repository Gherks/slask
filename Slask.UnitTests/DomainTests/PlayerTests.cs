using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class PlayerTests
    {
        [Fact]
        public void CanCreatePlayer()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();
            Match match = group.Matches.First();
            Player player = match.Player1;

            player.Should().NotBeNull();
            player.Id.Should().NotBeEmpty();
            player.PlayerReference.Should().NotBeNull();
            player.Score.Should().Be(0);
            player.MatchId.Should().Be(match.Id);
            player.Match.Should().Be(match);
        }

        [Fact]
        public void CanIncrementPlayerScore()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();
            Match match = group.Matches.First();

            match.Player1.IncrementScore();

            match.Player1.Score.Should().Be(1);
        }

        [Fact]
        public void CanDecrementPlayerScore()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();
            Match match = group.Matches.First();

            match.Player1.IncrementScore();
            match.Player1.DecrementScore();

            match.Player1.Score.Should().Be(0);
        }

        [Fact]
        public void CanIncreasePlayerScoreByTwo()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();
            Match match = group.Matches.First();

            match.Player1.IncreaseScore(2);

            match.Player1.Score.Should().Be(2);
        }

        [Fact]
        public void CanDecreasePlayerScoreByTwo()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();
            Match match = group.Matches.First();

            match.Player1.IncreaseScore(2);
            match.Player1.DecreaseScore(2);

            match.Player1.Score.Should().Be(0);
        }

        [Fact]
        public void CannotDecrementPlayerScoreBelowZero()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();
            Match match = group.Matches.First();

            match.Player1.DecrementScore();

            match.Player1.Score.Should().Be(0);
        }

        [Fact]
        public void CannotDecreasePlayerScoreBelowZero()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();
            Match match = group.Matches.First();

            match.Player1.DecreaseScore(1);

            match.Player1.Score.Should().Be(0);
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
