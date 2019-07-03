using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class PlayerTests
    {
        [Fact]
        public void EnsurePlayerIsValidWhenAddedToTournament()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.Player1.Should().NotBeNull();
            match.Player1.Name.Should().Be("Maru");
            match.Player1.Score.Should().Be(0);
            match.Player1.Match.Should().NotBeNull();
        }

        [Fact]
        public void PlayerCanBeRenamed()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.Player1.RenameTo("Taeja");

            match.Player1.Name.Should().Be("Taeja");
        }

        [Fact]
        public void PlayerCannotBeRenamedToEmptyName()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.Player1.RenameTo("");

            match.Player1.Name.Should().Be("Maru");
        }

        [Fact]
        public void PlayerCannotBeRenamedToSameNameAsOpponentNoMatterLetterCasing()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.Player1.RenameTo(match.Player2.Name.ToUpper());

            match.Player1.Name.Should().Be("Maru");
        }

        [Fact]
        public void ScoreInitiallyAtZero()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.Player1.Score.Should().Be(0);
        }

        [Fact]
        public void CanIncrementScore()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.Player1.IncrementScore();

            match.Player1.Score.Should().Be(1);
        }

        [Fact]
        public void CanDecrementScore()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.Player1.DecrementScore();

            match.Player1.Score.Should().Be(-1);
        }

        [Fact]
        public void CanIncreaseScoreByFive()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.Player1.SubtractScore(5);

            match.Player1.Score.Should().Be(-5);
        }

        [Fact]
        public void CanDecreaseScoreByFive()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.Player1.SubtractScore(5);

            match.Player1.Score.Should().Be(-5);
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
