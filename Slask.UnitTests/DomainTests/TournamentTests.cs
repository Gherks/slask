using FluentAssertions;
using Slask.Common;
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
            Tournament tournament = services.WhenCreatedTournament();

            tournament.Should().NotBeNull();
            tournament.Id.Should().NotBeEmpty();
            tournament.Name.Should().NotBeEmpty();
            tournament.Rounds.Should().BeEmpty();
            tournament.PlayerReferences.Should().BeEmpty();
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
            Tournament tournament = services.WhenCreatedTournament();

            tournament.RenameTo("BHA Open 2019");

            tournament.Name.Should().Be("BHA Open 2019");
        }

        [Fact]
        public void TournamentCannotBeRenamedToEmptyName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedTournament();

            tournament.RenameTo("");

            tournament.Name.Should().Be("WCS 2019");
        }

        [Fact]
        public void CannotCreateTournamentWithNameAlreadyInUseNoMatterLetterCasing()
        {
            TournamentServiceContext services = GivenServices();
            Tournament firstTournament = services.WhenCreatedTournament();
            Tournament secondTournament = services.TournamentService.CreateTournament(firstTournament.Name.ToUpper());

            secondTournament.Should().BeNull();
        }

        [Fact]
        public void TournamentCannotBeRenamedToNameAlreadyInUseNoMatterLetterCasing()
        {
            TournamentServiceContext services = GivenServices();
            Tournament firstTournament = services.WhenCreatedTournament();
            Tournament secondTournament = services.TournamentService.CreateTournament("BHA Open 2019");

            secondTournament.RenameTo(firstTournament.Name.ToUpper());

            secondTournament.Name.Should().Be("BHA Open 2019");
        }

        [Fact]
        public void CanGetPlayerInTournamentByPlayerNameEvenWhenNotMatchingLetterCasing()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            PlayerReference createdPlayerReference = tournament.PlayerReferences.First();

            PlayerReference fetchedPlayerReference = tournament.GetPlayerReferenceByPlayerName(createdPlayerReference.Name.ToLower());

            fetchedPlayerReference.Should().NotBeNull();
            fetchedPlayerReference.Id.Should().Be(createdPlayerReference.Id);
            fetchedPlayerReference.Name.Should().Be(createdPlayerReference.Name);
        }

        [Fact]
        public void CanGetRoundInTournamentByRoundId()
        {
            TournamentServiceContext services = GivenServices();
            Round createdRound = services.WhenCreatedRoundRobinRoundInTournament();
            Tournament tournament = createdRound.Tournament;

            Round fetchedRound = tournament.GetRoundByRoundId(createdRound.Id);

            fetchedRound.Should().NotBeNull();
            fetchedRound.Id.Should().Be(createdRound.Id);
            fetchedRound.Name.Should().Be(createdRound.Name);
        }

        [Fact]
        public void CanGetRoundInTournamentByRoundName()
        {
            TournamentServiceContext services = GivenServices();
            Round createdRound = services.WhenCreatedRoundRobinRoundInTournament();
            Tournament tournament = createdRound.Tournament;

            Round fetchedRound = tournament.GetRoundByRoundName(createdRound.Name);

            fetchedRound.Should().NotBeNull();
            fetchedRound.Id.Should().Be(createdRound.Id);
            fetchedRound.Name.Should().Be(createdRound.Name);
        }

        [Fact]
        public void CanGetPlayerReferenceInTournamentByPlayerId()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            PlayerReference createdPlayerReference = tournament.PlayerReferences.First();

            PlayerReference fetchedPlayerReference = tournament.GetPlayerReferenceByPlayerId(createdPlayerReference.Id);

            fetchedPlayerReference.Should().NotBeNull();
            fetchedPlayerReference.Id.Should().Be(createdPlayerReference.Id);
            fetchedPlayerReference.Name.Should().Be(createdPlayerReference.Name);
        }

        [Fact]
        public void CanGetPlayerReferenceInTournamentByPlayerName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            PlayerReference createdPlayerReference = tournament.PlayerReferences.First();

            PlayerReference fetchedPlayerReference = tournament.GetPlayerReferenceByPlayerName(createdPlayerReference.Name);

            fetchedPlayerReference.Should().NotBeNull();
            fetchedPlayerReference.Id.Should().Be(createdPlayerReference.Id);
            fetchedPlayerReference.Name.Should().Be(createdPlayerReference.Name);
        }

        [Fact]
        public void CanGetBetterInTournamentByBetterId()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedBetterInTournament();
            Better createdBetter = tournament.Betters.First();

            Better fetchedBetter = tournament.GetBetterById(createdBetter.Id);

            fetchedBetter.Should().NotBeNull();
            fetchedBetter.User.Should().NotBeNull();
            fetchedBetter.Id.Should().Be(createdBetter.Id);
            fetchedBetter.User.Name.Should().Be(createdBetter.User.Name);
        }

        [Fact]
        public void CanGetBetterInTournamentByBetterName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedBetterInTournament();
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
            Tournament tournament = services.WhenCreatedBetterInTournament();
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
            Tournament tournament = services.WhenCreatedBetterInTournament();
            Better createdBetter = tournament.Betters.First();

            Better duplicateBetter = tournament.AddBetter(createdBetter.User);

            duplicateBetter.Should().BeNull();
        }

        [Fact]
        public void TournamentCanAddRound()
        {
            TournamentServiceContext services = GivenServices();
            Round round = services.WhenCreatedRoundRobinRoundInTournament();

            round.Should().NotBeNull();
            round.Name.Should().Be("Group A");
            (round.BestOf % 2).Should().NotBe(0);
        }

        [Fact]
        public void TournamentDoesNotAcceptRoundRobinRoundsWithEvenBestOfs()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedTournament();
            Round round = tournament.AddRoundRobinRound("Group A", 4, 8);

            round.Should().BeNull();
        }

        [Fact]
        public void TournamentDoesNotAcceptDualTournamentRoundsWithEvenBestOfs()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedTournament();
            Round round = tournament.AddDualTournamentRound("Group A", 4);

            round.Should().BeNull();
        }

        [Fact]
        public void TournamentDoesNotAcceptBracketRoundsWithEvenBestOfs()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedTournament();
            Round round = tournament.AddBracketRound("Bracket A", 4);

            round.Should().BeNull();
        }

        [Fact]
        public void CanDetermineStatusOfMatchInTournamentAsNotBegun()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.GetRoundByRoundName("Round-Robin Group A").Groups.First().Matches.First();

            match.GetState().Should().Be(MatchState.HasNotBegun);
        }

        [Fact]
        public void CanDetermineStatusOfMatchInTournamentAsIsBeingPlayed()
        {
            TournamentServiceContext services = GivenServices();
            Group group = services.WhenCreatedGroupInRoundRobinRoundInTournament();
            Match match = group.AddMatch("Maru", "Stork", DateTimeHelper.Now);

            match.GetState().Should().Be(MatchState.IsBeingPlayed);
        }

        [Fact]
        public void CanDetermineStatusOfMatchInTournamentAsIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenCreatedMatchInRoundRobinRoundInTournamentAndMatchIsFinished();

            match.GetState().Should().Be(MatchState.IsFinished);
        }

        [Fact]
        public void CannotGetWinningPlayerOfMatchInTournamentBeforeItIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.GetRoundByRoundName("Round-Robin Group A").Groups.First().Matches.First();

            match.GetWinningPlayer().Should().BeNull();
        }

        [Fact]
        public void CannotGetLosingPlayerOfMatchInTournamentBeforeItIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.GetRoundByRoundName("Round-Robin Group A").Groups.First().Matches.First();

            match.GetLosingPlayer().Should().BeNull();
        }

        [Fact]
        public void CanGetWinningPlayerOfMatchInTournamentWhenMatchIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenCreatedMatchInRoundRobinRoundInTournamentAndMatchIsFinished();

            match.GetWinningPlayer().Should().Be(match.Player1);
        }

        [Fact]
        public void CanGetLosingPlayerOfMatchInTournamentWhenMatchIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            Match match = services.WhenCreatedMatchInRoundRobinRoundInTournamentAndMatchIsFinished();

            match.GetLosingPlayer().Should().Be(match.Player2);
        }

        [Fact]
        public void PlayerReferencesMustBeUniqueByNameWithinTournament()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.GetRoundByRoundName("Round-Robin Group A").Groups.First().Matches.First();

            GroupBase group = match.Group;
            group.AddMatch(match.Player1.Name, match.Player2.Name, DateTimeHelper.Now.AddSeconds(1));
            group.AddMatch(match.Player1.Name, match.Player2.Name, DateTimeHelper.Now.AddSeconds(1));

            List<PlayerReference> maruPlayers = tournament.PlayerReferences.Where(playerReference => playerReference.Name == match.Player1.Name).ToList();
            List<PlayerReference> storkPlayers = tournament.PlayerReferences.Where(playerReference => playerReference.Name == match.Player2.Name).ToList();

            maruPlayers.Should().NotBeNullOrEmpty();
            maruPlayers.Count.Should().Be(1);

            storkPlayers.Should().NotBeNullOrEmpty();
            storkPlayers.Count.Should().Be(1);
        }

        [Fact]
        public void AdvanceAmountInRoundMustBeDivisibleByTheNumberOfGroupsInRound()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CantAddMorePlayersToGroupThanAdvancementAmount()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void OnlyWinnersCanAdvanceToNextMatch()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GroupInDualTournamentRoundMustHaveExactlyFourPlayersBeforeFirstMatchInGroupIsCreated()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void DualTournamentRoundCanNeverContainMoreThanFiveMatches()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void OnlyTwoMatchesCanBe()
        {
            throw new NotImplementedException();
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
