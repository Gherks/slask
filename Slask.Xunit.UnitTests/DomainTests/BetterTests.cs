using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Bets;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds;
using System;
using System.Linq;
using Xunit;

namespace Slask.Xunit.UnitTests.DomainTests
{
    public class BetterTests
    {
        private readonly User user;
        private readonly Tournament tournament;
        private readonly BracketRound bracketRound;
        private BracketGroup bracketGroup;
        private Match match;

        public BetterTests()
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
        public void CanCreateBetter()
        {
            Better better = GivenABetterIsCreated();

            better.Id.Should().NotBeEmpty();
            better.User.Should().NotBeNull();
            better.Bets.Should().BeEmpty();
            better.TournamentId.Should().Be(tournament.Id);
            better.Tournament.Should().Be(tournament);
        }

        [Fact]
        public void CannotCreateBetterWithNullUser()
        {
            Better better = Better.Create(null, tournament);

            better.Should().BeNull();
        }

        [Fact]
        public void CannotCreateBetterWithNullTournament()
        {
            Better better = Better.Create(user, null);

            better.Should().BeNull();
        }

        [Fact]
        public void CanPlaceMatchBetOnMatch()
        {
            Better better = GivenABetterIsCreated();

            better.PlaceMatchBet(match, match.Player1);

            better.Bets.Should().HaveCount(1);
            better.Bets.First().Should().NotBeNull();
            MatchBet matchBet = better.Bets.First() as MatchBet;
            ValidateMatchBet(matchBet, better, match, match.Player1);
        }

        [Fact]
        public void CannotPlaceMatchBetOnMatchThatIsNotReady()
        {
            Better better = GivenABetterIsCreated();
            bracketRound.SetPlayersPerGroupCount(3);
            bracketRound.RegisterPlayerReference("Taeja");

            bracketGroup = bracketRound.Groups.First() as BracketGroup;
            Match incompleteMatch = bracketGroup.Matches.Last();

            better.PlaceMatchBet(incompleteMatch, incompleteMatch.Player1);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void CannotPlaceMatchBetOnMatchThatOngoing()
        {
            Better better = GivenABetterIsCreated();
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            better.PlaceMatchBet(match, match.Player1);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void CannotPlaceMatchBetOnMatchThatIsFinished()
        {
            Better better = GivenABetterIsCreated();
            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

            int winningScore = (int)Math.Ceiling(bracketRound.BestOf / 2.0);
            match.Player1.IncreaseScore(winningScore);

            better.PlaceMatchBet(match, match.Player1);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void BettingOnMatchThatHasAlreadyBeenBettedOnCreatesANewBet()
        {
            Better better = GivenABetterIsCreated();

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
            Better better = GivenABetterIsCreated();

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
            Better better = GivenABetterIsCreated();

            better.PlaceMatchBet(match, match.Player1);

            PlayerSwitcher.SwitchMatchesOn(match.Player1, match.Player2);

            better.Bets.Should().BeEmpty();
        }

        [Fact]
        public void MatchBetIsRemovedWhenMatchLayoutIsChangedInTwoMatches()
        {
            Better better = GivenABetterIsCreated();

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

        private Better GivenABetterIsCreated()
        {
            return tournament.AddBetter(user);
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
