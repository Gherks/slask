using FluentAssertions;
using Slask.Common;
using Slask.Domain.Bets.BetTypes;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds;
using System;
using System.Linq;
using Xunit;

namespace Slask.Domain.Xunit.IntegrationTests
{
    public class MatchBetTests
    {
        private readonly User user;
        private readonly Tournament tournament;
        private readonly RoundBase round;
        private GroupBase group;
        private Match match;

        public MatchBetTests()
        {
            user = User.Create("Stålberto");
            tournament = Tournament.Create("GSL 2019");
            round = tournament.AddBracketRound();
            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Stork");
            group = round.Groups.First();
            match = group.Matches.First();
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
        public void CanReplaceAMatchBetOnMatch()
        {
            Better better = tournament.AddBetter(user);

            better.PlaceMatchBet(match, match.Player1);

            better.Bets.Should().HaveCount(1);
            better.Bets.First().Should().NotBeNull();
            MatchBet matchBet = better.Bets.First() as MatchBet;
            ValidateMatchBet(matchBet, better, match, match.Player1);

            better.PlaceMatchBet(match, match.Player2);

            better.Bets.Should().HaveCount(1);
            better.Bets.First().Should().NotBeNull();
            matchBet = better.Bets.First() as MatchBet;
            ValidateMatchBet(matchBet, better, match, match.Player2);
        }

        [Fact]
        public void CannotPlaceMatchBetOnWithoutMatch()
        {
            Better better = tournament.AddBetter(user);
            round.SetPlayersPerGroupCount(3);
            round.RegisterPlayerReference("Taeja");

            group = round.Groups.First();
            Match incompleteMatch = group.Matches.Last();

            better.PlaceMatchBet(null, incompleteMatch.Player1);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void CannotPlaceMatchBetOnWithoutPlayer()
        {
            Better better = tournament.AddBetter(user);
            round.SetPlayersPerGroupCount(2);
            round.RegisterPlayerReference("Maru");
            round.RegisterPlayerReference("Taeja");

            group = round.Groups.First();
            Match match = group.Matches.First();

            better.PlaceMatchBet(null, match.Player1);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void CannotPlaceMatchBetOnMatchThatIsNotReady()
        {
            Better better = tournament.AddBetter(user);
            round.SetPlayersPerGroupCount(3);
            round.RegisterPlayerReference("Taeja");

            group = round.Groups.First();
            Match incompleteMatch = group.Matches.Last();

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

            round.SetPlayersPerGroupCount(4);
            round.RegisterPlayerReference("Taeja");
            round.RegisterPlayerReference("Rain");
            group = round.Groups.First();

            Match firstMatch = group.Matches[0];
            Match secondMatch = group.Matches[1];

            better.PlaceMatchBet(firstMatch, firstMatch.Player1);
            better.PlaceMatchBet(secondMatch, secondMatch.Player1);

            PlayerSwitcher.SwitchMatchesOn(firstMatch.Player1, secondMatch.Player1);

            better.Bets.Should().BeEmpty();
        }

        private void ValidateMatchBet(MatchBet matchBet, Better correctBetter, Match correctMatch, Player correctPlayer)
        {
            matchBet.BetterId.Should().Be(correctBetter.Id);
            matchBet.MatchId.Should().Be(correctMatch.Id);
            matchBet.PlayerId.Should().Be(correctPlayer.Id);
        }
    }
}
