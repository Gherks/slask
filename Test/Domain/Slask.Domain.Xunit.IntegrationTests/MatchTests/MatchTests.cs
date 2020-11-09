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
            match.GetPlayer1Name().Should().Be(firstPlayerName);
            match.GetPlayer2Name().Should().Be(secondPlayerName);
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

            match.PlayerReference1Id.Should().Be(playerReference.Id);
            match.PlayerReference2Id.Should().BeEmpty();
        }

        [Fact]
        public void CanAssignNewPlayerReferencesToMatch()
        {
            PlayerReference taejaPlayerReference = PlayerReference.Create("Taeja", tournament);
            PlayerReference rainPlayerReference = PlayerReference.Create("Rain", tournament);

            match.AssignPlayerReferencesToPlayers(taejaPlayerReference.Id, rainPlayerReference.Id);

            match.PlayerReference1Id.Should().Be(taejaPlayerReference.Id);
            match.PlayerReference2Id.Should().Be(rainPlayerReference.Id);
        }

        [Fact]
        public void CannotAssignSamePlayerReferenceAsBothPlayersInMatch()
        {
            Guid firstPlayerReferenceId = match.PlayerReference1Id;
            Guid secondPlayerReferenceId = match.PlayerReference2Id;

            PlayerReference playerReference = PlayerReference.Create("Taeja", tournament);

            match.AssignPlayerReferencesToPlayers(playerReference.Id, playerReference.Id);

            match.PlayerReference1Id.Should().Be(firstPlayerReferenceId);
            match.PlayerReference2Id.Should().Be(secondPlayerReferenceId);
        }

        [Fact]
        public void CanAssignNullPlayerReferenceToEitherPlayerInMatch()
        {
            PlayerReference maruPlayerReference = PlayerReference.Create("Maru", tournament);
            PlayerReference storkPlayerReference = PlayerReference.Create("Stork", tournament);

            match.AssignPlayerReferencesToPlayers(maruPlayerReference.Id, storkPlayerReference.Id);
            match.AssignPlayerReferencesToPlayers(Guid.Empty, storkPlayerReference.Id);

            match.PlayerReference1Id.Should().BeEmpty();
            match.PlayerReference2Id.Should().Be(storkPlayerReference.Id);

            match.AssignPlayerReferencesToPlayers(maruPlayerReference.Id, storkPlayerReference.Id);
            match.AssignPlayerReferencesToPlayers(maruPlayerReference.Id, Guid.Empty);

            match.PlayerReference1Id.Should().Be(maruPlayerReference.Id);
            match.PlayerReference2Id.Should().BeEmpty();
        }

        [Fact]
        public void CanAssignNullPlayerReferenceToBothPlayersInMatch()
        {
            match.AssignPlayerReferencesToPlayers(Guid.Empty, Guid.Empty);

            match.PlayerReference1Id.Should().BeEmpty();
            match.PlayerReference2Id.Should().BeEmpty();
        }

        [Fact]
        public void MatchIsReadyWhenPlayerReferencesHasBeenAssignedToPlayers()
        {
            match.IsReady().Should().BeTrue();
            match.PlayerReference1Id.Should().NotBeEmpty();
            match.PlayerReference2Id.Should().NotBeEmpty();
        }

        [Fact]
        public void MatchIsNotReadyWhenNoPlayerReferenceHasBeenAssignedToMatch()
        {
            match.AssignPlayerReferencesToPlayers(Guid.Empty, Guid.Empty);

            match.IsReady().Should().BeFalse();
            match.PlayerReference1Id.Should().BeEmpty();
            match.PlayerReference2Id.Should().BeEmpty();
        }

        [Fact]
        public void MatchIsNotReadyWhenEitherPlayerReferenceHasBeenAssignedNull()
        {
            Guid firstPlayerReferenceId = match.PlayerReference1Id;
            Guid secondPlayerReferenceId = match.PlayerReference2Id;

            match.AssignPlayerReferencesToPlayers(firstPlayerReferenceId, Guid.Empty);

            match.IsReady().Should().BeFalse();
            match.PlayerReference1Id.Should().Be(firstPlayerReferenceId);
            match.PlayerReference2Id.Should().BeEmpty();

            match.AssignPlayerReferencesToPlayers(Guid.Empty, secondPlayerReferenceId);

            match.IsReady().Should().BeFalse();
            match.PlayerReference1Id.Should().BeEmpty();
            match.PlayerReference2Id.Should().Be(secondPlayerReferenceId);
        }

        [Fact]
        public void CanFindPlayerInMatchByPlayerName()
        {
            Guid firstFoundPlayerReferenceId = match.FindPlayer(firstPlayerName);
            Guid secondFoundPlayerReferenceId = match.FindPlayer(secondPlayerName);

            firstFoundPlayerReferenceId.Should().Be(match.PlayerReference1Id);
            match.GetPlayer1Name().Should().Be(firstPlayerName);

            secondFoundPlayerReferenceId.Should().Be(match.PlayerReference2Id);
            match.GetPlayer2Name().Should().Be(secondPlayerName);
        }

        [Fact]
        public void ReturnsNullWhenLookingForNonExistingPlayerInMatchByPlayerName()
        {
            Guid foundPlayerReferenceId = match.FindPlayer("non-existing-player");

            foundPlayerReferenceId.Should().BeEmpty();
        }

        [Fact]
        public void ReturnsFalseWhenLookingForNonExistingPlayerInMatchByPlayerId()
        {
            bool searchResult = match.HasPlayer(Guid.NewGuid());

            searchResult.Should().BeFalse();
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

            match.SetStartDateTime(SystemTime.Now.AddSeconds(-1));

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
            match.IncreaseScoreForPlayer1(GetWinningScore() - 1);

            PlayStateEnum playState = match.GetPlayState();

            playState.Should().Be(PlayStateEnum.Ongoing);
        }

        [Fact]
        public void PlayStateIsEqualToFinishedWhenMatchHasAWinner()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.IncreaseScoreForPlayer1(GetWinningScore());

            PlayStateEnum playState = match.GetPlayState();

            playState.Should().Be(PlayStateEnum.Finished);
        }

        [Fact]
        public void CanGetWinningPlayerWhenMatchIsFinished()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.IncreaseScoreForPlayer1(GetWinningScore());

            Guid playerReferenceId = match.GetWinningPlayerReference();

            playerReferenceId.Should().Be(match.PlayerReference1Id);
        }

        [Fact]
        public void CanGetLosingPlayerWhenMatchIsFinished()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.IncreaseScoreForPlayer1(GetWinningScore());

            Guid playerReferenceId = match.GetLosingPlayerReference();

            playerReferenceId.Should().Be(match.PlayerReference2Id);
        }

        [Fact]
        public void CannotGetWinningPlayerBeforeMatchHasStarted()
        {
            Guid playerReferenceId = match.GetWinningPlayerReference();

            playerReferenceId.Should().BeEmpty();
        }

        [Fact]
        public void CannotGetLosingPlayerBeforeMatchHasStarted()
        {
            Guid playerReferenceId = match.GetLosingPlayerReference();

            playerReferenceId.Should().BeEmpty();
        }

        [Fact]
        public void CannotGetWinningPlayerWhileMatchIsOngoing()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.IncreaseScoreForPlayer1(GetWinningScore() - 1);

            Guid playerReferenceId = match.GetWinningPlayerReference();

            playerReferenceId.Should().BeEmpty();
        }

        [Fact]
        public void CannotGetLosingPlayerWhileMatchIsOngoing()
        {
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
            match.IncreaseScoreForPlayer1(GetWinningScore() - 1);

            Guid playerReferenceId = match.GetLosingPlayerReference();

            playerReferenceId.Should().BeEmpty();
        }

        [Fact]
        public void CannotIncreaseScoreBeforeMatchHasStarted()
        {
            match.IncreaseScoreForPlayer1(1);
            match.IncreaseScoreForPlayer2(1);

            match.Player1Score.Should().Be(0);
            match.Player2Score.Should().Be(0);
        }

        [Fact]
        public void CannotIncreaseScoreWhenMatchIsFinished()
        {
            match.IncreaseScoreForPlayer1(1);
            match.IncreaseScoreForPlayer2(1);

            match.Player1Score.Should().Be(0);
            match.Player2Score.Should().Be(0);
        }

        [Fact]
        public void MatchRemainsUnchangedWhenAddingPlayerReferenceToMatchWithTwoPlayersAlready()
        {
            PlayerReference playerReference = PlayerReference.Create("Taeja", tournament);

            match.AssignPlayerReferenceToFirstAvailablePlayer(playerReference.Id);

            match.PlayerReference1Id.Should().NotBeEmpty();
            match.GetPlayer1Name().Should().Be(firstPlayerName);
            match.PlayerReference2Id.Should().NotBeEmpty();
            match.GetPlayer2Name().Should().Be(secondPlayerName);
        }

        private int GetWinningScore()
        {
            return (int)Math.Ceiling(match.BestOf / 2.0);
        }
    }
}
