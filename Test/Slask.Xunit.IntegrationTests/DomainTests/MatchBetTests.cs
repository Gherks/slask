using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Bets.BetTypes;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds.RoundTypes;
using System;
using System.Linq;
using Xunit;

namespace Slask.Xunit.IntegrationTests.DomainTests
{
    public class MatchBetTests
    {
        private readonly User user;
        private readonly Tournament tournament;
        private readonly BracketRound bracketRound;
        private BracketGroup bracketGroup;
        private Match match;

        public MatchBetTests()
        {
            user = User.Create("Stålberto");
            tournament = Tournament.Create("GSL 2019");
            bracketRound = tournament.AddBracketRound() as BracketRound;
            bracketRound.RegisterPlayerReference("Maru");
            bracketRound.RegisterPlayerReference("Stork");
            bracketGroup = bracketRound.Groups.First() as BracketGroup;
            match = bracketGroup.Matches.First();
        }

        [Fact]
        public void CanPlaceMatchBetOnMatch()
        {
            Better better = tournament.AddBetter(user);

            better.PlaceMatchBet(match, match.Player1);

            better.Bets.Should().HaveCount(1);
            better.Bets.First().Should().NotBeNull();
            MatchBet matchBet = better.Bets.First() as MatchBet;
            ValidateMatchBet(matchBet, better, match, match.Player1);
        }

        [Fact]
        public void CannotPlaceMatchBetOnMatchThatIsNotReady()
        {
            Better better = tournament.AddBetter(user);
            bracketRound.SetPlayersPerGroupCount(3);
            bracketRound.RegisterPlayerReference("Taeja");

            bracketGroup = bracketRound.Groups.First() as BracketGroup;
            Match incompleteMatch = bracketGroup.Matches.Last();

            better.PlaceMatchBet(incompleteMatch, incompleteMatch.Player1);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void CannotPlaceMatchBetOnMatchThatIsOngoing()
        {
            Better better = tournament.AddBetter(user);
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            better.PlaceMatchBet(match, match.Player1);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void CannotReplaceMatchBetOnMatchThatIsOngoing()
        {
            Better better = tournament.AddBetter(user);

            better.PlaceMatchBet(match, match.Player1);
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            better.PlaceMatchBet(match, match.Player2);

            better.Bets.Should().HaveCount(1);
            better.Bets.First().Should().NotBeNull();

            MatchBet matchBet = better.Bets.First() as MatchBet;
            ValidateMatchBet(matchBet, better, match, match.Player1);
        }

        [Fact]
        public void CannotPlaceMatchBetOnMatchThatIsFinished()
        {
            Better better = tournament.AddBetter(user);
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            int winningScore = (int)Math.Ceiling(match.BestOf / 2.0);
            match.Player1.IncreaseScore(winningScore);

            better.PlaceMatchBet(match, match.Player1);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void BettingOnMatchThatHasAlreadyBeenBettedOnCreatesANewBet()
        {
            Better better = tournament.AddBetter(user);

            better.PlaceMatchBet(match, match.Player1);
            better.PlaceMatchBet(match, match.Player2);

            better.Bets.Should().HaveCount(1);
            better.Bets.First().Should().NotBeNull();
            MatchBet matchBet = better.Bets.First() as MatchBet;
            ValidateMatchBet(matchBet, better, match, match.Player2);
        }

        [Fact]
        public void CannotCreateNewBetOnMatchThatHasBegun()
        {
            Better better = tournament.AddBetter(user);

            better.PlaceMatchBet(match, match.Player1);

            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            better.PlaceMatchBet(match, match.Player2);

            better.Bets.Should().HaveCount(1);
            better.Bets.First().Should().NotBeNull();
            MatchBet matchBet = better.Bets.First() as MatchBet;
            ValidateMatchBet(matchBet, better, match, match.Player1);
        }

        [Fact]
        public void MatchBetIsRemovedWhenMatchLayoutIsChangedInOneMatch()
        {
            Better better = tournament.AddBetter(user);

            better.PlaceMatchBet(match, match.Player1);

            PlayerSwitcher.SwitchMatchesOn(match.Player1, match.Player2);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void MatchBetIsRemovedWhenMatchLayoutIsChangedInTwoMatches()
        {
            Better better = tournament.AddBetter(user);

            bracketRound.SetPlayersPerGroupCount(4);
            bracketRound.RegisterPlayerReference("Taeja");
            bracketRound.RegisterPlayerReference("Rain");
            bracketGroup = bracketRound.Groups.First() as BracketGroup;

            Match firstMatch = bracketGroup.Matches[0];
            Match secondMatch = bracketGroup.Matches[1];

            better.PlaceMatchBet(firstMatch, firstMatch.Player1);
            better.PlaceMatchBet(secondMatch, secondMatch.Player1);

            PlayerSwitcher.SwitchMatchesOn(firstMatch.Player1, secondMatch.Player1);

            better.Bets.Should().BeEmpty();
        }

        private void ValidateMatchBet(MatchBet matchBet, Better correctBetter, Match correctMatch, Player correctPlayer)
        {
            matchBet.BetterId.Should().Be(correctBetter.Id);
            matchBet.Better.Should().Be(correctBetter);
            matchBet.MatchId.Should().Be(correctMatch.Id);
            matchBet.Match.Should().Be(correctMatch);
            matchBet.PlayerId.Should().Be(correctPlayer.Id);
            matchBet.Player.Should().Be(correctPlayer);
        }
    }
}
