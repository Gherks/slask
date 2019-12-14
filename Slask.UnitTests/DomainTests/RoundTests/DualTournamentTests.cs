using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.TestCore;
using System;
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
        public void CanCreateDualTournamentRound()
        {
            string name = "Dual tournament round";
            int bestOf = 3;

            DualTournamentRound dualTournamentRound = DualTournamentRound.Create(name, bestOf, tournament);

            dualTournamentRound.Should().NotBeNull();
            dualTournamentRound.Id.Should().NotBeEmpty();
            dualTournamentRound.Name.Should().Be(name);
            dualTournamentRound.BestOf.Should().Be(bestOf);
            dualTournamentRound.AdvancingPerGroupAmount.Should().Be(2);
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
        public void GetsBackNullWhenFetchingPreviousRoundWithFirstRound()
        {
            DualTournamentRound dualTournamentRound = CreateDualTournamentRound();

            RoundBase round = dualTournamentRound.GetPreviousRound();

            round.Should().BeNull();
        }

        [Fact]
        public void AddingGroupToDualTournamentRoundCreatesADualTournamentGroup()
        {
            DualTournamentRound dualTournamentRound = CreateDualTournamentRound();

            dualTournamentRound.AddGroup();

            DualTournamentGroup group = dualTournamentRound.Groups.First() as DualTournamentGroup;

            dualTournamentRound.Groups.Should().HaveCount(1);
            group.Should().BeOfType<DualTournamentGroup>();
        }

        private DualTournamentRound CreateDualTournamentRound(string name = "Dual tournament round", int bestOf = 3)
        {
            return DualTournamentRound.Create(name, bestOf, tournament);
        }
    }
}
