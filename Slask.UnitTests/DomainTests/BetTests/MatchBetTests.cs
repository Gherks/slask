// Feature suggestion: Add option that lets user set whether bet is removed or update to new user
// when players are changed in match?

using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Bets;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using System;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.BetTests
{
    public class MatchBetTests
    {
        // IF ANY PLAYER CHANGES IN MATCH, RESET ALL ASSOCIATED BETS

        private User user;
        private Tournament tournament;
        private Better better;
        private BracketRound round;
        private BracketGroup group;
        private PlayerReference playerReference;
        private Match firstMatch;

        public MatchBetTests()
        {
            user = User.Create("Stålberto");
            tournament = Tournament.Create("GSL 2019");
            better = tournament.AddBetter(user);
            round = tournament.AddBracketRound("Bracket round", 7) as BracketRound;
            group = round.AddGroup() as BracketGroup;
            playerReference = group.AddPlayerReference("Maru");
            group.AddPlayerReference("Stork");
            firstMatch = group.Matches.First();
        }

        [Fact]
        public void CanCreateMatchBet()
        {
            MatchBet matchBet = MatchBet.Create(better, firstMatch, firstMatch.Player1);

            matchBet.Should().NotBeNull();
            matchBet.Id.Should().NotBeEmpty();
            matchBet.BetterId.Should().Be(better.Id);
            matchBet.Better.Should().Be(better);
            matchBet.MatchId.Should().Be(firstMatch.Id);
            matchBet.Match.Should().Be(firstMatch);
            matchBet.PlayerId.Should().Be(firstMatch.Player1.Id);
            matchBet.Player.Should().Be(firstMatch.Player1);
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

        [Fact]
        public void CannotCreateMatchBetForMatchThatIsNotReady()
        {
            group.AddPlayerReference("Taeja");
            Match incompleteMatch = group.Matches.Last();

            MatchBet matchBet = MatchBet.Create(better, incompleteMatch, incompleteMatch.Player1);

            matchBet.Should().BeNull();
        }

        [Fact]
        public void CannotCreateMatchBetForMatchThatIsPlaying()
        {
            SystemTimeMocker.Set(firstMatch.StartDateTime.AddMinutes(1));

            MatchBet matchBet = MatchBet.Create(better, firstMatch, firstMatch.Player1);

            matchBet.Should().BeNull();
        }

        [Fact]
        public void CannotCreateMatchBetForMatchThatIsFinished()
        {
            SystemTimeMocker.Set(firstMatch.StartDateTime.AddMinutes(1));

            int winningScore = (int)Math.Ceiling(round.BestOf / 2.0);
            firstMatch.Player1.IncreaseScore(winningScore);

            MatchBet matchBet = MatchBet.Create(better, firstMatch, firstMatch.Player1);

            matchBet.Should().BeNull();
        }

        [Fact]
        public void CannotCreateMatchBetWithPlayerThatIsNotPresentInGivenMatch()
        {
            group.AddPlayerReference("Taeja");
            group.AddPlayerReference("Rain");
            Match secondMatch = group.Matches[1];

            MatchBet matchBet = MatchBet.Create(better, firstMatch, secondMatch.Player1);

            matchBet.Should().BeNull();
        }

        //[Fact] IS INTEGRATION TEST
        //public void AllMatchBetsForParticularMatchIsRemovedIfMatchParticipantsChanges()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
