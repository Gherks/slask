using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.TestCore;
using System;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class MatchTests
    {
        private readonly Tournament tournament;
        private readonly BracketRound bracketRound;
        private readonly BracketGroup bracketGroup;

        public MatchTests()
        {
            tournament = Tournament.Create("GSL 2019");
            bracketRound = tournament.AddBracketRound("Bracket round", 3) as BracketRound;
            bracketGroup = bracketRound.AddGroup() as BracketGroup;
        }

        [Fact]
        public void CanCreateMatch()
        {
            string firstPlayerName = "Maru";
            string secondPlayerName = "Stork";

            Match match = InitializeFirstMatch(firstPlayerName, secondPlayerName);

            match.Should().NotBeNull();
            match.Player1.Should().NotBeNull();
            match.Player1.Name.Should().Be(firstPlayerName);
            match.Player2.Should().NotBeNull();
            match.Player2.Name.Should().Be(secondPlayerName);
            match.StartDateTime.Should().NotBeBefore(DateTime.Now);
            match.GroupId.Should().Be(bracketGroup.Id);
            match.Group.Should().Be(bracketGroup);
        }

        [Fact]
        public void MatchMustContainDifferentPlayers()
        {
            string playerName = "Maru";

            InitializeFirstMatch(playerName, playerName);

            bracketGroup.Matches.Should().BeEmpty();
        }

        [Fact]
        public void CanAssignNewPlayerReferencesToMatch()
        {
            Match match = InitializeFirstMatch();

            PlayerReference taejaPlayerReference = PlayerReference.Create("Taeja", tournament);
            PlayerReference rainPlayerReference = PlayerReference.Create("Rain", tournament);

            match.AssignPlayerReferences(taejaPlayerReference, rainPlayerReference);

            match.Player1.PlayerReference.Should().Be(taejaPlayerReference);
            match.Player2.PlayerReference.Should().Be(rainPlayerReference);
        }

        [Fact]
        public void CannotAssignSamePlayerReferenceAsBothPlayersInMatch()
        {
            Match match = InitializeFirstMatch();

            PlayerReference firstPlayerReference = match.Player1.PlayerReference;
            PlayerReference secondPlayerReference = match.Player2.PlayerReference;

            PlayerReference playerReference = PlayerReference.Create("Taeja", tournament);

            match.AssignPlayerReferences(playerReference, playerReference);

            match.Player1.PlayerReference.Should().Be(firstPlayerReference);
            match.Player2.PlayerReference.Should().Be(secondPlayerReference);
        }

        [Fact]
        public void CanAssignNullPlayerReferenceToEitherPlayerInMatch()
        {
            Match match = InitializeFirstMatch();

            PlayerReference maruPlayerReference = PlayerReference.Create("Maru", tournament);
            PlayerReference storkPlayerReference = PlayerReference.Create("Stork", tournament);

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
            Match match = InitializeFirstMatch();

            match.AssignPlayerReferences(null, null);

            match.Player1.PlayerReference.Should().Be(null);
            match.Player2.PlayerReference.Should().Be(null);
        }

        [Fact]
        public void MatchIsReadyWhenPlayerReferencesHasBeenAssignedToPlayers()
        {
            Match match = InitializeFirstMatch();

            match.IsReady().Should().BeTrue();
            match.Player1.PlayerReference.Should().NotBeNull();
            match.Player2.PlayerReference.Should().NotBeNull();
        }

        [Fact]
        public void MatchIsNotReadyWhenNoPlayerReferenceHasBeenAssignedToMatch()
        {
            Match match = InitializeFirstMatch();

            match.AssignPlayerReferences(null, null);

            match.IsReady().Should().BeFalse();
            match.Player1.PlayerReference.Should().BeNull();
            match.Player2.PlayerReference.Should().BeNull();
        }

        [Fact]
        public void MatchIsNotReadyWhenEitherPlayerReferenceHasBeenAssignedNull()
        {
            Match match = InitializeFirstMatch();

            PlayerReference firstPlayerReference = match.Player1.PlayerReference;
            PlayerReference secondPlayerReference = match.Player2.PlayerReference;

            match.AssignPlayerReferences(firstPlayerReference, null);

            match.IsReady().Should().BeFalse();
            match.Player1.PlayerReference.Should().Be(firstPlayerReference);
            match.Player2.PlayerReference.Should().BeNull();

            match.AssignPlayerReferences(null, secondPlayerReference);

            match.IsReady().Should().BeFalse();
            match.Player1.PlayerReference.Should().BeNull();
            match.Player2.PlayerReference.Should().Be(secondPlayerReference);
        }

        [Fact]
        public void CanFindPlayerInMatchByPlayerName()
        {
            string firstPlayerName = "Maru";
            string secondPlayerName = "Stork";

            Match match = InitializeFirstMatch(firstPlayerName, secondPlayerName);

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
            Match match = InitializeFirstMatch();

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
            Match match = InitializeFirstMatch();

            Player foundPlayer = match.FindPlayer("non-existing-player");

            foundPlayer.Should().BeNull();
        }

        [Fact]
        public void ReturnsNullWhenLookingForNonExistingPlayerInMatchByPlayerId()
        {
            Match match = InitializeFirstMatch();

            Player foundPlayer = match.FindPlayer(Guid.NewGuid());

            foundPlayer.Should().BeNull();
        }

        [Fact]
        public void MatchStartDateTimeCannotBeChangedToSometimeInThePast()
        {
            Match match = InitializeFirstMatch();
            DateTime initialDateTime = match.StartDateTime;

            match.SetStartDateTime(DateTime.Now.AddSeconds(-1));

            match.StartDateTime.Should().Be(initialDateTime);
        }

        [Fact]
        public void PlayStateIsEqualToNotBegunBeforeMactchHasStarted()
        {
            Match match = InitializeFirstMatch();

            PlayState playState = match.GetPlayState();

            playState.Should().Be(PlayState.NotBegun);
        }

        [Fact]
        public void PlayStateIsEqualToIsPlayingWhenMatchHasStartedButNotFinished()
        {
            Match match = InitializeFirstMatch();

            SystemTimeMocker.Set(match.StartDateTime.AddMinutes(1));
            match.Player1.IncreaseScore(GetWinningScore() - 1);

            PlayState playState = match.GetPlayState();

            playState.Should().Be(PlayState.IsPlaying);
        }

        [Fact]
        public void PlayStateIsEqualToIsFinishedWhenMatchHasAWinner()
        {
            Match match = InitializeFirstMatch();

            SystemTimeMocker.Set(match.StartDateTime.AddMinutes(1));
            match.Player1.IncreaseScore(GetWinningScore());

            PlayState playState = match.GetPlayState();

            playState.Should().Be(PlayState.IsFinished);
        }

        [Fact]
        public void CanGetWinningPlayerWhenMatchIsFinished()
        {
            Match match = InitializeFirstMatch();

            SystemTimeMocker.Set(match.StartDateTime.AddMinutes(1));
            match.Player1.IncreaseScore(GetWinningScore());

            Player player = match.GetWinningPlayer();

            player.Should().Be(match.Player1);
        }

        [Fact]
        public void CanGetLosingPlayerWhenMatchIsFinished()
        {
            Match match = InitializeFirstMatch();

            SystemTimeMocker.Set(match.StartDateTime.AddMinutes(1));
            match.Player1.IncreaseScore(GetWinningScore());

            Player player = match.GetLosingPlayer();

            player.Should().Be(match.Player2);
        }

        [Fact]
        public void CannotGetWinningPlayerBeforeMatchHasStarted()
        {
            Match match = InitializeFirstMatch();

            Player player = match.GetWinningPlayer();

            player.Should().BeNull();
        }

        [Fact]
        public void CannotGetLosingPlayerBeforeMatchHasStarted()
        {
            Match match = InitializeFirstMatch();

            Player player = match.GetLosingPlayer();

            player.Should().BeNull();
        }

        [Fact]
        public void CannotGetWinningPlayerWhileMatchIsPlaying()
        {
            Match match = InitializeFirstMatch();

            SystemTimeMocker.Set(match.StartDateTime.AddMinutes(1));
            match.Player1.IncreaseScore(GetWinningScore() - 1);

            Player player = match.GetWinningPlayer();

            player.Should().BeNull();
        }

        [Fact] 
        public void CannotGetLosingPlayerWhileMatchIsPlaying()
        {
            Match match = InitializeFirstMatch();

            SystemTimeMocker.Set(match.StartDateTime.AddMinutes(1));
            match.Player1.IncreaseScore(GetWinningScore() - 1);

            Player player = match.GetLosingPlayer();

            player.Should().BeNull();
        }

        [Fact]
        public void CannotIncreaseScoreBeforeMatchHasStarted()
        {
            Match match = InitializeFirstMatch();

            match.Player1.IncreaseScore(1);
            match.Player2.IncreaseScore(1);

            match.Player1.Score.Should().Be(0);
            match.Player2.Score.Should().Be(0);
        }

        [Fact]
        public void CannotIncreaseScoreWhenMatchIsFinished()
        {
            Match match = InitializeFirstMatch();

            match.Player1.IncreaseScore(1);
            match.Player2.IncreaseScore(1);

            match.Player1.Score.Should().Be(0);
            match.Player2.Score.Should().Be(0);
        }

        private Match InitializeFirstMatch(string firstPlayerName = "Maru", string secondPlayerName = "Stork")
        {
            bracketGroup.AddPlayerReference(firstPlayerName);
            bracketGroup.AddPlayerReference(secondPlayerName);

            if(bracketGroup.Matches.Count > 0)
            {
                return bracketGroup.Matches.First();
            }

            return null;
        }

        private int GetWinningScore()
        {
            return (int)Math.Ceiling(bracketRound.BestOf / 2.0);
        }
    }
}
