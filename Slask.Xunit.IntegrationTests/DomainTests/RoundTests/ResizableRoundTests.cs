using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Xunit.IntegrationTests.DomainTests.RoundTests
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
            RoundRobinRound round = tournament.AddRoundRobinRound();

            round.Groups.First().Matches.Should().HaveCount(1);
            round.SetPlayersPerGroupCount(4);
            round.Groups.First().Matches.Should().HaveCount(6);
        }
    }
}
