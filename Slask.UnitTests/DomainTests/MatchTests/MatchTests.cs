using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using System;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.MatchTests
{
    public class MatchTests
    {
        private const string firstPlayerName = "Maru";
        private const string secondPlayerName = "Stork";

        private readonly Tournament tournament;
        private readonly BracketRound bracketRound;
        private readonly BracketGroup bracketGroup;
        private readonly Match match;

        public MatchTests()
        {
            tournament = Tournament.Create("GSL 2019");
            bracketRound = tournament.AddBracketRound() as BracketRound;
            bracketRound.RegisterPlayerReference(firstPlayerName);
            bracketRound.RegisterPlayerReference(secondPlayerName);
            bracketGroup = bracketRound.Groups.First() as BracketGroup;
            match = bracketGroup.Matches.First();
        }

        [Fact]
        public void CanCreateMatch()
        {
            match.Should().NotBeNull();
            match.Player1.Should().NotBeNull();
            match.Player1.Name.Should().Be(firstPlayerName);
            match.Player2.Should().NotBeNull();
            match.Player2.Name.Should().Be(secondPlayerName);
            match.StartDateTime.Should().NotBeBefore(SystemTime.Now);
            match.GroupId.Should().Be(bracketGroup.Id);
            match.Group.Should().Be(bracketGroup);
        }

        [Fact]
        public void MatchMustContainDifferentPlayers()
        {
            Tournament tournament = Tournament.Create("GSL 2019");
            BracketRound bracketRound = tournament.AddBracketRound() as BracketRound;
            PlayerReference playerReference = bracketRound.RegisterPlayerReference(firstPlayerName);

            Match match = bracketRound.Groups.First().Matches.First();

            match.SetPlayers(playerReference, playerReference);

            match.Player1.PlayerReference.Should().Be(playerReference);
            match.Player2.Should().BeNull();
        }

        [Fact]
        public void CanAssignNewPlayerReferencesToMatch()
        {
            PlayerReference taejaPlayerReference = PlayerReference.Create("Taeja", tournament);
            PlayerReference rainPlayerReference = PlayerReference.Create("Rain", tournament);

            match.SetPlayers(taejaPlayerReference, rainPlayerReference);

            match.Player1.PlayerReference.Should().Be(taejaPlayerReference);
            match.Player2.PlayerReference.Should().Be(rainPlayerReference);
        }

        [Fact]
        public void CannotAssignSamePlayerReferenceAsBothPlayersInMatch()
        {
            PlayerReference firstPlayerReference = match.Player1.PlayerReference;
            PlayerReference secondPlayerReference = match.Player2.PlayerReference;

            PlayerReference playerReference = PlayerReference.Create("Taeja", tournament);

            match.SetPlayers(playerReference, playerReference);

            match.Player1.PlayerReference.Should().Be(firstPlayerReference);
            match.Player2.PlayerReference.Should().Be(secondPlayerReference);
        }

        [Fact]
        public void CanAssignNullPlayerReferenceToEitherPlayerInMatch()
        {
            PlayerReference maruPlayerReference = PlayerReference.Create("Maru", tournament);
            PlayerReference storkPlayerReference = PlayerReference.Create("Stork", tournament);

            match.SetPlayers(maruPlayerReference, storkPlayerReference);
            match.SetPlayers(null, storkPlayerReference);

            match.Player1.Should().BeNull();
            match.Player2.PlayerReference.Should().Be(storkPlayerReference);

            match.SetPlayers(maruPlayerReference, storkPlayerReference);
            match.SetPlayers(maruPlayerReference, null);

            match.Player1.PlayerReference.Should().Be(maruPlayerReference);
            match.Player2.Should().BeNull();
        }

        [Fact]
        public void CanAssignNullPlayerReferenceToBothPlayersInMatch()
        {
            match.SetPlayers(null, null);

            match.Player1.Should().Be(null);
            match.Player2.Should().Be(null);
        }

        [Fact]
        public void MatchIsReadyWhenPlayerReferencesHasBeenAssignedToPlayers()
        {
            match.IsReady().Should().BeTrue();
            match.Player1.PlayerReference.Should().NotBeNull();
            match.Player2.PlayerReference.Should().NotBeNull();
        }

        [Fact]
        public void MatchIsNotReadyWhenNoPlayerReferenceHasBeenAssignedToMatch()
        {
            match.SetPlayers(null, null);

            match.IsReady().Should().BeFalse();
            match.Player1.Should().BeNull();
            match.Player2.Should().BeNull();
        }

        [Fact]
        public void MatchIsNotReadyWhenEitherPlayerReferenceHasBeenAssignedNull()
        {
            PlayerReference firstPlayerReference = match.Player1.PlayerReference;
            PlayerReference secondPlayerReference = match.Player2.PlayerReference;

            match.SetPlayers(firstPlayerReference, null);

            match.IsReady().Should().BeFalse();
            match.Player1.PlayerReference.Should().Be(firstPlayerReference);
            match.Player2.Should().BeNull();

            match.SetPlayers(null, secondPlayerReference);

            match.IsReady().Should().BeFalse();
            match.Player1.Should().BeNull();
            match.Player2.PlayerReference.Should().Be(secondPlayerReference);
        }

        [Fact]
        public void CanFindPlayerInMatchByPlayerName()
        {
            Player firstFoundPlayer = match.FindPlayer(firstPlayerName);
            Player secondFoundPlayer = match.FindPlayer(secondPlayerName);

            firstFoundPlayer.Should().NotBeNull();
            firstFoundPlayer.Id.Should().Be(match.Player1.Id);
            firstFoundPlayer.Name.Should().Be(firstPlayerName);

            secondFoundPlayer.Should().NotBeNull();
            secondFoundPlayer.Id.Should().Be(match.Player2.Id);
            secondFoundPlayer.Name.Should().Be(secondPlayerName);
        }

        [Fact]
        public void CanFindPlayerInMatchByPlayerId()
        {
            Player firstFoundPlayer = match.FindPlayer(match.Player1.Id);
            Player secondFoundPlayer = match.FindPlayer(match.Player2.Id);

            firstFoundPlayer.Should().NotBeNull();
            firstFoundPlayer.Id.Should().Be(match.Player1.Id);
            firstFoundPlayer.Name.Should().Be(match.Player1.Name);

            secondFoundPlayer.Should().NotBeNull();
            secondFoundPlayer.Id.Should().Be(match.Player2.Id);
            secondFoundPlayer.Name.Should().Be(match.Player2.Name);
        }

        [Fact]
        public void ReturnsNullWhenLookingForNonExistingPlayerInMatchByPlayerName()
        {
            Player foundPlayer = match.FindPlayer("non-existing-player");

            foundPlayer.Should().BeNull();
        }

        [Fact]
        public void ReturnsNullWhenLookingForNonExistingPlayerInMatchByPlayerId()
        {
            Player foundPlayer = match.FindPlayer(Guid.NewGuid());

            foundPlayer.Should().BeNull();
        }

        [Fact]
        public void MatchStartDateTimeCanBeSetToFutureDateTime()
        {
            DateTime futureStartDateTime = SystemTime.Now.AddHours(1);

            match.SetStartDateTime(futureStartDateTime);

            match.StartDateTime.Should().Be(futureStartDateTime);
        }

        [Fact]
        public void MatchStartDateTimeCannotBeChangedToSometimeInThePast()
        {
            DateTime initialDateTime = match.StartDateTime;

            match.SetStartDateTime(DateTime.Now.AddSeconds(-1));

            match.StartDateTime.Should().Be(initialDateTime);
        }

        [Fact]
        public void PlayStateIsEqualToNotBegunBeforeMactchHasStarted()
        {
            PlayState playState = match.GetPlayState();

            playState.Should().Be(PlayState.NotBegun);
        }

        [Fact]
        public void PlayStateIsEqualToOngoingWhenMatchHasStartedButNotFinished()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.Player1.IncreaseScore(GetWinningScore() - 1);

            PlayState playState = match.GetPlayState();

            playState.Should().Be(PlayState.Ongoing);
        }

        [Fact]
        public void PlayStateIsEqualToFinishedWhenMatchHasAWinner()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.Player1.IncreaseScore(GetWinningScore());

            PlayState playState = match.GetPlayState();

            playState.Should().Be(PlayState.Finished);
        }

        [Fact]
        public void CanGetWinningPlayerWhenMatchIsFinished()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.Player1.IncreaseScore(GetWinningScore());

            Player player = match.GetWinningPlayer();

            player.Should().Be(match.Player1);
        }

        [Fact]
        public void CanGetLosingPlayerWhenMatchIsFinished()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.Player1.IncreaseScore(GetWinningScore());

            Player player = match.GetLosingPlayer();

            player.Should().Be(match.Player2);
        }

        [Fact]
        public void CannotGetWinningPlayerBeforeMatchHasStarted()
        {
            Player player = match.GetWinningPlayer();

            player.Should().BeNull();
        }

        [Fact]
        public void CannotGetLosingPlayerBeforeMatchHasStarted()
        {
            Player player = match.GetLosingPlayer();

            player.Should().BeNull();
        }

        [Fact]
        public void CannotGetWinningPlayerWhileMatchIsOngoing()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.Player1.IncreaseScore(GetWinningScore() - 1);

            Player player = match.GetWinningPlayer();

            player.Should().BeNull();
        }

        [Fact]
        public void CannotGetLosingPlayerWhileMatchIsOngoing()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.Player1.IncreaseScore(GetWinningScore() - 1);

            Player player = match.GetLosingPlayer();

            player.Should().BeNull();
        }

        [Fact]
        public void CannotIncreaseScoreBeforeMatchHasStarted()
        {
            match.Player1.IncreaseScore(1);
            match.Player2.IncreaseScore(1);

            match.Player1.Score.Should().Be(0);
            match.Player2.Score.Should().Be(0);
        }

        [Fact]
        public void CannotIncreaseScoreWhenMatchIsFinished()
        {
            match.Player1.IncreaseScore(1);
            match.Player2.IncreaseScore(1);

            match.Player1.Score.Should().Be(0);
            match.Player2.Score.Should().Be(0);
        }

        [Fact]
        public void MatchRemainsUnchangedWhenAddingPlayerReferenceToMatchWithTwoPlayersAlready()
        {
            PlayerReference playerReference = PlayerReference.Create("Taeja", tournament);

            match.AddPlayer(playerReference);

            match.Player1.Should().NotBeNull();
            match.Player1.Name.Should().Be(firstPlayerName);
            match.Player2.Should().NotBeNull();
            match.Player2.Name.Should().Be(secondPlayerName);
        }

        private int GetWinningScore()
        {
            return (int)Math.Ceiling(bracketRound.BestOf / 2.0);
        }
    }
}
