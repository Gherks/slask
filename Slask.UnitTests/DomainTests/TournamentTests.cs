using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class TournamentTests
    {
        [Fact]
        public void EnsureTournamentIsValidWhenCreated()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenTournamentCreated();

            tournament.Should().NotBeNull();
            tournament.Id.Should().NotBeEmpty();
            tournament.Name.Should().NotBeEmpty();
            tournament.Rounds.Should().BeEmpty();
            tournament.PlayerNameReferences.Should().BeEmpty();
            tournament.Betters.Should().BeEmpty();
            tournament.Settings.Should().BeEmpty();
            tournament.MiscBetCatalogue.Should().BeEmpty();
        }

        [Fact]
        public void TournamentCreationFailsWithoutName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.TournamentService.CreateTournament("");

            tournament.Should().BeNull();
        }

        [Fact]
        public void TournamentCanBeRenamed()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenTournamentCreated();

            tournament.RenameTo("BHA Open 2019");

            tournament.Name.Should().Be("BHA Open 2019");
        }

        [Fact]
        public void TournamentCannotBeRenamedToEmptyName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenTournamentCreated();

            tournament.RenameTo("");

            tournament.Name.Should().Be("WCS 2019");
        }

        [Fact]
        public void CannotCreateTournamentWithNameAlreadyInUseNoMatterLetterCasing()
        {
            TournamentServiceContext services = GivenServices();
            Tournament firstTournament = services.WhenTournamentCreated();
            Tournament secondTournament = services.TournamentService.CreateTournament(firstTournament.Name.ToUpper());

            secondTournament.Should().BeNull();
        }

        [Fact]
        public void TournamentCannotBeRenamedToNameAlreadyInUseNoMatterLetterCasing()
        {
            TournamentServiceContext services = GivenServices();
            Tournament firstTournament = services.WhenTournamentCreated();
            Tournament secondTournament = services.TournamentService.CreateTournament("BHA Open 2019");

            secondTournament.RenameTo(firstTournament.Name.ToUpper());

            secondTournament.Name.Should().Be("BHA Open 2019");
        }

        [Fact]
        public void CanGetPlayerInTournamentByPlayerNameEvenWhenNotMatchingLetterCasing()
        {
            throw new NotImplementedException();
            //TournamentServiceContext services = GivenServices();
            //Tournament tournament = services.WhenAddedMatchesToTournament();
            //Player createdPlayer = tournament.Players.First();

            //Player fetchedPlayer = tournament.GetPlayerByName(createdPlayer.Name.ToLower());

            //fetchedPlayer.Should().NotBeNull();
            //fetchedPlayer.Id.Should().Be(createdPlayer.Id);
            //fetchedPlayer.Name.Should().Be(createdPlayer.Name);
        }

        [Fact]
        public void CanGetPlayerInTournamentByPlayerId()
        {
            throw new NotImplementedException();
            //TournamentServiceContext services = GivenServices();
            //Tournament tournament = services.WhenAddedMatchesToTournament();
            //Player createdPlayer = tournament.Players.First();

            //Player fetchedPlayer = tournament.GetPlayerById(createdPlayer.Id);

            //fetchedPlayer.Should().NotBeNull();
            //fetchedPlayer.Id.Should().Be(createdPlayer.Id);
            //fetchedPlayer.Name.Should().Be(createdPlayer.Name);
        }

        [Fact]
        public void CanGetPlayerInTournamentByPlayerName()
        {
            throw new NotImplementedException();
            //TournamentServiceContext services = GivenServices();
            //Tournament tournament = services.WhenAddedMatchesToTournament();
            //Player createdPlayer = tournament.Players.First();

            //Player fetchedPlayer = tournament.GetPlayerByName(createdPlayer.Name);

            //fetchedPlayer.Should().NotBeNull();
            //fetchedPlayer.Id.Should().Be(createdPlayer.Id);
            //fetchedPlayer.Name.Should().Be(createdPlayer.Name);
        }

        [Fact]
        public void CanGetBetterInTournamentByPlayerId()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenAddedBetterToTournament();
            Better createdBetter = tournament.Betters.First();

            Better fetchedBetter = tournament.GetBetterById(createdBetter.Id);

            fetchedBetter.Should().NotBeNull();
            fetchedBetter.User.Should().NotBeNull();
            fetchedBetter.Id.Should().Be(createdBetter.Id);
            fetchedBetter.User.Name.Should().Be(createdBetter.User.Name);
        }

        [Fact]
        public void CanGetBetterInTournamentByPlayerName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenAddedBetterToTournament();
            Better createdBetter = tournament.Betters.First();

            Better fetchedBetter = tournament.GetBetterByName(createdBetter.User.Name);

            fetchedBetter.Should().NotBeNull();
            fetchedBetter.User.Should().NotBeNull();
            fetchedBetter.Id.Should().Be(createdBetter.Id);
            fetchedBetter.User.Name.Should().Be(createdBetter.User.Name);
        }

        [Fact]
        public void CanGetBetterInTournamentByPlayerNameEvenWhenNotMatchingLetterCasing()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenAddedBetterToTournament();
            Better createdBetter = tournament.Betters.First();

            Better fetchedBetter = tournament.GetBetterByName(createdBetter.User.Name.ToLower());

            fetchedBetter.Should().NotBeNull();
            fetchedBetter.User.Should().NotBeNull();
            fetchedBetter.Id.Should().Be(createdBetter.Id);
            fetchedBetter.User.Name.Should().Be(createdBetter.User.Name);
        }

        [Fact]
        public void CanOnlyAddUserAsBetterOncePerTournament()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenAddedBetterToTournament();
            Better createdBetter = tournament.Betters.First();

            Better duplicateBetter = tournament.AddBetter(createdBetter.User);

            duplicateBetter.Should().BeNull();
        }

        [Fact]
        public void TournamentCanAddRound()
        {
            TournamentServiceContext services = GivenServices();
            Round round = services.WhenAddedRoundToTournament();

            round.Should().NotBeNull();
            round.Name.Should().Be("Group A");
            (round.BestOf % 2).Should().NotBe(0);
        }

        [Fact]
        public void TournamentDoesNotAcceptRoundsWithEvenBestOfs()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenTournamentCreated();
            Round round = tournament.AddRound("Group A", 1, 4, 8);

            round.Should().BeNull();
        }

        [Fact]
        public void TournamentMatchMustContainTwoPlayers()
        {
            TournamentServiceContext services = GivenServices();
            Group group = services.WhenAddedGroupToTournament();
            Match matchMissingFirstPlayer = group.AddMatch("Maru", "", DateTime.Now.AddSeconds(1));
            Match matchMissingSecondPlayer = group.AddMatch("", "Stork", DateTime.Now.AddSeconds(1));
            Match matchMissingBothPlayers = group.AddMatch("", "", DateTime.Now.AddSeconds(1));

            matchMissingFirstPlayer.Should().BeNull();
            matchMissingSecondPlayer.Should().BeNull();
            matchMissingBothPlayers.Should().BeNull();
        }

        [Fact]
        public void TournamentMatchMustContainDifferentPlayers()
        {
            TournamentServiceContext services = GivenServices();
            Group group = services.WhenAddedGroupToTournament();
            Match match = group.AddMatch("Maru", "Maru", DateTime.Now.AddSeconds(1));

            match.Should().BeNull();
        }

        [Fact]
        public void CanDetermineStatusOfMatchInTournamentAsNotBegun()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.GetState().Should().Be(Match.State.HasNotBegun);
        }

        [Fact]
        public void CanDetermineStatusOfMatchInTournamentAsIsBeingPlayed()
        {
            TournamentServiceContext services = GivenServices();
            Group group = services.WhenAddedGroupToTournament();
            Match match = group.AddMatch("Maru", "Stork", DateTime.Now);

            match.GetState().Should().Be(Match.State.IsBeingPlayed);
        }

        [Fact]
        public void CanDetermineStatusOfMatchInTournamentAsIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournamentAndMatchIsFinished();

            match.GetState().Should().Be(Match.State.IsFinished);
        }

        [Fact]
        public void CannotGetWinningPlayerOfMatchInTournamentBeforeItIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.GetWinningPlayer().Should().BeNull();
        }

        [Fact]
        public void CannotGetLosingPlayerOfMatchInTournamentBeforeItIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournament();

            match.GetLosingPlayer().Should().BeNull();
        }

        [Fact]
        public void CanGetWinningPlayerOfMatchInTournamentWhenMatchIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournamentAndMatchIsFinished();

            match.GetWinningPlayer().Should().Be(match.Player1);
        }

        [Fact]
        public void CanGetLosingPlayerOfMatchInTournamentWhenMatchIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenAddedMatchToTournamentAndMatchIsFinished();

            match.GetLosingPlayer().Should().Be(match.Player2);
        }

        [Fact]
        public void PlayerNameReferencesMustBeUniqueWithinTournament()
        {
            throw new NotImplementedException();
            //TournamentServiceContext services = GivenServices();
            //Match match = services.WhenAddedMatchToTournament();

            //Group group = match.Group;
            //group.AddMatch(match.Player1.Name, match.Player2.Name, DateTime.Now.AddSeconds(1));
            //group.AddMatch(match.Player1.Name, match.Player2.Name, DateTime.Now.AddSeconds(1));

            //Tournament tournament = group.Round.Tournament;

            //List<Player> maruPlayers = tournament.Players.Where(player => player.Name.Contains(match.Player1.Name)).ToList();
            //List<Player> storkPlayers = tournament.Players.Where(player => player.Name.Contains(match.Player2.Name)).ToList();

            //maruPlayers.Should().NotBeNullOrEmpty();
            //maruPlayers.Count.Should().Be(1);

            //storkPlayers.Should().NotBeNullOrEmpty();
            //storkPlayers.Count.Should().Be(1);
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
