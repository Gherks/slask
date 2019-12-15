using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.TestCore;
using System;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class MatchTests
    {
        // public void CannotAddPointsToPlayersInMatchThatHasNotStarted
        // públic void MatchCanStartWithoutAnyBets

        [Fact]
        public void CanCreateMatch()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05AddedPlayersToRoundRobinGroup(services);

            group.Matches.Should().HaveCount(28);

            foreach (Match match in group.Matches)
            {
                match.Should().NotBeNull();
                match.Player1.Should().NotBeNull();
                match.Player2.Should().NotBeNull();
                match.StartDateTime.Should().NotBeBefore(DateTime.Now);
                match.GroupId.Should().Be(group.Id);
                match.Group.Should().Be(group);
            }
        }

        [Fact]
        public void MatchMustContainDifferentPlayers()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05AddedPlayersToRoundRobinGroup(services);

            foreach (Match match in group.Matches)
            {
                match.Player1.Should().NotBe(match.Player2);
            }
        }

        [Fact]
        public void CanAssignPlayerReferenceToPlayersInMatch()
        {
            TournamentServiceContext services = GivenServices();
            DualTournamentGroup group = BHAOpenSetup.Part04AddGroupsToDualTournamentRound(services).First();
            Match match = group.Matches.First();

            PlayerReference maruPlayerReference = PlayerReference.Create("Maru", group.Round.Tournament);
            PlayerReference storkPlayerReference = PlayerReference.Create("Stork", group.Round.Tournament);

            match.AssignPlayerReferences(maruPlayerReference, storkPlayerReference);

            match.Player1.PlayerReference.Should().Be(maruPlayerReference);
            match.Player2.PlayerReference.Should().Be(storkPlayerReference);
        }

        [Fact]
        public void CannotAssignSamePlayerReferenceToBothPlayersInMatch()
        {
            TournamentServiceContext services = GivenServices();
            DualTournamentGroup group = BHAOpenSetup.Part04AddGroupsToDualTournamentRound(services).First();
            Match match = group.Matches.First();

            PlayerReference playerReference = PlayerReference.Create("Maru", group.Round.Tournament);

            match.AssignPlayerReferences(playerReference, playerReference);

            match.Player1.PlayerReference.Should().BeNull();
            match.Player2.PlayerReference.Should().BeNull();
        }

        [Fact]
        public void CanAssignNullPlayerReferenceToEitherMatchPlayerInMatch()
        {
            TournamentServiceContext services = GivenServices();
            DualTournamentGroup group = BHAOpenSetup.Part04AddGroupsToDualTournamentRound(services).First();
            Match match = group.Matches.First();

            PlayerReference maruPlayerReference = PlayerReference.Create("Maru", group.Round.Tournament);
            PlayerReference storkPlayerReference = PlayerReference.Create("Stork", group.Round.Tournament);

            match.AssignPlayerReferences(maruPlayerReference, storkPlayerReference);
            match.AssignPlayerReferences(null, storkPlayerReference);

            match.Player1.PlayerReference.Should().Be(null);
            match.Player2.PlayerReference.Should().Be(storkPlayerReference);

            match.AssignPlayerReferences(maruPlayerReference, storkPlayerReference);
            match.AssignPlayerReferences(maruPlayerReference, null);

            match.Player1.PlayerReference.Should().Be(maruPlayerReference);
            match.Player2.PlayerReference.Should().Be(null);
        }

        [Fact]
        public void CanAssignNullPlayerReferenceToBothPlayersInMatch()
        {
            TournamentServiceContext services = GivenServices();
            DualTournamentGroup group = BHAOpenSetup.Part04AddGroupsToDualTournamentRound(services).First();
            Match match = group.Matches.First();

            PlayerReference maruPlayerReference = PlayerReference.Create("Maru", group.Round.Tournament);
            PlayerReference storkPlayerReference = PlayerReference.Create("Stork", group.Round.Tournament);

            match.AssignPlayerReferences(maruPlayerReference, storkPlayerReference);
            match.AssignPlayerReferences(null, null);

            match.Player1.PlayerReference.Should().Be(null);
            match.Player2.PlayerReference.Should().Be(null);
        }

        [Fact]
        public void MatchIsReadyWhenPlayerReferencesHasBeenAssignedToPlayers()
        {
            TournamentServiceContext services = GivenServices();
            DualTournamentGroup group = BHAOpenSetup.Part05AddedPlayersToDualTournamentGroups(services).First();

            group.Matches[0].IsReady().Should().BeTrue();
            group.Matches[0].Player1.PlayerReference.Should().NotBeNull();
            group.Matches[0].Player2.PlayerReference.Should().NotBeNull();

            group.Matches[1].IsReady().Should().BeTrue();
            group.Matches[1].Player1.PlayerReference.Should().NotBeNull();
            group.Matches[1].Player2.PlayerReference.Should().NotBeNull();

            group.Matches[2].IsReady().Should().BeFalse();
            group.Matches[2].Player1.PlayerReference.Should().BeNull();
            group.Matches[2].Player2.PlayerReference.Should().BeNull();

            group.Matches[3].IsReady().Should().BeFalse();
            group.Matches[3].Player1.PlayerReference.Should().BeNull();
            group.Matches[3].Player2.PlayerReference.Should().BeNull();

            group.Matches[4].IsReady().Should().BeFalse();
            group.Matches[4].Player1.PlayerReference.Should().BeNull();
            group.Matches[4].Player2.PlayerReference.Should().BeNull();
        }

        [Fact]
        public void MatchIsNotReadyWhenNoPlayerReferenceHasBeenAssignedToAnyPlayer()
        {
            TournamentServiceContext services = GivenServices();
            DualTournamentGroup group = BHAOpenSetup.Part04AddGroupsToDualTournamentRound(services).First();

            foreach (Domain.Match match in group.Matches)
            {
                match.IsReady().Should().BeFalse();
                match.Player1.PlayerReference.Should().BeNull();
                match.Player2.PlayerReference.Should().BeNull();
            }
        }

        [Fact]
        public void MatchIsNotReadyWhenOnlyOnePlayerHasBeenAssignedAPlayerReference()
        {
            TournamentServiceContext services = GivenServices();
            DualTournamentGroup group = BHAOpenSetup.Part04AddGroupsToDualTournamentRound(services).First();

            group.AddPlayerReference("Maru").Should().NotBeNull();

            group.Matches[0].IsReady().Should().BeFalse();
            group.Matches[0].Player1.PlayerReference.Should().NotBeNull();
            group.Matches[0].Player2.PlayerReference.Should().BeNull();
        }

        [Fact]
        public void CanFindPlayerInMatchByPlayerName()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05AddedPlayersToRoundRobinGroup(services);
            Match match = group.Matches.First();

            Player foundPlayer = match.FindPlayer(match.Player1.Name);

            foundPlayer.Should().NotBeNull();
            foundPlayer.Id.Should().Be(match.Player1.Id);
            foundPlayer.Name.Should().Be(match.Player1.Name);
        }

        [Fact]
        public void ReturnsNullWhenLookingForNonExistingPlayerInMatchByPlayerName()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05AddedPlayersToRoundRobinGroup(services);
            Match match = group.Matches.First();

            Player foundPlayer = match.FindPlayer("non-existing-player");

            foundPlayer.Should().BeNull();
        }

        [Fact]
        public void CanFindPlayerInMatchByPlayerId()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05AddedPlayersToRoundRobinGroup(services);
            Match match = group.Matches.First();

            Player foundPlayer = match.FindPlayer(match.Player1.Id);

            foundPlayer.Should().NotBeNull();
            foundPlayer.Id.Should().Be(match.Player1.Id);
            foundPlayer.Name.Should().Be(match.Player1.Name);
        }

        [Fact]
        public void ReturnsNullWhenLookingForNonExistingPlayerInMatchByPlayerId()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05AddedPlayersToRoundRobinGroup(services);
            Match match = group.Matches.First();

            Player foundPlayer = match.FindPlayer(Guid.NewGuid());

            foundPlayer.Should().BeNull();
        }

        [Fact]
        public void MatchStartDateTimeCannotBeChangedToSometimeInThePast()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05AddedPlayersToRoundRobinGroup(services);
            Match match = group.Matches.First();
            DateTime initialDateTime = match.StartDateTime;

            match.SetStartDateTime(DateTime.Now.AddSeconds(-1));

            match.StartDateTime.Should().Be(initialDateTime);
        }

        //[Fact]
        //public void CannotIncreaseScoreBeforeMatchHasStartedInGroup()
        //{
        //    TournamentServiceContext services = GivenServices();
        //    BracketGroup group = HomestoryCupSetup.Part14BetsPlacedOnMatchesInBracketGroup(services);
        //    Domain.Match match = group.Matches.First();

        //    match.Player1.IncreaseScore(1);
        //    match.Player2.IncreaseScore(1);

        //    match.Player1.Score.Should().Be(0);
        //    match.Player2.Score.Should().Be(0);
        //}

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
