using Slask.TestCore;
using System;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class RoundTests
    {
        [Fact]
        public void CanCreateRoundRobinRound()
        {
            TournamentServiceContext services = GivenServices();
            services.WhenCreatedGroupInBracketRoundInTournament();

            throw new NotImplementedException();
        }

        [Fact]
        public void CanCreateDualTournamentRound()
        {
            TournamentServiceContext services = GivenServices();
            services.WhenCreatedGroupInDualTournamentRoundInTournament();

            throw new NotImplementedException();
        }

        [Fact]
        public void CanCreateBracketRound()
        {
            TournamentServiceContext services = GivenServices();
            services.WhenCreatedGroupInBracketRoundInTournament();

            throw new NotImplementedException();
        }

        [Fact]
        public void OnlyWinningPlayersCanAdvanceToNextRound()
        {
            TournamentServiceContext services = GivenServices();
            services.WhenCreatedGroupInBracketRoundInTournament();

            throw new NotImplementedException();
        }

        [Fact]
        public void CanOnlyAddAdvancingPlayersWhenPreviousRoundExist()
        {
            TournamentServiceContext services = GivenServices();

            throw new NotImplementedException();
        }

        [Fact]
        public void CannotAddGroupsToRoundThatDoesNotMatchByType()
        {
            TournamentServiceContext services = GivenServices();

            throw new NotImplementedException();
        }

        [Fact]
        public void AllGroupsWithinRoundMustBeOfSameType()
        {
            TournamentServiceContext services = GivenServices();

            throw new NotImplementedException();
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
