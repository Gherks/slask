using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.RoundTests
{
    public class RoundRobinRoundTests
    {
        private readonly Tournament tournament;

        public RoundRobinRoundTests()
        {
            tournament = Tournament.Create("GSL 2019");
        }

        // Tests covering advancing amount 

        [Fact]
        public void CanCreateRound()
        {
            string name = "Round robin round";
            int bestOf = 3;
            int advancingPerGroupAmount = 2;

            RoundRobinRound roundRobinRound = RoundRobinRound.Create(name, bestOf, advancingPerGroupAmount, tournament);

            roundRobinRound.Should().NotBeNull();
            roundRobinRound.Id.Should().NotBeEmpty();
            roundRobinRound.Name.Should().Be(name);
            roundRobinRound.BestOf.Should().Be(bestOf);
            roundRobinRound.AdvancingPerGroupCount.Should().Be(advancingPerGroupAmount);
            roundRobinRound.Groups.Should().BeEmpty();
            roundRobinRound.TournamentId.Should().NotBeEmpty();
            roundRobinRound.Tournament.Should().NotBeNull();
        }

        [Fact]
        public void CannotCreateRoundWithoutName()
        {
            RoundRobinRound roundRobinRound = CreateRoundRobinRound("");

            roundRobinRound.Should().BeNull();
        }

        [Fact]
        public void CannotCreateRoundWithEvenOrZeroBestOfs()
        {
            for (int bestOf = 0; bestOf < 32; ++bestOf)
            {
                RoundRobinRound roundRobinRound = CreateRoundRobinRound("Round robin round", bestOf);
                bool bestOfIsEven = bestOf % 2 == 0;

                if (bestOfIsEven)
                {
                    roundRobinRound.Should().BeNull();
                }
                else
                {
                    roundRobinRound.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void CannotCreateRoundWithZeroOrLessAdvancingPlayers()
        {
            RoundRobinRound firstRoundRobinRound = CreateRoundRobinRound(advancingPerGroupAmount: 0);
            RoundRobinRound secondRoundRobinRound = CreateRoundRobinRound(advancingPerGroupAmount: -1);

            firstRoundRobinRound.Should().BeNull();
            secondRoundRobinRound.Should().BeNull();
        }

        [Fact]
        public void FetchingPreviousRoundFromFirstRoundYieldsNull()
        {
            RoundRobinRound roundRobinRound = CreateRoundRobinRound();

            RoundBase round = roundRobinRound.GetPreviousRound();

            round.Should().BeNull();
        }

        [Fact]
        public void FetchingPreviousRoundFromSecondRoundYieldsFirstRound()
        {
            RoundRobinRound firstRound = CreateRoundRobinRound("First round");
            RoundRobinRound secondRound = CreateRoundRobinRound("Second round");

            RoundBase previousRound = secondRound.GetPreviousRound();

            previousRound.Should().Be(firstRound);
        }

        [Fact]
        public void AddingGroupToRoundRobinRoundCreatesARoundRobinGroup()
        {
            RoundRobinRound roundRobinRound = CreateRoundRobinRound();

            roundRobinRound.AddGroup();

            RoundRobinGroup group = roundRobinRound.Groups.First() as RoundRobinGroup;

            roundRobinRound.Groups.Should().HaveCount(1);
            group.Should().BeOfType<RoundRobinGroup>();
        }

        private RoundRobinRound CreateRoundRobinRound(string name = "Round robin round", int bestOf = 3, int advancingPerGroupAmount = 2)
        {
            return tournament.AddRoundRobinRound(name, bestOf, advancingPerGroupAmount) as RoundRobinRound;
        }
    }
}
