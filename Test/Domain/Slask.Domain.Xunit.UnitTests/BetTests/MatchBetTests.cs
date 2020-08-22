using FluentAssertions;
using Slask.Common;
using Slask.Domain.Bets.BetTypes;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Rounds.RoundTypes;
using System;
using System.Linq;
using Xunit;

namespace Slask.Domain.Xunit.UnitTests.BetTests
{
    public class MatchBetTests
    {
        private readonly User user;
        private readonly Tournament tournament;
        private readonly Better better;
        private readonly BracketRound round;
        private readonly BracketGroup group;
        private readonly Match firstMatch;

        public MatchBetTests()
        {
            user = User.Create("Stålberto");
            tournament = Tournament.Create("GSL 2019");
            better = tournament.AddBetter(user);
            round = tournament.AddBracketRound() as BracketRound;
            tournament.RegisterPlayerReference("Maru");
            tournament.RegisterPlayerReference("Stork");
            group = round.Groups.First() as BracketGroup;
            firstMatch = group.Matches.First();
        }

        [Fact]
        public void CanCreateMatchBet()
        {
            MatchBet matchBet = MatchBet.Create(better, firstMatch, firstMatch.Player1);

            matchBet.Should().NotBeNull();
            matchBet.Id.Should().NotBeEmpty();
            matchBet.BetterId.Should().Be(better.Id);
            matchBet.MatchId.Should().Be(firstMatch.Id);
            matchBet.PlayerId.Should().Be(firstMatch.Player1.Id);
        }

        [Fact]
        public void CannotCreateMatchBetWithoutBetter()
        {
            MatchBet matchBet = MatchBet.Create(null, firstMatch, firstMatch.Player1);

            matchBet.Should().BeNull();
        }

        [Fact]
        public void CannotCreateMatchBetWithoutMatch()
        {
            MatchBet matchBet = MatchBet.Create(better, null, firstMatch.Player1);

            matchBet.Should().BeNull();
        }

        [Fact]
        public void CannotCreateMatchBetWithoutPlayer()
        {
            MatchBet matchBet = MatchBet.Create(better, firstMatch, null);

            matchBet.Should().BeNull();
        }

        //[Fact]
        //public void CannotCreateMatchBetForMatchThatIsNotReady()
        //{
        //    group.AddNewPlayerReference("Taeja");
        //    Match incompleteMatch = group.Matches.Last();

        //    MatchBet matchBet = MatchBet.Create(better, incompleteMatch, incompleteMatch.Player1);

        //    matchBet.Should().BeNull();
        //}

        [Fact]
        public void CannotCreateMatchBetForMatchThatIsOngoing()
        {
            SystemTimeMocker.SetOneSecondAfter(firstMatch.StartDateTime);

            MatchBet matchBet = MatchBet.Create(better, firstMatch, firstMatch.Player1);

            matchBet.Should().BeNull();
        }

        [Fact]
        public void CannotCreateMatchBetForMatchThatIsFinished()
        {
            SystemTimeMocker.SetOneSecondAfter(firstMatch.StartDateTime);

            int winningScore = (int)Math.Ceiling(firstMatch.BestOf / 2.0);
            firstMatch.Player1.IncreaseScore(winningScore);

            MatchBet matchBet = MatchBet.Create(better, firstMatch, firstMatch.Player1);

            matchBet.Should().BeNull();
        }

        //[Fact]
        //public void CannotCreateMatchBetWithPlayerThatIsNotPresentInGivenMatch()
        //{
        //    group.AddNewPlayerReference("Taeja");
        //    group.AddNewPlayerReference("Rain");
        //    Match secondMatch = group.Matches[1];

        //    MatchBet matchBet = MatchBet.Create(better, firstMatch, secondMatch.Player1);

        //    matchBet.Should().BeNull();
        //}
    }
}
