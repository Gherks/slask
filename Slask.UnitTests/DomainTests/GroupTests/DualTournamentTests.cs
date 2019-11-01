using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using System;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class DualTournamentTests
    {
        [Fact]
        public void CanConstructDualTournamentMatchLayout()
        {
            TournamentServiceContext services = GivenServices();
            //DualTournamentGroup group = HomestoryCupSetup.Part04_AddGroupToRoundRobinRound();

            //group.AddPlayerReference("Maru");
            //group.AddPlayerReference("Stork");
            //group.AddPlayerReference("Taeja");
            //group.AddPlayerReference("Rain");

            throw new NotImplementedException();
        }

        [Fact]
        public void CanClearRoundRobinGroup()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup(services);

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
