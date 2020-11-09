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
            tournament.RegisterPlayerReference("Maru");
            tournament.RegisterPlayerReference("Stork");
            group = round.Groups.First();
            match = group.Matches.First();
        }

        [Fact]
        public void CanPlaceMatchBetOnMatch()
        {
            Better better = tournament.AddBetter(user);

            better.PlaceMatchBet(match, match.PlayerReference1Id);

            better.Bets.Should().HaveCount(1);
            better.Bets.First().Should().NotBeNull();
            MatchBet matchBet = better.Bets.First() as MatchBet;
            ValidateMatchBet(matchBet, better, match, match.PlayerReference1Id);
        }

        [Fact]
        public void CanReplaceAMatchBetOnMatch()
        {
            Better better = tournament.AddBetter(user);

            better.PlaceMatchBet(match, match.PlayerReference1Id);

            better.Bets.Should().HaveCount(1);
            better.Bets.First().Should().NotBeNull();
            MatchBet matchBet = better.Bets.First() as MatchBet;
            ValidateMatchBet(matchBet, better, match, match.PlayerReference1Id);

            better.PlaceMatchBet(match, match.PlayerReference2Id);

            better.Bets.Should().HaveCount(1);
            better.Bets.First().Should().NotBeNull();
            matchBet = better.Bets.First() as MatchBet;
            ValidateMatchBet(matchBet, better, match, match.PlayerReference2Id);
        }

        [Fact]
        public void CannotPlaceMatchBetOnWithoutMatch()
        {
            Better better = tournament.AddBetter(user);
            round.SetPlayersPerGroupCount(3);
            tournament.RegisterPlayerReference("Taeja");

            group = round.Groups.First();
            Match incompleteMatch = group.Matches.Last();

            better.PlaceMatchBet(null, incompleteMatch.PlayerReference1Id);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void CannotPlaceMatchBetOnWithoutPlayer()
        {
            Better better = tournament.AddBetter(user);
            tournament.RegisterPlayerReference("Maru");
            round.SetPlayersPerGroupCount(2);
            tournament.RegisterPlayerReference("Taeja");

            group = round.Groups.First();
            Match match = group.Matches.First();

            better.PlaceMatchBet(null, match.PlayerReference1Id);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void CannotPlaceMatchBetOnMatchThatIsNotReady()
        {
            Better better = tournament.AddBetter(user);
            tournament.RegisterPlayerReference("Taeja");
            round.SetPlayersPerGroupCount(3);

            group = round.Groups.First();
            Match incompleteMatch = group.Matches.Last();

            better.PlaceMatchBet(incompleteMatch, incompleteMatch.PlayerReference1Id);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void CannotPlaceMatchBetOnMatchThatIsOngoing()
        {
            Better better = tournament.AddBetter(user);
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            better.PlaceMatchBet(match, match.PlayerReference1Id);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void CannotReplaceMatchBetOnMatchThatIsOngoing()
        {
            Better better = tournament.AddBetter(user);

            better.PlaceMatchBet(match, match.PlayerReference1Id);
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            better.PlaceMatchBet(match, match.PlayerReference2Id);

            better.Bets.Should().HaveCount(1);
            better.Bets.First().Should().NotBeNull();

            MatchBet matchBet = better.Bets.First() as MatchBet;
            ValidateMatchBet(matchBet, better, match, match.PlayerReference1Id);
        }

        [Fact]
        public void CannotPlaceMatchBetOnMatchThatIsFinished()
        {
            Better better = tournament.AddBetter(user);
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            int winningScore = (int)Math.Ceiling(match.BestOf / 2.0);
            match.IncreaseScoreForPlayer1(winningScore);

            better.PlaceMatchBet(match, match.PlayerReference2Id);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void MatchBetIsRemovedWhenMatchLayoutIsChangedInOneMatch()
        {
            Better better = tournament.AddBetter(user);

            better.PlaceMatchBet(match, match.PlayerReference1Id);

            PlayerSwitcher.SwitchMatchesOn(match, match.PlayerReference1Id, match, match.PlayerReference2Id);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void MatchBetIsRemovedWhenMatchLayoutIsChangedInTwoMatches()
        {
            Better better = tournament.AddBetter(user);

            round.SetPlayersPerGroupCount(4);
            tournament.RegisterPlayerReference("Taeja");
            tournament.RegisterPlayerReference("Rain");
            group = round.Groups.First();

            Match firstMatch = group.Matches[0];
            Match secondMatch = group.Matches[1];

            better.PlaceMatchBet(firstMatch, firstMatch.PlayerReference1Id);
            better.PlaceMatchBet(secondMatch, secondMatch.PlayerReference1Id);

            PlayerSwitcher.SwitchMatchesOn(firstMatch, firstMatch.PlayerReference1Id, secondMatch, secondMatch.PlayerReference1Id);

            better.Bets.Should().BeEmpty();
        }

        private void ValidateMatchBet(MatchBet matchBet, Better correctBetter, Match correctMatch, Guid correctPlayerReferenceId)
        {
            matchBet.BetterId.Should().Be(correctBetter.Id);
            matchBet.MatchId.Should().Be(correctMatch.Id);
            matchBet.PlayerReferenceId.Should().Be(correctPlayerReferenceId);
        }
    }
}
