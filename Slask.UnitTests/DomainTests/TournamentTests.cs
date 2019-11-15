using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    // IF PLAYER REF WAS REMOVED FROM GROUP ENTIERLY, AND GROUP IS IN FIRST ROUND, CHECK WHETHER PLAYER REF SHOULD BE REMOVED FROM TOURNAMENT REF LIST AS WELL

    public class TournamentTests
    {
        [Fact]
        public void EnsureTournamentIsValidWhenCreated()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part01CreateTournament(services);

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
        public void CanGetRoundInTournamentByRoundId()
        {
            TournamentServiceContext services = GivenServices();
            RoundBase createdRound = HomestoryCupSetup.Part03AddRoundRobinRound(services);
            Tournament tournament = createdRound.Tournament;

            RoundBase fetchedRound = tournament.GetRoundByRoundId(createdRound.Id);

            fetchedRound.Should().NotBeNull();
            fetchedRound.Id.Should().Be(createdRound.Id);
            fetchedRound.Name.Should().Be(createdRound.Name);
        }

        [Fact]
        public void CanGetRoundInTournamentByRoundName()
        {
            TournamentServiceContext services = GivenServices();
            RoundBase createdRound = HomestoryCupSetup.Part03AddRoundRobinRound(services);
            Tournament tournament = createdRound.Tournament;

            RoundBase fetchedRound = tournament.GetRoundByRoundName(createdRound.Name);

            fetchedRound.Should().NotBeNull();
            fetchedRound.Id.Should().Be(createdRound.Id);
            fetchedRound.Name.Should().Be(createdRound.Name);
        }

        [Fact]
        public void CanGetPlayerReferenceInTournamentByPlayerId()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05AddedPlayersToRoundRobinGroup(services);
            Tournament tournament = group.Round.Tournament;

            PlayerReference createdPlayerReference = tournament.PlayerReferences.First();
            PlayerReference fetchedPlayerReference = tournament.GetPlayerReferenceByPlayerId(createdPlayerReference.Id);

            fetchedPlayerReference.Should().NotBeNull();
            fetchedPlayerReference.Id.Should().Be(createdPlayerReference.Id);
            fetchedPlayerReference.Name.Should().Be(createdPlayerReference.Name);
        }

        [Fact]
        public void CanGetPlayerInTournamentByPlayerNameNoMatterLetterCasing()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05AddedPlayersToRoundRobinGroup(services);
            Tournament tournament = group.Round.Tournament;

            PlayerReference createdPlayerReference = tournament.PlayerReferences.First();
            PlayerReference fetchedPlayerReference = tournament.GetPlayerReferenceByPlayerName(createdPlayerReference.Name.ToLower());

            fetchedPlayerReference.Should().NotBeNull();
            fetchedPlayerReference.Id.Should().Be(createdPlayerReference.Id);
            fetchedPlayerReference.Name.Should().Be(createdPlayerReference.Name);
        }

        [Fact]
        public void CanGetBetterInTournamentByBetterId()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part02BettersAddedToTournament(services);
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
            Tournament tournament = HomestoryCupSetup.Part02BettersAddedToTournament(services);
            Better createdBetter = tournament.Betters.First();

            Better fetchedBetter = tournament.GetBetterByName(createdBetter.User.Name);

            fetchedBetter.Should().NotBeNull();
            fetchedBetter.User.Should().NotBeNull();
            fetchedBetter.Id.Should().Be(createdBetter.Id);
            fetchedBetter.User.Name.Should().Be(createdBetter.User.Name);
        }

        [Fact]
        public void CanGetBetterInTournamentByBetterNameNoMatterLetterCasing()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part02BettersAddedToTournament(services);
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
            Tournament tournament = HomestoryCupSetup.Part02BettersAddedToTournament(services);
            Better createdBetter = tournament.Betters.First();

            Better duplicateBetter = tournament.AddBetter(createdBetter.User);

            duplicateBetter.Should().BeNull();
        }

        [Fact]
        public void TournamentDoesNotAcceptRoundRobinRoundsWithEvenBestOfs()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part01CreateTournament(services);

            for (int bestOf = 0; bestOf <= 64; bestOf += 2)
            {
                RoundBase round = tournament.AddRoundRobinRound("Group A", bestOf, 8);
                round.Should().BeNull();
            }
        }

        [Fact]
        public void TournamentDoesNotAcceptDualTournamentRoundsWithEvenBestOfs()
        {
            //TournamentServiceContext services = GivenServices();
            //Tournament tournament = services.WhenCreatedTournament();

            //for (int bestOf = 0; bestOf <= 64; bestOf += 2)
            //{
            //    Round round = tournament.AddDualTournamentRound("Group A", bestOf);
            //    round.Should().BeNull();
            //}
        }

        [Fact]
        public void TournamentDoesNotAcceptBracketRoundsWithEvenBestOfs()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = HomestoryCupSetup.Part01CreateTournament(services);

            for (int bestOf = 0; bestOf <= 64; bestOf += 2)
            {
                RoundBase round = tournament.AddBracketRound("Bracket A", bestOf);
                round.Should().BeNull();
            }
        }

        [Fact]
        public void CanDetermineStatusOfMatchInTournamentAsNotBegun()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part06StartDateTimeSetToMatchesInRoundRobinGroup(services);
            Match match = group.Matches.First();

            match.GetPlayState().Should().Be(PlayState.NotBegun);
        }

        [Fact]
        public void CanDetermineStatusOfMatchInTournamentAsIsBeingPlayed()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part07BetsPlacedOnMatchesInRoundRobinGroup(services);
            Match match = group.Matches.First();
            //services.SetMockedTime();// DateTimeMockHelper.SetTime(match.StartDateTime);

            match.GetPlayState().Should().Be(PlayState.IsPlaying);
        }

        [Fact]
        public void CanDetermineStatusOfMatchInTournamentAsIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part08CompleteFirstMatchInRoundRobinGroup(services);
            Match match = group.Matches.First();
            //services.SetMockedTime();// DateTimeMockHelper.SetTime(match.StartDateTime);

            match.GetPlayState().Should().Be(PlayState.IsFinished);
        }

        [Fact]
        public void CannotGetWinningPlayerOfMatchInTournamentBeforeItIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part07BetsPlacedOnMatchesInRoundRobinGroup(services);
            Match match = group.Matches.First();

            match.GetWinningPlayer().Should().BeNull();
        }

        [Fact]
        public void CannotGetLosingPlayerOfMatchInTournamentBeforeItIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part07BetsPlacedOnMatchesInRoundRobinGroup(services);
            Match match = group.Matches.First();

            match.GetLosingPlayer().Should().BeNull();
        }

        [Fact]
        public void CanGetWinningPlayerOfMatchInTournamentWhenMatchIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part08CompleteFirstMatchInRoundRobinGroup(services);
            Match match = group.Matches.First();

            // Must verify
            match.GetWinningPlayer().Should().Be(match.Player1);
        }

        [Fact]
        public void CanGetLosingPlayerOfMatchInTournamentWhenMatchIsFinished()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part08CompleteFirstMatchInRoundRobinGroup(services);
            Match match = group.Matches.First();

            // Must verify
            match.GetLosingPlayer().Should().Be(match.Player2);
        }

        [Fact]
        public void PlayerReferencesMustBeUniqueByNameWithinTournament()
        {
            TournamentServiceContext services = GivenServices();
            BracketGroup group = HomestoryCupSetup.Part16CompleteAllMatchesInBracketGroup(services);
            Tournament tournament = group.Round.Tournament;

            foreach (PlayerReference currentPlayerReference in tournament.PlayerReferences)
            {
                List<PlayerReference> playerReferences = tournament.PlayerReferences.Where(
                    playerReference => playerReference.Name == currentPlayerReference.Name).ToList();

                playerReferences.Should().NotBeNullOrEmpty();
                playerReferences.Should().HaveCount(1);
            }
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
