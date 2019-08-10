using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class RoundRobinGroupTests
    {
        [Fact]
        public void CanIncreaseAmountOfMatchesBasedOnParticipatingPlayers()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_04_AddGroupToRoundRobinRound();

            group.AddPlayerReference("Maru");
            group.AddPlayerReference("Stork");
        }

        [Fact]
        public void CanClearRoundRobinGroup()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_05_AddedPlayersToRoundRobinGroup();

            group.Clear();

            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().BeEmpty();
            group.IsReady.Should().BeFalse();
        }

        [Fact]
        public void CanConstructRoundRobinMatchLayout()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = services.HomestoryCup_04_AddGroupToRoundRobinRound();

            group.AddPlayerReference("Maru");
            RunTestsWithOnePlayer(group);

            group.AddPlayerReference("Stork");
            RunTestsWithTwoPlayers(group);

            group.AddPlayerReference("Taeja");
            RunTestsWithThreePlayers(group);

            group.AddPlayerReference("Rain");
            RunTestsWithFourPlayers(group);

            group.AddPlayerReference("Bomber");
            RunTestsWithFivePlayers(group);

            group.AddPlayerReference("FanTaSy");
            RunTestsWithSixPlayers(group);

            group.AddPlayerReference("Stephano");
            RunTestsWithSevenPlayers(group);

            group.AddPlayerReference("Thorzain");
            RunTestsWithEightPlayers(group);
        }

        private void RunTestsWithOnePlayer(RoundRobinGroup group)
        {
            group.Matches.Should().BeEmpty();
        }

        private void RunTestsWithTwoPlayers(RoundRobinGroup group)
        {
            group.Matches.Count.Should().Be(1);

            group.Matches[0].Player1.Name.Should().Be("Maru");
            group.Matches[0].Player2.Name.Should().Be("Stork");
        }

        private void RunTestsWithThreePlayers(RoundRobinGroup group)
        {
            group.Matches.Count.Should().Be(3);

            group.Matches[0].Player1.Name.Should().Be("Maru");
            group.Matches[0].Player2.Name.Should().Be("Taeja");

            group.Matches[1].Player1.Name.Should().Be("Taeja");
            group.Matches[1].Player2.Name.Should().Be("Stork");

            group.Matches[2].Player1.Name.Should().Be("Stork");
            group.Matches[2].Player2.Name.Should().Be("Maru");
        }

        private void RunTestsWithFourPlayers(RoundRobinGroup group)
        {
            group.Matches.Count.Should().Be(6);

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
            group.Matches.Count.Should().Be(10);

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
            group.Matches.Count.Should().Be(15);

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
            group.Matches.Count.Should().Be(21);

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
            group.Matches.Count.Should().Be(28);

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

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
