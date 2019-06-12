using FluentAssertions;
using Slask.Domain;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class MatchPlayerTests
    {

        [Fact]
        public void ScoreInitiallyAtZero()
        {
            MatchPlayer matchPlayer = WhenMatchPlayerCreated();

            matchPlayer.Score.Should().Be(0);
        }

        [Fact]
        public void CanIncrementScore()
        {
            MatchPlayer matchPlayer = WhenMatchPlayerCreated();

            matchPlayer.IncrementScore();
            matchPlayer.Score.Should().Be(1);
        }

        [Fact]
        public void CanDecrementScore()
        {
            MatchPlayer matchPlayer = WhenMatchPlayerCreated();

            matchPlayer.DecrementScore();
            matchPlayer.Score.Should().Be(-1);
        }

        [Fact]
        public void CanIncreaseScoreByFive()
        {
            MatchPlayer matchPlayer = WhenMatchPlayerCreated();

            matchPlayer.AddScore(5);
            matchPlayer.Score.Should().Be(-5);
        }

        [Fact]
        public void CanDecreaseScoreByFive()
        {
            MatchPlayer matchPlayer = WhenMatchPlayerCreated();

            matchPlayer.SubtractScore(5);
            matchPlayer.Score.Should().Be(-5);
        }

        private MatchPlayer WhenMatchPlayerCreated()
        {
            Player player = Player.Create("Maru");
            return MatchPlayer.Create(player);
        }
    }
}
