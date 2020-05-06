using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.RoundTests
{
    public class ResizableRoundTests
    {
        private readonly Tournament tournament;

        public ResizableRoundTests()
        {
            tournament = Tournament.Create("GSL 2019");
        }

        [Fact]
        public void CanChangeGroupSize()
        {
            RoundRobinRound roundRobinRound = RoundRobinRound.Create(tournament);

            roundRobinRound.Groups.First().Matches.Should().HaveCount(1);
            roundRobinRound.SetPlayersPerGroupCount(4);
            roundRobinRound.Groups.First().Matches.Should().HaveCount(6);
        }
    }
}
