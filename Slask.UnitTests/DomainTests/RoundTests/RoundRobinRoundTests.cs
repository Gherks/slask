using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
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

        [Fact]
        public void CanCreateRound()
        {
            string name = "Round robin round";
            int bestOf = 3;
            int advancingPerGroupCount = 1;

            RoundRobinRound roundRobinRound = CreateRoundRobinRound(name, bestOf, advancingPerGroupCount);

            roundRobinRound.Should().NotBeNull();
            roundRobinRound.Id.Should().NotBeEmpty();
            roundRobinRound.Name.Should().Be(name);
            roundRobinRound.PlayersPerGroupCount.Should().Be(2);
            roundRobinRound.BestOf.Should().Be(bestOf);
            roundRobinRound.AdvancingPerGroupCount.Should().Be(advancingPerGroupCount);
            roundRobinRound.Groups.Should().HaveCount(1);
            roundRobinRound.TournamentId.Should().Be(tournament.Id);
            roundRobinRound.Tournament.Should().Be(tournament);
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
            RoundRobinRound firstRoundRobinRound = CreateRoundRobinRound(advancingPerGroupCount: 0);
            RoundRobinRound secondRoundRobinRound = CreateRoundRobinRound(advancingPerGroupCount: -1);

            firstRoundRobinRound.Should().BeNull();
            secondRoundRobinRound.Should().BeNull();
        }

        [Fact]
        public void CanChangeGroupSize()
        {
            RoundRobinRound roundRobinRound = CreateRoundRobinRound();

            roundRobinRound.Groups.First().Matches.Should().HaveCount(1);
            roundRobinRound.SetPlayersPerGroupCount(4);
            roundRobinRound.Groups.First().Matches.Should().HaveCount(6);
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
        public void CanRegisterPlayerReferencesToFirstRoundRobinRound()
        {
            string playerName = "Maru";

            RoundRobinRound roundRobinRound = CreateRoundRobinRound();

            PlayerReference playerReference =  roundRobinRound.RegisterPlayerReference(playerName);

            playerReference.Id.Should().NotBeEmpty();
            playerReference.Name.Should().Be(playerName);
            playerReference.TournamentId.Should().Be(roundRobinRound.TournamentId);
            playerReference.Tournament.Should().Be(roundRobinRound.Tournament);
        }

        [Fact]
        public void CannotRegisterPlayerReferencesToRoundRobinRoundsThatIsNotTheFirstOne()
        {
            string playerName = "Maru";
            string roundName = "Round robin round";
            int roundCount = 5;

            RoundRobinRound firstRoundRobinRound = CreateRoundRobinRound();

            for(int index = 1; index < roundCount; ++index)
            {
                RoundRobinRound roundRobinRound = CreateRoundRobinRound(roundName + index.ToString());
                PlayerReference playerReference = roundRobinRound.RegisterPlayerReference(playerName + index.ToString());

                playerReference.Should().BeNull();
                roundRobinRound.PlayerReferences.Should().HaveCount(0);
            }
        }

        private RoundRobinRound CreateRoundRobinRound(string name = "Round robin round", int bestOf = 3, int advancingPerGroupCount = 2)
        {
            return tournament.AddRoundRobinRound(name, bestOf, advancingPerGroupCount) as RoundRobinRound;
        }
    }
}
