using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class PlayerTests
    {
        // CAN SET UP PLAYERS IN EACH GROUP TYPE

        //[Fact]
        //public void CanCreatePlayerInRoundRobinGroup()
        //{
        //    TournamentServiceContext services = GivenServices();
        //    RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup();
        //    Match match = group.Matches.First();
        //    Player player = match.Player1;

        //    player.Should().NotBeNull();
        //    player.Id.Should().NotBeEmpty();
        //    player.PlayerReference.Should().NotBeNull();
        //    player.Score.Should().Be(0);
        //    player.MatchId.Should().Be(match.Id);
        //    player.Match.Should().Be(match);
        //}

        //[Fact]
        //public void CanCreatePlayerInDualTournamentGroup()
        //{
        //    TournamentServiceContext services = GivenServices();
        //    DualTournamentGroup group = BHAOpenSetup.Part05_AddedPlayersToDualTournamentGroups().First();
        //    Match match = group.Matches.First();
        //    Player player = match.Player1;

        //    player.Should().NotBeNull();
        //    player.Id.Should().NotBeEmpty();
        //    player.PlayerReference.Should().NotBeNull();
        //    player.Score.Should().Be(0);
        //    player.MatchId.Should().Be(match.Id);
        //    player.Match.Should().Be(match);
        //}

        //[Fact]
        //public void CanCreatePlayerInBracketGroup()
        //{
        //    TournamentServiceContext services = GivenServices();
        //    BracketGroup group = HomestoryCupSetup.Part12_AddWinningPlayersToBracketGroup();
        //    Match match = group.Matches.First();
        //    Player player = match.Player1;

        //    player.Should().NotBeNull();
        //    player.Id.Should().NotBeEmpty();
        //    player.PlayerReference.Should().NotBeNull();
        //    player.Score.Should().Be(0);
        //    player.MatchId.Should().Be(match.Id);
        //    player.Match.Should().Be(match);
        //}

        [Fact]
        public void CanAssignPlayerReference()
        {
            TournamentServiceContext services = GivenServices();
            DualTournamentGroup group = BHAOpenSetup.Part04_AddGroupsToDualTournamentRound(services).First();
            Player player = group.Matches.First().Player1;

            PlayerReference playerReference = PlayerReference.Create("Maru", group.Round.Tournament);

            player.SetPlayerReference(playerReference);
            player.PlayerReference.Should().Be(playerReference);
        }

        [Fact]
        public void CanChangePlayerReferenceToAnotherPlayerReference()
        {
            TournamentServiceContext services = GivenServices();
            DualTournamentGroup group = BHAOpenSetup.Part04_AddGroupsToDualTournamentRound(services).First();
            Player player = group.Matches.First().Player1;

            PlayerReference maruPlayerReference = PlayerReference.Create("Maru", group.Round.Tournament);
            PlayerReference storkPlayerReference = PlayerReference.Create("Stork", group.Round.Tournament);

            player.SetPlayerReference(maruPlayerReference);
            player.SetPlayerReference(storkPlayerReference);
            player.PlayerReference.Should().Be(storkPlayerReference);
        }

        [Fact]
        public void CanSetExistingPlayerReferenceToNull()
        {
            TournamentServiceContext services = GivenServices();
            DualTournamentGroup group = BHAOpenSetup.Part04_AddGroupsToDualTournamentRound(services).First();
            Player player = group.Matches.First().Player1;

            PlayerReference playerReference = PlayerReference.Create("Maru", group.Round.Tournament);

            player.SetPlayerReference(playerReference);
            player.SetPlayerReference(null);
            player.PlayerReference.Should().BeNull();
        }

        //[Fact]
        //public void CanIncrementPlayerScore()
        //{
        //    TournamentServiceContext services = GivenServices();
        //    RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup();
        //    Match match = group.Matches.First();

        //    match.Player1.IncrementScore();

        //    match.Player1.Score.Should().Be(1);
        //}

        //[Fact]
        //public void CanDecrementPlayerScore()
        //{
        //    TournamentServiceContext services = GivenServices();
        //    RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup();
        //    Match match = group.Matches.First();

        //    match.Player1.IncrementScore();
        //    match.Player1.DecrementScore();

        //    match.Player1.Score.Should().Be(0);
        //}

        [Fact]
        public void CanIncreasePlayerScoreByTwo()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup(services);
            Match match = group.Matches.First();

            match.Player1.IncreaseScore(2);

            match.Player1.Score.Should().Be(2);
        }

        [Fact]
        public void CanDecreasePlayerScoreByTwo()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup(services);
            Match match = group.Matches.First();

            match.Player1.IncreaseScore(2);
            match.Player1.DecreaseScore(2);

            match.Player1.Score.Should().Be(0);
        }

        //[Fact]
        //public void CannotDecrementPlayerScoreBelowZero()
        //{
        //    TournamentServiceContext services = GivenServices();
        //    RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup();
        //    Match match = group.Matches.First();

        //    match.Player1.DecrementScore();

        //    match.Player1.Score.Should().Be(0);
        //}

        [Fact]
        public void CannotDecreasePlayerScoreBelowZero()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup(services);
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
