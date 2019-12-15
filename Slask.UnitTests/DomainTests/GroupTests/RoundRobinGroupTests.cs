using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.GroupTests
{
    public class RoundRobinGroupTests
    {


        // ALL MATCHES MUST BE ORDERED DESCENDING BY STARTDATETIME

        // Create tests for GetPlayState

        // CAN CHANGE LAST EXISTING PLAYER REF TO NULL AND IT IS REMOVED FROM GROUP
        // CAN CHANGE LAST EXISTING PLAYER REF TO ANOTHER PLAYER REF 

        // CANNOT CREATE GROUPS OF TYPES THAT DOES NOT MATCH ROUND TYPE
        [Fact]
        public void CanCreateGroup()
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
        public void CanIncreaseAmountOfMatchesBasedOnParticipatingPlayers()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part04AddedGroupToRoundRobinRound(services);

            group.AddPlayerReference("Maru").Should().NotBeNull();
            group.AddPlayerReference("Stork").Should().NotBeNull();
        }

        [Fact]
        public void CanConstructRoundRobinMatchLayout()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part04AddedGroupToRoundRobinRound(services);

            group.AddPlayerReference("Maru").Should().NotBeNull();
            RunTestsWithOnePlayer(group);

            group.AddPlayerReference("Stork").Should().NotBeNull();
            RunTestsWithTwoPlayers(group);

            group.AddPlayerReference("Taeja").Should().NotBeNull();
            RunTestsWithThreePlayers(group);

            group.AddPlayerReference("Rain").Should().NotBeNull();
            RunTestsWithFourPlayers(group);

            group.AddPlayerReference("Bomber").Should().NotBeNull();
            RunTestsWithFivePlayers(group);

            group.AddPlayerReference("FanTaSy").Should().NotBeNull();
            RunTestsWithSixPlayers(group);

            group.AddPlayerReference("Stephano").Should().NotBeNull();
            RunTestsWithSevenPlayers(group);

            group.AddPlayerReference("Thorzain").Should().NotBeNull();
            RunTestsWithEightPlayers(group);
        }

        private void RunTestsWithOnePlayer(RoundRobinGroup group)
        {
            group.Matches.Should().BeEmpty();
        }

        private void RunTestsWithTwoPlayers(RoundRobinGroup group)
        {
            group.Matches.Should().HaveCount(1);

            group.Matches[0].Player1.Name.Should().Be("Maru");
            group.Matches[0].Player2.Name.Should().Be("Stork");
        }

        private void RunTestsWithThreePlayers(RoundRobinGroup group)
        {
            group.Matches.Should().HaveCount(3);

            group.Matches[0].Player1.Name.Should().Be("Maru");
            group.Matches[0].Player2.Name.Should().Be("Taeja");

            group.Matches[1].Player1.Name.Should().Be("Taeja");
            group.Matches[1].Player2.Name.Should().Be("Stork");

            group.Matches[2].Player1.Name.Should().Be("Stork");
            group.Matches[2].Player2.Name.Should().Be("Maru");
        }

        private void RunTestsWithFourPlayers(RoundRobinGroup group)
        {
            group.Matches.Should().HaveCount(6);

            group.Matches[0].Player1.Name.Should().Be("Maru");
            group.Matches[0].Player2.Name.Should().Be("Taeja");

            group.Matches[1].Player1.Name.Should().Be("Stork");
            group.Matches[1].Player2.Name.Should().Be("Rain");

            group.Matches[2].Player1.Name.Should().Be("Maru");
            group.Matches[2].Player2.Name.Should().Be("Rain");

            group.Matches[3].Player1.Name.Should().Be("Taeja");
            group.Matches[3].Player2.Name.Should().Be("Stork");

            group.Matches[4].Player1.Name.Should().Be("Maru");
            group.Matches[4].Player2.Name.Should().Be("Stork");

            group.Matches[5].Player1.Name.Should().Be("Rain");
            group.Matches[5].Player2.Name.Should().Be("Taeja");
        }

        private void RunTestsWithFivePlayers(RoundRobinGroup group)
        {
            group.Matches.Should().HaveCount(10);

            group.Matches[0].Player1.Name.Should().Be("Maru");
            group.Matches[0].Player2.Name.Should().Be("Rain");

            group.Matches[1].Player1.Name.Should().Be("Stork");
            group.Matches[1].Player2.Name.Should().Be("Bomber");

            group.Matches[2].Player1.Name.Should().Be("Rain");
            group.Matches[2].Player2.Name.Should().Be("Bomber");

            group.Matches[3].Player1.Name.Should().Be("Maru");
            group.Matches[3].Player2.Name.Should().Be("Taeja");

            group.Matches[4].Player1.Name.Should().Be("Bomber");
            group.Matches[4].Player2.Name.Should().Be("Taeja");

            group.Matches[5].Player1.Name.Should().Be("Rain");
            group.Matches[5].Player2.Name.Should().Be("Stork");

            group.Matches[6].Player1.Name.Should().Be("Taeja");
            group.Matches[6].Player2.Name.Should().Be("Stork");

            group.Matches[7].Player1.Name.Should().Be("Bomber");
            group.Matches[7].Player2.Name.Should().Be("Maru");

            group.Matches[8].Player1.Name.Should().Be("Stork");
            group.Matches[8].Player2.Name.Should().Be("Maru");

            group.Matches[9].Player1.Name.Should().Be("Taeja");
            group.Matches[9].Player2.Name.Should().Be("Rain");
        }

        private void RunTestsWithSixPlayers(RoundRobinGroup group)
        {
            group.Matches.Should().HaveCount(15);

            group.Matches[0].Player1.Name.Should().Be("Maru");
            group.Matches[0].Player2.Name.Should().Be("Rain");

            group.Matches[1].Player1.Name.Should().Be("Stork");
            group.Matches[1].Player2.Name.Should().Be("Bomber");

            group.Matches[2].Player1.Name.Should().Be("Taeja");
            group.Matches[2].Player2.Name.Should().Be("FanTaSy");

            group.Matches[3].Player1.Name.Should().Be("Maru");
            group.Matches[3].Player2.Name.Should().Be("Bomber");

            group.Matches[4].Player1.Name.Should().Be("Rain");
            group.Matches[4].Player2.Name.Should().Be("FanTaSy");

            group.Matches[5].Player1.Name.Should().Be("Stork");
            group.Matches[5].Player2.Name.Should().Be("Taeja");

            group.Matches[6].Player1.Name.Should().Be("Maru");
            group.Matches[6].Player2.Name.Should().Be("FanTaSy");

            group.Matches[7].Player1.Name.Should().Be("Bomber");
            group.Matches[7].Player2.Name.Should().Be("Taeja");

            group.Matches[8].Player1.Name.Should().Be("Rain");
            group.Matches[8].Player2.Name.Should().Be("Stork");

            group.Matches[9].Player1.Name.Should().Be("Maru");
            group.Matches[9].Player2.Name.Should().Be("Taeja");

            group.Matches[10].Player1.Name.Should().Be("FanTaSy");
            group.Matches[10].Player2.Name.Should().Be("Stork");

            group.Matches[11].Player1.Name.Should().Be("Bomber");
            group.Matches[11].Player2.Name.Should().Be("Rain");

            group.Matches[12].Player1.Name.Should().Be("Maru");
            group.Matches[12].Player2.Name.Should().Be("Stork");

            group.Matches[13].Player1.Name.Should().Be("Taeja");
            group.Matches[13].Player2.Name.Should().Be("Rain");

            group.Matches[14].Player1.Name.Should().Be("FanTaSy");
            group.Matches[14].Player2.Name.Should().Be("Bomber");
        }

        private void RunTestsWithSevenPlayers(RoundRobinGroup group)
        {
            group.Matches.Should().HaveCount(21);

            group.Matches[0].Player1.Name.Should().Be("Maru");
            group.Matches[0].Player2.Name.Should().Be("Bomber");

            group.Matches[1].Player1.Name.Should().Be("Stork");
            group.Matches[1].Player2.Name.Should().Be("FanTaSy");

            group.Matches[2].Player1.Name.Should().Be("Taeja");
            group.Matches[2].Player2.Name.Should().Be("Stephano");

            group.Matches[3].Player1.Name.Should().Be("Bomber");
            group.Matches[3].Player2.Name.Should().Be("FanTaSy");

            group.Matches[4].Player1.Name.Should().Be("Maru");
            group.Matches[4].Player2.Name.Should().Be("Stephano");

            group.Matches[5].Player1.Name.Should().Be("Stork");
            group.Matches[5].Player2.Name.Should().Be("Rain");

            group.Matches[6].Player1.Name.Should().Be("FanTaSy");
            group.Matches[6].Player2.Name.Should().Be("Stephano");

            group.Matches[7].Player1.Name.Should().Be("Bomber");
            group.Matches[7].Player2.Name.Should().Be("Rain");

            group.Matches[8].Player1.Name.Should().Be("Maru");
            group.Matches[8].Player2.Name.Should().Be("Taeja");

            group.Matches[9].Player1.Name.Should().Be("Stephano");
            group.Matches[9].Player2.Name.Should().Be("Rain");

            group.Matches[10].Player1.Name.Should().Be("FanTaSy");
            group.Matches[10].Player2.Name.Should().Be("Taeja");

            group.Matches[11].Player1.Name.Should().Be("Bomber");
            group.Matches[11].Player2.Name.Should().Be("Stork");

            group.Matches[12].Player1.Name.Should().Be("Rain");
            group.Matches[12].Player2.Name.Should().Be("Taeja");

            group.Matches[13].Player1.Name.Should().Be("Stephano");
            group.Matches[13].Player2.Name.Should().Be("Stork");

            group.Matches[14].Player1.Name.Should().Be("FanTaSy");
            group.Matches[14].Player2.Name.Should().Be("Maru");

            group.Matches[15].Player1.Name.Should().Be("Taeja");
            group.Matches[15].Player2.Name.Should().Be("Stork");

            group.Matches[16].Player1.Name.Should().Be("Rain");
            group.Matches[16].Player2.Name.Should().Be("Maru");

            group.Matches[17].Player1.Name.Should().Be("Stephano");
            group.Matches[17].Player2.Name.Should().Be("Bomber");

            group.Matches[18].Player1.Name.Should().Be("Stork");
            group.Matches[18].Player2.Name.Should().Be("Maru");

            group.Matches[19].Player1.Name.Should().Be("Taeja");
            group.Matches[19].Player2.Name.Should().Be("Bomber");

            group.Matches[20].Player1.Name.Should().Be("Rain");
            group.Matches[20].Player2.Name.Should().Be("FanTaSy");
        }

        private void RunTestsWithEightPlayers(RoundRobinGroup group)
        {
            group.Matches.Should().HaveCount(28);

            group.Matches[0].Player1.Name.Should().Be("Maru");
            group.Matches[0].Player2.Name.Should().Be("Bomber");

            group.Matches[1].Player1.Name.Should().Be("Stork");
            group.Matches[1].Player2.Name.Should().Be("FanTaSy");

            group.Matches[2].Player1.Name.Should().Be("Taeja");
            group.Matches[2].Player2.Name.Should().Be("Stephano");

            group.Matches[3].Player1.Name.Should().Be("Rain");
            group.Matches[3].Player2.Name.Should().Be("Thorzain");

            group.Matches[4].Player1.Name.Should().Be("Maru");
            group.Matches[4].Player2.Name.Should().Be("FanTaSy");

            group.Matches[5].Player1.Name.Should().Be("Bomber");
            group.Matches[5].Player2.Name.Should().Be("Stephano");

            group.Matches[6].Player1.Name.Should().Be("Stork");
            group.Matches[6].Player2.Name.Should().Be("Thorzain");

            group.Matches[7].Player1.Name.Should().Be("Taeja");
            group.Matches[7].Player2.Name.Should().Be("Rain");

            group.Matches[8].Player1.Name.Should().Be("Maru");
            group.Matches[8].Player2.Name.Should().Be("Stephano");

            group.Matches[9].Player1.Name.Should().Be("FanTaSy");
            group.Matches[9].Player2.Name.Should().Be("Thorzain");

            group.Matches[10].Player1.Name.Should().Be("Bomber");
            group.Matches[10].Player2.Name.Should().Be("Rain");

            group.Matches[11].Player1.Name.Should().Be("Stork");
            group.Matches[11].Player2.Name.Should().Be("Taeja");

            group.Matches[12].Player1.Name.Should().Be("Maru");
            group.Matches[12].Player2.Name.Should().Be("Thorzain");

            group.Matches[13].Player1.Name.Should().Be("Stephano");
            group.Matches[13].Player2.Name.Should().Be("Rain");

            group.Matches[14].Player1.Name.Should().Be("FanTaSy");
            group.Matches[14].Player2.Name.Should().Be("Taeja");

            group.Matches[15].Player1.Name.Should().Be("Bomber");
            group.Matches[15].Player2.Name.Should().Be("Stork");

            group.Matches[16].Player1.Name.Should().Be("Maru");
            group.Matches[16].Player2.Name.Should().Be("Rain");

            group.Matches[17].Player1.Name.Should().Be("Thorzain");
            group.Matches[17].Player2.Name.Should().Be("Taeja");

            group.Matches[18].Player1.Name.Should().Be("Stephano");
            group.Matches[18].Player2.Name.Should().Be("Stork");

            group.Matches[19].Player1.Name.Should().Be("FanTaSy");
            group.Matches[19].Player2.Name.Should().Be("Bomber");

            group.Matches[20].Player1.Name.Should().Be("Maru");
            group.Matches[20].Player2.Name.Should().Be("Taeja");

            group.Matches[21].Player1.Name.Should().Be("Rain");
            group.Matches[21].Player2.Name.Should().Be("Stork");

            group.Matches[22].Player1.Name.Should().Be("Thorzain");
            group.Matches[22].Player2.Name.Should().Be("Bomber");

            group.Matches[23].Player1.Name.Should().Be("Stephano");
            group.Matches[23].Player2.Name.Should().Be("FanTaSy");

            group.Matches[24].Player1.Name.Should().Be("Maru");
            group.Matches[24].Player2.Name.Should().Be("Stork");

            group.Matches[25].Player1.Name.Should().Be("Taeja");
            group.Matches[25].Player2.Name.Should().Be("Bomber");

            group.Matches[26].Player1.Name.Should().Be("Rain");
            group.Matches[26].Player2.Name.Should().Be("FanTaSy");

            group.Matches[27].Player1.Name.Should().Be("Thorzain");
            group.Matches[27].Player2.Name.Should().Be("Stephano");
        }

        [Fact]
        public void CannotIncreaseScoreBeforeMatchHasStartedInGroup()
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
        public void CannotAddNewPlayerToGroupAfterFirstMatchHasStarted()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part08CompleteFirstMatchInRoundRobinGroup(services);

            SystemTimeMocker.Set(DateTime.Now.AddSeconds(1));

            group.AddPlayerReference("Flash").Should().BeNull();

            foreach (Domain.Match match in group.Matches)
            {
                match.FindPlayer("Flash").Should().BeNull();
            }
            group.ParticipatingPlayers.Should().NotContain(player => player.Name == "Flash");
        }

        [Fact]
        public void FetchingAdvancingPlayersInGroupOnlyReturnsWinners()
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

        private TournamentServiceContext GivenServices()
        {
            return null;
        }
    }
}
