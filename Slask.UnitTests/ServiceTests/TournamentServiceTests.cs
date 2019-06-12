using FluentAssertions;
using Slask.Domain;
using Slask.UnitTests.TestContexts;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.ServiceTests
{
    public class TournamentServiceTests
    {
        [Fact]
        public void TournamentCanBeCreated()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Tournament tournament = services.WhenCreatedTournament();

            tournament.Should().NotBeNull();
            tournament.Name.Should().Be("WCS 2019");
        }

        [Fact]
        public void CanGetTournamentByName()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Tournament createdTournament = services.WhenCreatedTournament();
            Tournament fetchedTournament = services.TournamentService.GetTournamentByName(createdTournament.Name);

            fetchedTournament.Should().NotBeNull();
            fetchedTournament.Name.Should().Be("WCS 2019");
        }

        [Fact]
        public void CanGetTournamentById()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Tournament createdTournament = services.WhenCreatedTournament();
            Tournament fetchedTournament = services.TournamentService.GetTournamentById(createdTournament.Id);

            fetchedTournament.Should().NotBeNull();
            fetchedTournament.Name.Should().Be("WCS 2019");
        }

        [Fact]
        public void TournamentNameMustBeUnique()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            services.WhenCreatedTournament();
            Tournament duplicateNamedTournament = services.WhenCreatedTournament();

            duplicateNamedTournament.Should().BeNull();
        }

        [Fact]
        public void TournamentCanAddRound()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Round round = services.WhenAddedRoundToTournament();

            round.Should().NotBeNull();
            round.Name.Should().Be("Group A");
            (round.BestOf % 2).Should().NotBe(0);
        }

        [Fact]
        public void TournamentDoesNotAcceptRoundsWithEvenBestOfs()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Tournament tournament = services.WhenCreatedTournament();
            Round round = tournament.AddRound("Group A", 1, 4, 8);

            round.Should().BeNull();
        }

        [Fact]
        public void TournamentMatchMustContainTwoPlayers()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Group group = services.WhenAddedGroupToTournament();

            Player player1 = Player.Create("Maru");
            Player player2 = Player.Create("Stork");
            Match matchMissingFirstPlayer = group.AddMatch(null, MatchPlayer.Create(player2));
            Match matchMissingSecondPlayer = group.AddMatch(MatchPlayer.Create(player1), null);
            Match matchMissingBothPlayers = group.AddMatch(MatchPlayer.Create(player1), MatchPlayer.Create(player2));

            matchMissingFirstPlayer.Should().BeNull();
            matchMissingSecondPlayer.Should().BeNull();
            matchMissingBothPlayers.Should().BeNull();
        }

        [Fact]
        public void TournamentMatchMustContainDifferentMatchPlayers()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Group group = services.WhenAddedGroupToTournament();

            MatchPlayer matchPlayer = MatchPlayer.Create(Player.Create("Maru"));
            Match match = group.AddMatch(matchPlayer, matchPlayer);

            match.Should().BeNull();
        }

        [Fact]
        public void TournamentMatchMustContainDifferentPlayers()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Group group = services.WhenAddedGroupToTournament();

            Player player = Player.Create("Maru");
            Match match = group.AddMatch(MatchPlayer.Create(player), MatchPlayer.Create(player));

            match.Should().BeNull();
        }

        [Fact]
        public void NewPlayerIsAddedToTournamentPlayerListWhenAddedToMatch()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            Tournament tournament = services.TournamentService.GetTournamentByName("WCS 2019");
            tournament.Players.Contains(match.MatchPlayer1.Player).Should().BeTrue();
            tournament.Players.Contains(match.MatchPlayer2.Player).Should().BeTrue();
        }

        [Fact]
        public void CanAddBetterToTournamentWithUserService()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Better better = services.WhenAddedBetterToTournament();

            better.Should().NotBeNull();
        }

        [Fact]
        public void CanOnlyAddUserAsBetterOncePerTournament()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            services.WhenAddedBetterToTournament();
            Better better = services.WhenAddedBetterToTournament();

            better.Should().BeNull();
        }

        [Fact]
        public void CanGetAllRoundsInTournamentByTournamentName()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
        }

        [Fact]
        public void CanGetAllRoundsInTournamentByTournamentId()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
        }

        [Fact]
        public void CanGetAllPlayersInTournamentByTournamentName()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Tournament tournament = services.WhenTournamentWithMatchesHasBeenCreated();

            List<Player> players = services.TournamentService.GetAllPlayersByName(tournament.Name);

            players.FirstOrDefault(player => player.Name.Contains("Maru")).Should().NotBeNull();
            players.FirstOrDefault(player => player.Name.Contains("Stork")).Should().NotBeNull();
            players.FirstOrDefault(player => player.Name.Contains("Taeja")).Should().NotBeNull();
            players.FirstOrDefault(player => player.Name.Contains("Rain")).Should().NotBeNull();
            players.FirstOrDefault(player => player.Name.Contains("Bomber")).Should().NotBeNull();
            players.FirstOrDefault(player => player.Name.Contains("FanTaSy")).Should().NotBeNull();
            players.FirstOrDefault(player => player.Name.Contains("Stephano")).Should().NotBeNull();
            players.FirstOrDefault(player => player.Name.Contains("Thorzain")).Should().NotBeNull();
        }

        [Fact]
        public void CanGetAllPlayersInTournamentByTournamentId()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Tournament tournament = services.WhenTournamentWithMatchesHasBeenCreated();

            List<Player> players = services.TournamentService.GetAllPlayersById(tournament.Id);

            players.FirstOrDefault(player => player.Name.Contains("Maru")).Should().NotBeNull();
            players.FirstOrDefault(player => player.Name.Contains("Stork")).Should().NotBeNull();
            players.FirstOrDefault(player => player.Name.Contains("Taeja")).Should().NotBeNull();
            players.FirstOrDefault(player => player.Name.Contains("Rain")).Should().NotBeNull();
            players.FirstOrDefault(player => player.Name.Contains("Bomber")).Should().NotBeNull();
            players.FirstOrDefault(player => player.Name.Contains("FanTaSy")).Should().NotBeNull();
            players.FirstOrDefault(player => player.Name.Contains("Stephano")).Should().NotBeNull();
            players.FirstOrDefault(player => player.Name.Contains("Thorzain")).Should().NotBeNull();
        }

        [Fact]
        public void CanGetStatusOfMatchInTournament()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Tournament tournament = services.WhenTournamentWithMatchesHasBeenCreated();
        }

        [Fact]
        public void CanGetWinningMatchPlayerOfMatchInTournament()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Tournament tournament = services.WhenTournamentWithMatchesHasBeenCreated();
        }

        [Fact]
        public void CanGetLosingMatchPlayerOfMatchInTournament()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Tournament tournament = services.WhenTournamentWithMatchesHasBeenCreated();
        }

        [Fact]
        public void CanGetAllBettersInTournamentByTournamentName()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Tournament tournament = services.WhenTournamentWithBettersHasBeenCreated();
        }

        [Fact]
        public void CanGetAllBettersInTournamentByTournamentId()
        {
            TournamentServiceContext services = TournamentServiceContext.GivenServices();
            Tournament tournament = services.WhenTournamentWithBettersHasBeenCreated();
        }
    }
}
