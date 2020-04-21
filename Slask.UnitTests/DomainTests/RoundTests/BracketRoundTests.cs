using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.RoundTests
{
    public class BracketRoundTests
    {
        private readonly Tournament tournament;

        public BracketRoundTests()
        {
            tournament = Tournament.Create("GSL 2019");
        }

        [Fact]
        public void CanCreateRound()
        {
            string name = "Bracket round";
            int bestOf = 3;
            int playersPerGroupCount = 2;

            BracketRound bracketRound = BracketRound.Create(name, bestOf, playersPerGroupCount, tournament);

            bracketRound.Should().NotBeNull();
            bracketRound.Id.Should().NotBeEmpty();
            bracketRound.Name.Should().Be(name);
            bracketRound.PlayersPerGroupCount.Should().Be(playersPerGroupCount);
            bracketRound.BestOf.Should().Be(bestOf);
            bracketRound.AdvancingPerGroupCount.Should().Be(1);
            bracketRound.Groups.Should().BeEmpty();
            bracketRound.TournamentId.Should().NotBeEmpty();
            bracketRound.Tournament.Should().NotBeNull();
        }

        [Fact]
        public void CannotCreateRoundWithoutName()
        {
            BracketRound bracketRound = CreateBracketRound("");

            bracketRound.Should().BeNull();
        }

        [Fact]
        public void CannotCreateRoundWithEvenOrZeroBestOfs()
        {
            for (int bestOf = 0; bestOf < 32; ++bestOf)
            {
                BracketRound bracketRound = CreateBracketRound("Bracket round", bestOf);
                bool bestOfIsEven = bestOf % 2 == 0;

                if (bestOfIsEven)
                {
                    bracketRound.Should().BeNull();
                }
                else
                {
                    bracketRound.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void FetchingPreviousRoundFromFirstRoundYieldsNull()
        {
            BracketRound bracketRound = CreateBracketRound();

            RoundBase round = bracketRound.GetPreviousRound();

            round.Should().BeNull();
        }

        [Fact]
        public void FetchingPreviousRoundFromSecondRoundYieldsFirstRound()
        {
            BracketRound firstRound = CreateBracketRound("First round");
            BracketRound secondRound = CreateBracketRound("Second round");

            RoundBase previousRound = secondRound.GetPreviousRound();

            previousRound.Should().Be(firstRound);
        }

        [Fact]
        public void CanRegisterPlayerReferencesToFirstBracketRound()
        {
            string playerName = "Maru";

            BracketRound bracketRound = CreateBracketRound();

            PlayerReference playerReference = bracketRound.RegisterPlayerReference(playerName);

            playerReference.Id.Should().NotBeEmpty();
            playerReference.Name.Should().Be(playerName);
            playerReference.TournamentId.Should().Be(bracketRound.TournamentId);
            playerReference.Tournament.Should().Be(bracketRound.Tournament);
        }

        [Fact]
        public void CannotRegisterPlayerReferencesToBracketRoundsThatIsNotTheFirstOne()
        {
            string playerName = "Maru";
            string roundName = "Bracket round";
            int roundCount = 5;

            BracketRound firstBracketRound = CreateBracketRound();

            for (int index = 1; index < roundCount; ++index)
            {
                BracketRound bracketRound = CreateBracketRound(roundName + index.ToString());
                PlayerReference playerReference = bracketRound.RegisterPlayerReference(playerName + index.ToString());

                playerReference.Should().BeNull();
                bracketRound.PlayerReferences.Should().HaveCount(0);
            }
        }

        private BracketRound CreateBracketRound(string name = "Bracket round", int bestOf = 3)
        {
            return tournament.AddBracketRound(name, bestOf) as BracketRound;
        }
    }
}
