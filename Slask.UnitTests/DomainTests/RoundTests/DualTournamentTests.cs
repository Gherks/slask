using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.RoundTests
{
    public class DualTournamentRoundTests
    {
        private readonly Tournament tournament;

        public DualTournamentRoundTests()
        {
            tournament = Tournament.Create("GSL 2019");
        }

        [Fact]
        public void CanCreateRound()
        {
            string name = "Dual tournament round";
            int bestOf = 3;

            DualTournamentRound dualTournamentRound = DualTournamentRound.Create(name, bestOf, tournament);

            dualTournamentRound.Should().NotBeNull();
            dualTournamentRound.Id.Should().NotBeEmpty();
            dualTournamentRound.Name.Should().Be(name);
            dualTournamentRound.BestOf.Should().Be(bestOf);
            dualTournamentRound.AdvancingPerGroupCount.Should().Be(2);
            dualTournamentRound.Groups.Should().BeEmpty();
            dualTournamentRound.TournamentId.Should().NotBeEmpty();
            dualTournamentRound.Tournament.Should().NotBeNull();
        }

        [Fact]
        public void CannotCreateRoundWithoutName()
        {
            DualTournamentRound dualTournamentRound = CreateDualTournamentRound("");

            dualTournamentRound.Should().BeNull();
        }

        [Fact]
        public void CannotCreateRoundWithEvenOrZeroBestOfs()
        {
            for (int bestOf = 0; bestOf < 32; ++bestOf)
            {
                DualTournamentRound dualTournamentRound = CreateDualTournamentRound("Dual tournament round", bestOf);
                bool bestOfIsEven = bestOf % 2 == 0;

                if (bestOfIsEven)
                {
                    dualTournamentRound.Should().BeNull();
                }
                else
                {
                    dualTournamentRound.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void FetchingPreviousRoundFromFirstRoundYieldsNull()
        {
            DualTournamentRound dualTournamentRound = CreateDualTournamentRound();

            RoundBase round = dualTournamentRound.GetPreviousRound();

            round.Should().BeNull();
        }

        [Fact]
        public void FetchingPreviousRoundFromSecondRoundYieldsFirstRound()
        {
            DualTournamentRound firstRound = CreateDualTournamentRound("First round");
            DualTournamentRound secondRound = CreateDualTournamentRound("Second round");

            RoundBase previousRound = secondRound.GetPreviousRound();

            previousRound.Should().Be(firstRound);
        }

        [Fact]
        public void CanRegisterPlayerReferencesToFirstDualTournamentRound()
        {
            string playerName = "Maru";

            DualTournamentRound dualTournamentRound = CreateDualTournamentRound();

            PlayerReference playerReference = dualTournamentRound.RegisterPlayerReference(playerName);

            playerReference.Id.Should().NotBeEmpty();
            playerReference.Name.Should().Be(playerName);
            playerReference.TournamentId.Should().Be(dualTournamentRound.TournamentId);
            playerReference.Tournament.Should().Be(dualTournamentRound.Tournament);
        }

        [Fact]
        public void CannotRegisterPlayerReferencesToDualTournamentRoundsThatIsNotTheFirstOne()
        {
            string playerName = "Maru";
            string roundName = "Dual tournament round";
            int roundCount = 5;

            DualTournamentRound firstDualTournamentRound = CreateDualTournamentRound();

            for (int index = 1; index < roundCount; ++index)
            {
                DualTournamentRound dualTournamentRound = CreateDualTournamentRound(roundName + index.ToString());
                PlayerReference playerReference = dualTournamentRound.RegisterPlayerReference(playerName + index.ToString());

                playerReference.Should().BeNull();
                dualTournamentRound.PlayerReferences.Should().HaveCount(0);
            }
        }

        private DualTournamentRound CreateDualTournamentRound(string name = "Dual tournament round", int bestOf = 3)
        {
            return tournament.AddDualTournamentRound(name, bestOf) as DualTournamentRound;
        }
    }
}
