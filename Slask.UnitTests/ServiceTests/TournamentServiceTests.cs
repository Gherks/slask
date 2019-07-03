using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.ServiceTests
{
    public class TournamentServiceTests
    {

        [Fact]
        public void CanGetTournamentById()
        {
            TournamentServiceContext services = GivenServices();
            Tournament createdTournament = services.WhenTournamentCreated();
            Tournament fetchedTournament = services.TournamentService.GetTournamentById(createdTournament.Id);

            fetchedTournament.Should().NotBeNull();
            fetchedTournament.Name.Should().Be("WCS 2019");
        }

        [Fact]
        public void CanGetTournamentByName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament createdTournament = services.WhenTournamentCreated();
            Tournament fetchedTournament = services.TournamentService.GetTournamentByName(createdTournament.Name);

            fetchedTournament.Should().NotBeNull();
            fetchedTournament.Name.Should().Be("WCS 2019");
        }

        [Fact]
        public void CanAddBetterToTournamentWithUserService()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenAddedBetterToTournament();

            tournament.Betters.First().Should().NotBeNull();
        }

        [Fact]
        public void CanOnlyAddUserAsBetterOncePerTournament()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenAddedBetterToTournament();
            Better better = tournament.AddBetter(services.UserService.GetUserByName(tournament.Betters.First().User.Name));

            better.Should().BeNull();
        }

        [Fact]
        public void PlayerNamesAreAddedToListWhenNewPlayersAreAddedTournament()
        {
            throw new NotImplementedException();

            //TournamentServiceContext services = GivenServices();
            //Tournament tournament = services.WhenAddedMatchesToTournament();

            //tournament.Players.Should().NotBeNull();
            //tournament.Players.Count.Should().Be(8);

            //tournament.Players.FirstOrDefault(player => player.Name.Contains("Maru")).Should().NotBeNull();
            //tournament.Players.FirstOrDefault(player => player.Name.Contains("Stork")).Should().NotBeNull();
            //tournament.Players.FirstOrDefault(player => player.Name.Contains("Taeja")).Should().NotBeNull();
            //tournament.Players.FirstOrDefault(player => player.Name.Contains("Rain")).Should().NotBeNull();
            //tournament.Players.FirstOrDefault(player => player.Name.Contains("Bomber")).Should().NotBeNull();
            //tournament.Players.FirstOrDefault(player => player.Name.Contains("FanTaSy")).Should().NotBeNull();
            //tournament.Players.FirstOrDefault(player => player.Name.Contains("Stephano")).Should().NotBeNull();
            //tournament.Players.FirstOrDefault(player => player.Name.Contains("Thorzain")).Should().NotBeNull();
        }

        [Fact]
        public void CanGetAllPlayerNamesInTournamentByTournamentName()
        {
            throw new NotImplementedException();
            //TournamentServiceContext services = GivenServices();
            //Tournament tournament = services.WhenAddedMatchesToTournament();

            //List<Player> players = services.TournamentService.GetAllPlayersByName(tournament.Name);

            //players.Should().NotBeNullOrEmpty();
            //players.Count.Should().Be(8);

            //players.FirstOrDefault(player => player.Name.Contains("Maru")).Should().NotBeNull();
            //players.FirstOrDefault(player => player.Name.Contains("Stork")).Should().NotBeNull();
            //players.FirstOrDefault(player => player.Name.Contains("Taeja")).Should().NotBeNull();
            //players.FirstOrDefault(player => player.Name.Contains("Rain")).Should().NotBeNull();
            //players.FirstOrDefault(player => player.Name.Contains("Bomber")).Should().NotBeNull();
            //players.FirstOrDefault(player => player.Name.Contains("FanTaSy")).Should().NotBeNull();
            //players.FirstOrDefault(player => player.Name.Contains("Stephano")).Should().NotBeNull();
            //players.FirstOrDefault(player => player.Name.Contains("Thorzain")).Should().NotBeNull();
        }

        [Fact]
        public void CanGetAllPlayerNamesInTournamentByTournamentId()
        {
            throw new NotImplementedException();
            //TournamentServiceContext services = GivenServices();
            //Tournament tournament = services.WhenAddedMatchesToTournament();

            //List<Player> players = services.TournamentService.GetAllPlayersById(tournament.Id);

            //players.Should().NotBeNullOrEmpty();
            //players.Count.Should().Be(8);

            //players.FirstOrDefault(player => player.Name.Contains("Maru")).Should().NotBeNull();
            //players.FirstOrDefault(player => player.Name.Contains("Stork")).Should().NotBeNull();
            //players.FirstOrDefault(player => player.Name.Contains("Taeja")).Should().NotBeNull();
            //players.FirstOrDefault(player => player.Name.Contains("Rain")).Should().NotBeNull();
            //players.FirstOrDefault(player => player.Name.Contains("Bomber")).Should().NotBeNull();
            //players.FirstOrDefault(player => player.Name.Contains("FanTaSy")).Should().NotBeNull();
            //players.FirstOrDefault(player => player.Name.Contains("Stephano")).Should().NotBeNull();
            //players.FirstOrDefault(player => player.Name.Contains("Thorzain")).Should().NotBeNull();
        }

        [Fact] 
        public void CanGetAllPlayerInstancesInTournamentByPlayerNameId()
        {
            throw new NotImplementedException();
            //TournamentServiceContext services = GivenServices();
            //Tournament tournament = services.WhenAddedMatchesToTournament();
            //Player createdPlayer = tournament.Players.First();

            //Player fetchedPlayer = services.TournamentService.GetAllPlayersByPlayerNameId(createdPlayer.Id);

            //fetchedPlayer.Should().NotBeNull();
            //fetchedPlayer.Id.Should().Be(createdPlayer.Id);
            //fetchedPlayer.Name.Should().Be(createdPlayer.Name);
        }

        [Fact]
        public void CanGetAllPlayerInstancesInTournamentByPlayerName()
        {
            throw new NotImplementedException();
            //TournamentServiceContext services = GivenServices();
            //Tournament tournament = services.WhenAddedMatchesToTournament();
            //Player createdPlayer = tournament.Players.First();

            //Player fetchedPlayer = services.TournamentService.GetAllPlayersByName(createdPlayer.Name);

            //fetchedPlayer.Should().NotBeNull();
            //fetchedPlayer.Id.Should().Be(createdPlayer.Id);
            //fetchedPlayer.Name.Should().Be(createdPlayer.Name);
        }

        [Fact]
        public void CanGetAllBettersInTournamentByTournamentName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenAddedBetterToTournament();

            List<Better> betters = services.TournamentService.GetBettersByTournamentName(tournament.Name);

            betters.Should().NotBeNullOrEmpty();
            betters.Count.Should().Be(1);
        }

        [Fact]
        public void CanGetAllBettersInTournamentByTournamentId()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenAddedBetterToTournament();

            List<Better> betters = services.TournamentService.GetBettersByTournamentId(tournament.Id);

            betters.Should().NotBeNullOrEmpty();
            betters.Count.Should().Be(1);
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
