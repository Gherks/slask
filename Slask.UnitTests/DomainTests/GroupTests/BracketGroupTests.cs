using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using System;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class BracketGroupTests
    {
        [Fact]
        public void CanConstructBracketMatchLayout()
        {
            TournamentServiceContext services = GivenServices();
            BracketGroup group = HomestoryCupSetup.Part12AddWinningPlayersToBracketGroup(services);

            // Must dig up the test results...

            throw new NotImplementedException();
        }

        [Fact]
        public void CanClearRoundRobinGroup()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05AddedPlayersToRoundRobinGroup(services);

            group.Clear();

            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().HaveCount(5);
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
