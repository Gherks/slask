using FluentAssertions;
using Moq;
using Slask.Common;
using Slask.Domain;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class GroupTests : IDisposable
    {
        public void Dispose()
        {
            SystemTimeMocker.Reset();
        }

        [Fact]
        public void CanCreateRoundRobinGroup()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part04AddedGroupToRoundRobinRound(services);

            group.Should().NotBeNull();
            group.Id.Should().NotBeEmpty();
            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().BeEmpty();
            group.RoundId.Should().NotBeEmpty();
            group.Round.Should().NotBeNull();
        }

        [Fact]
        public void CanCreateDualTournamentGroup()
        {
            TournamentServiceContext services = GivenServices();
            List<DualTournamentGroup> groups = BHAOpenSetup.Part04AddGroupsToDualTournamentRound(services);

            foreach (DualTournamentGroup group in groups)
            {
                group.Id.Should().NotBeEmpty();
                group.ParticipatingPlayers.Should().BeEmpty();
                group.Matches.Should().HaveCount(5);
                group.RoundId.Should().NotBeEmpty();
                group.Round.Should().NotBeNull();
            }
        }

        [Fact]
        public void CanCreateBracketGroup()
        {
            TournamentServiceContext services = GivenServices();
            BracketGroup group = HomestoryCupSetup.Part11AddGroupToBracketRound(services);

            group.Should().NotBeNull();
            group.Id.Should().NotBeEmpty();
            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().BeEmpty();
            group.RoundId.Should().NotBeEmpty();
            group.Round.Should().NotBeNull();
        }

        [Fact]
        public void StartDateTimeInMatchesInRoundRobinGroupIsSpacedWithOneHourUponCreation()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void StartDateTimeInMatchesInDualTournamentGroupIsSpacedWithOneHourUponCreation()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void StartDateTimeInMatchesInBracketGroupIsSpacedWithOneHourUponCreation()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void PlayerReferenceIsAddedToTournamentWhenNewPlayerIsAddedToGroup()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CannotAddNewPlayerToGroupAfterFirstMatchHasStarted()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part08CompleteFirstMatchInRoundRobinGroup(services);

            SystemTimeMocker.Set(DateTime.Now.AddSeconds(1));

            group.AddPlayerReference("Flash").Should().BeFalse();

            foreach (Domain.Match match in group.Matches)
            {
                match.FindPlayer("Flash").Should().BeNull();
            }
            group.ParticipatingPlayers.Should().NotContain(player => player.Name == "Flash");
        }

        [Fact]
        public void CannotIncreaseScoreBeforeMatchHasStartedInRoundRobinGroup()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part07BetsPlacedOnMatchesInRoundRobinGroup(services);
            Domain.Match match = group.Matches.First();

            match.Player1.IncreaseScore(1);
            match.Player2.IncreaseScore(1);

            match.Player1.Score.Should().Be(0);
            match.Player2.Score.Should().Be(0);
        }

        [Fact]
        public void CannotIncreaseScoreBeforeMatchHasStartedInDualTournamentGroup()
        {
            TournamentServiceContext services = GivenServices();
            DualTournamentGroup group = BHAOpenSetup.Part07BetsPlacedOnMatchesInDualTournamentGroups(services).First();
            Domain.Match match = group.Matches.First();

            match.Player1.IncreaseScore(1);
            match.Player2.IncreaseScore(1);

            match.Player1.Score.Should().Be(0);
            match.Player2.Score.Should().Be(0);
        }

        [Fact]
        public void CannotIncreaseScoreBeforeMatchHasStartedInBracketGroup()
        {
            TournamentServiceContext services = GivenServices();
            BracketGroup group = HomestoryCupSetup.Part14BetsPlacedOnMatchesInBracketGroup(services);
            Domain.Match match = group.Matches.First();

            match.Player1.IncreaseScore(1);
            match.Player2.IncreaseScore(1);

            match.Player1.Score.Should().Be(0);
            match.Player2.Score.Should().Be(0);
        }

        [Fact]
        public void CanClearRoundRobinGroup()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05AddedPlayersToRoundRobinGroup(services);

            group.Clear();

            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().BeEmpty();
        }

        [Fact]
        public void CanClearDualTournamentGroup()
        {
            TournamentServiceContext services = GivenServices();
            DualTournamentGroup group = BHAOpenSetup.Part05AddedPlayersToDualTournamentGroups(services).First();

            group.Clear();

            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().HaveCount(5);

            for(int index = 0; index < group.Matches.Count; ++index)
            {
                group.Matches[index].Player1.PlayerReference.Should().BeNull();
                group.Matches[index].Player2.PlayerReference.Should().BeNull();

                group.Matches[index].StartDateTime.Should().BeCloseTo(DateTime.Now.AddYears(1).AddHours(index));
            }
        }

        [Fact]
        public void CanClearBracketGroup()
        {
            TournamentServiceContext services = GivenServices();
            BracketGroup group = HomestoryCupSetup.Part12AddWinningPlayersToBracketGroup(services);

            group.Clear();

            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().BeEmpty();
        }

        // ALL MATCHES MUST BE ORDERED DESCENDING BY STARTDATETIME

        // Create tests for GetPlayState

        // CAN CHANGE LAST EXISTING PLAYER REF TO NULL AND IT IS REMOVED FROM GROUP
        // CAN CHANGE LAST EXISTING PLAYER REF TO ANOTHER PLAYER REF 

        // CANNOT CREATE GROUPS OF TYPES THAT DOES NOT MATCH ROUND TYPE

        [Fact]
        public void FetchingAdvancingPlayersInRoundRobinGroupReturnsAtLeastNumberOfPlayersSetByParentRound()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part09CompleteAllMatchesInRoundRobinGroup(services);

            List<PlayerReference> topPlayers = group.GetAdvancingPlayers();

            topPlayers.Should().HaveCount(group.Round.AdvancingPerGroupAmount);
            topPlayers[0].Name.Should().Be("");
            topPlayers[1].Name.Should().Be("");
            topPlayers[2].Name.Should().Be("");
        }

        [Fact]
        public void FetchingAdvancingPlayersInRoundRobinGroupReturnsAllPlayersIfNumberOfAdvancingPlayersIsGreaterThanPlayersParticipating()
        {
            TournamentServiceContext services = GivenServices();
            throw new NotImplementedException();
        }

        [Fact]
        public void FetchingAdvancingPlayersInDualTournamentGroupOnlyReturnsTopTwoPlayers()
        {
            TournamentServiceContext services = GivenServices();
            DualTournamentGroup group = BHAOpenSetup.Part09CompleteAllMatchesInDualTournamentGroups(services).First();

            List<PlayerReference> topPlayers = group.GetAdvancingPlayers();

            topPlayers.Should().HaveCount(2);
            topPlayers[0].Name.Should().Be("");
            topPlayers[1].Name.Should().Be("");
        }

        [Fact]
        public void FetchingAdvancingPlayersInBracketGroupOnlyReturnsBracketWinner()
        {
            TournamentServiceContext services = GivenServices();
            BracketGroup group = HomestoryCupSetup.Part16CompleteAllMatchesInBracketGroup(services);
            //services.SetMockedTime();// DateTimeMockHelper.SetTime(group.Matches.Last().StartDateTime.AddSeconds(1));

            List<PlayerReference> topPlayers = group.GetAdvancingPlayers();

            topPlayers.Should().HaveCount(1);
            topPlayers[0].Name.Should().Be("");
        }

        [Fact]
        public void CannotFetchAdvancingPlayersBeforeGroupIsPlayedOut()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part08CompleteFirstMatchInRoundRobinGroup(services);

            List<PlayerReference> topPlayers = group.GetAdvancingPlayers();

            topPlayers.Should().BeEmpty();
        }

        [Fact]
        public void PlayerIsRemovedFromParticipantListWhenNotAssignedToASingleMatch()
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
