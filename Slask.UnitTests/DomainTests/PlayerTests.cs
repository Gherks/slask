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
        public void EnsurePlayerIsValidWhenAddedToMatchInTournament()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.Rounds.First().Groups.First().Matches.First();
            Player player = match.Player1;

            player.Should().NotBeNull();
            player.PlayerReference.Should().BeNull();
            player.Score.Should().Be(0);
            player.MatchId.Should().Be(match.Id);
            player.Match.Should().Be(match);
        }

        [Fact]
        public void ScoreInitiallyAtZero()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.Rounds.First().Groups.First().Matches.First();

            match.Player1.Score.Should().Be(0);
        }

        [Fact]
        public void CanIncrementScore()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.Rounds.First().Groups.First().Matches.First();

            match.Player1.IncrementScore();

            match.Player1.Score.Should().Be(1);
        }

        [Fact]
        public void CanDecrementScore()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.Rounds.First().Groups.First().Matches.First();

            match.Player1.DecrementScore();

            match.Player1.Score.Should().Be(-1);
        }

        [Fact]
        public void CanIncreaseScoreByFive()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.Rounds.First().Groups.First().Matches.First();

            match.Player1.SubtractScore(5);

            match.Player1.Score.Should().Be(-5);
        }

        [Fact]
        public void CanDecreaseScoreByFive()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.Rounds.First().Groups.First().Matches.First();

            match.Player1.SubtractScore(5);

            match.Player1.Score.Should().Be(-5);
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
