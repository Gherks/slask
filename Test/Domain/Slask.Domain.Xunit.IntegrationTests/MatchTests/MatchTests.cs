using FluentAssertions;
using Slask.Common;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Domain.Utilities;
using System;
using System.Linq;
using Xunit;

namespace Slask.Domain.Xunit.IntegrationTests.MatchTests
{
    public class MatchTests : IDisposable
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
            tournament.RegisterPlayerReference(firstPlayerName);
            tournament.RegisterPlayerReference(secondPlayerName);
            bracketGroup = bracketRound.Groups.First() as BracketGroup;
            match = bracketGroup.Matches.First();
        }

        public void Dispose()
        {
            SystemTimeMocker.Reset();
        }

        [Fact]
        public void CanCreateMatch()
        {
            match.Should().NotBeNull();
            match.BestOf.Should().Be(3);
            match.Player1.GetName().Should().Be(firstPlayerName);
            match.Player2.GetName().Should().Be(secondPlayerName);
            match.StartDateTime.Should().NotBeBefore(SystemTime.Now);
            match.GroupId.Should().Be(bracketGroup.Id);
            match.Group.Should().Be(bracketGroup);
        }

        [Fact]
        public void CanChangeBestOf()
        {
            int bestOf = 9;

            bool setResult = match.SetBestOf(bestOf);

            setResult.Should().BeTrue();
            match.BestOf.Should().Be(bestOf);
        }

        [Fact]
        public void CannotSetRoundBestOfToZero()
        {
            bool setResult = match.SetBestOf(0);

            setResult.Should().BeFalse();
            match.BestOf.Should().Be(3);
        }

        [Fact]
        public void CannotSetRoundBestOfToEvenValue()
        {
            match.SetBestOf(1);

            for (int bestOf = 1; bestOf < 32; ++bestOf)
            {
                bool setResult = match.SetBestOf(bestOf);

                bool bestOfIsEven = bestOf % 2 == 0;
                if (bestOfIsEven)
                {
                    setResult.Should().BeFalse();
                    match.BestOf.Should().Be(bestOf - 1);
                }
                else
                {
                    setResult.Should().BeTrue();
                    match.BestOf.Should().Be(bestOf);
                }
            }
        }

        [Fact]
        public void CannotChangeBestOfWhenMatchHasStarted()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            bool setResult = match.SetBestOf(7);

            setResult.Should().BeFalse();
            match.BestOf.Should().Be(3);
        }

        [Fact]
        public void MatchMustContainDifferentPlayers()
        {
            Tournament tournament = Tournament.Create("GSL 2019");
            BracketRound bracketRound = tournament.AddBracketRound() as BracketRound;
            PlayerReference playerReference = tournament.RegisterPlayerReference(firstPlayerName);

            Match match = bracketRound.Groups.First().Matches.First();

            match.AssignPlayerReferencesToPlayers(playerReference.Id, playerReference.Id);

            match.Player1.PlayerReferenceId.Should().Be(playerReference.Id);
            match.Player2.PlayerReferenceId.Should().BeEmpty();
        }

        [Fact]
        public void CanAssignNewPlayerReferencesToMatch()
        {
            PlayerReference taejaPlayerReference = PlayerReference.Create("Taeja", tournament);
            PlayerReference rainPlayerReference = PlayerReference.Create("Rain", tournament);

            match.AssignPlayerReferencesToPlayers(taejaPlayerReference.Id, rainPlayerReference.Id);

            match.Player1.PlayerReferenceId.Should().Be(taejaPlayerReference.Id);
            match.Player2.PlayerReferenceId.Should().Be(rainPlayerReference.Id);
        }

        [Fact]
        public void CannotAssignSamePlayerReferenceAsBothPlayersInMatch()
        {
            Guid firstPlayerReferenceId = match.Player1.PlayerReferenceId;
            Guid secondPlayerReferenceId = match.Player2.PlayerReferenceId;

            PlayerReference playerReference = PlayerReference.Create("Taeja", tournament);

            match.AssignPlayerReferencesToPlayers(playerReference.Id, playerReference.Id);

            match.Player1.PlayerReferenceId.Should().Be(firstPlayerReferenceId);
            match.Player2.PlayerReferenceId.Should().Be(secondPlayerReferenceId);
        }

        [Fact]
        public void CanAssignNullPlayerReferenceToEitherPlayerInMatch()
        {
            PlayerReference maruPlayerReference = PlayerReference.Create("Maru", tournament);
            PlayerReference storkPlayerReference = PlayerReference.Create("Stork", tournament);

            match.AssignPlayerReferencesToPlayers(maruPlayerReference.Id, storkPlayerReference.Id);
            match.AssignPlayerReferencesToPlayers(Guid.Empty, storkPlayerReference.Id);

            match.Player1.PlayerReferenceId.Should().BeEmpty();
            match.Player2.PlayerReferenceId.Should().Be(storkPlayerReference.Id);

            match.AssignPlayerReferencesToPlayers(maruPlayerReference.Id, storkPlayerReference.Id);
            match.AssignPlayerReferencesToPlayers(maruPlayerReference.Id, Guid.Empty);

            match.Player1.PlayerReferenceId.Should().Be(maruPlayerReference.Id);
            match.Player2.PlayerReferenceId.Should().BeEmpty();
        }

        [Fact]
        public void CanAssignNullPlayerReferenceToBothPlayersInMatch()
        {
            match.AssignPlayerReferencesToPlayers(Guid.Empty, Guid.Empty);

            match.Player1.PlayerReferenceId.Should().BeEmpty();
            match.Player2.PlayerReferenceId.Should().BeEmpty();
        }

        [Fact]
        public void MatchIsReadyWhenPlayerReferencesHasBeenAssignedToPlayers()
        {
            match.IsReady().Should().BeTrue();
            match.Player1.PlayerReferenceId.Should().NotBeEmpty();
            match.Player2.PlayerReferenceId.Should().NotBeEmpty();
        }

        [Fact]
        public void MatchIsNotReadyWhenNoPlayerReferenceHasBeenAssignedToMatch()
        {
            match.AssignPlayerReferencesToPlayers(Guid.Empty, Guid.Empty);

            match.IsReady().Should().BeFalse();
            match.Player1.PlayerReferenceId.Should().BeEmpty();
            match.Player2.PlayerReferenceId.Should().BeEmpty();
        }

        [Fact]
        public void MatchIsNotReadyWhenEitherPlayerReferenceHasBeenAssignedNull()
        {
            Guid firstPlayerReferenceId = match.Player1.PlayerReferenceId;
            Guid secondPlayerReferenceId = match.Player2.PlayerReferenceId;

            match.AssignPlayerReferencesToPlayers(firstPlayerReferenceId, Guid.Empty);

            match.IsReady().Should().BeFalse();
            match.Player1.PlayerReferenceId.Should().Be(firstPlayerReferenceId);
            match.Player2.PlayerReferenceId.Should().BeEmpty();

            match.AssignPlayerReferencesToPlayers(Guid.Empty, secondPlayerReferenceId);

            match.IsReady().Should().BeFalse();
            match.Player1.PlayerReferenceId.Should().BeEmpty();
            match.Player2.PlayerReferenceId.Should().Be(secondPlayerReferenceId);
        }

        [Fact]
        public void CanFindPlayerInMatchByPlayerName()
        {
            Player firstFoundPlayer = match.FindPlayer(firstPlayerName);
            Player secondFoundPlayer = match.FindPlayer(secondPlayerName);

            firstFoundPlayer.Should().NotBeNull();
            firstFoundPlayer.Id.Should().Be(match.Player1.Id);
            firstFoundPlayer.GetName().Should().Be(firstPlayerName);

            secondFoundPlayer.Should().NotBeNull();
            secondFoundPlayer.Id.Should().Be(match.Player2.Id);
            secondFoundPlayer.GetName().Should().Be(secondPlayerName);
        }

        [Fact]
        public void CanFindPlayerInMatchByPlayerId()
        {
            Player firstFoundPlayer = match.FindPlayer(match.Player1.Id);
            Player secondFoundPlayer = match.FindPlayer(match.Player2.Id);

            firstFoundPlayer.Should().NotBeNull();
            firstFoundPlayer.Id.Should().Be(match.Player1.Id);
            firstFoundPlayer.GetName().Should().Be(match.Player1.GetName());

            secondFoundPlayer.Should().NotBeNull();
            secondFoundPlayer.Id.Should().Be(match.Player2.Id);
            secondFoundPlayer.GetName().Should().Be(match.Player2.GetName());
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
            PlayStateEnum playState = match.GetPlayState();

            playState.Should().Be(PlayStateEnum.NotBegun);
        }

        [Fact]
        public void PlayStateIsEqualToOngoingWhenMatchHasStartedButNotFinished()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.Player1.IncreaseScore(GetWinningScore() - 1);

            PlayStateEnum playState = match.GetPlayState();

            playState.Should().Be(PlayStateEnum.Ongoing);
        }

        [Fact]
        public void PlayStateIsEqualToFinishedWhenMatchHasAWinner()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.Player1.IncreaseScore(GetWinningScore());

            PlayStateEnum playState = match.GetPlayState();

            playState.Should().Be(PlayStateEnum.Finished);
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

            match.AssignPlayerReferenceToFirstAvailablePlayer(playerReference.Id);

            match.Player1.PlayerReferenceId.Should().NotBeEmpty();
            match.Player1.GetName().Should().Be(firstPlayerName);
            match.Player2.PlayerReferenceId.Should().NotBeEmpty();
            match.Player2.GetName().Should().Be(secondPlayerName);
        }

        private int GetWinningScore()
        {
            return (int)Math.Ceiling(match.BestOf / 2.0);
        }
    }
}
