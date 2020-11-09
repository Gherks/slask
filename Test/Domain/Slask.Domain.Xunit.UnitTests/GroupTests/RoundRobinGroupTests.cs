using FluentAssertions;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Rounds.RoundTypes;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Domain.Xunit.UnitTests.GroupTests
{
    public class RoundRobinGroupTests
    {
        private readonly Tournament tournament;
        private readonly RoundRobinRound roundRobinRound;

        public RoundRobinGroupTests()
        {
            tournament = Tournament.Create("GSL 2019");
            roundRobinRound = tournament.AddRoundRobinRound() as RoundRobinRound;
        }

        [Fact]
        public void CanCreateGroup()
        {
            RoundRobinGroup roundRobinGroup = RoundRobinGroup.Create(roundRobinRound);

            roundRobinGroup.Should().NotBeNull();
            roundRobinGroup.Id.Should().NotBeEmpty();
            roundRobinGroup.Matches.Should().BeEmpty();
            roundRobinGroup.RoundId.Should().Be(roundRobinRound.Id);
            roundRobinGroup.Round.Should().Be(roundRobinRound);
        }

        [Fact]
        public void MatchIsCreatedWhenTwoPlayerReferencesAreAddedToGroup()
        {
            string firstPlayerName = "Maru";
            string secondPlayerName = "Stork";

            RoundRobinGroup roundRobinGroup = RegisterPlayers(new List<string>() { firstPlayerName, secondPlayerName });

            roundRobinGroup.Matches.Should().HaveCount(1);
            roundRobinGroup.Matches.FirstOrDefault(match => match.GetPlayer1Name() == firstPlayerName).Should().NotBeNull();
            roundRobinGroup.Matches.FirstOrDefault(match => match.GetPlayer2Name() == secondPlayerName).Should().NotBeNull();
        }

        [Fact]
        public void ConstructsAppropriateCountOfMatchesDependingOnGroupSize()
        {
            int groupSizeCounter = 0;

            roundRobinRound.SetPlayersPerGroupCount(++groupSizeCounter);
            roundRobinRound.Groups.First().Matches.Should().HaveCount(1);

            roundRobinRound.SetPlayersPerGroupCount(++groupSizeCounter);
            roundRobinRound.Groups.First().Matches.Should().HaveCount(1);

            roundRobinRound.SetPlayersPerGroupCount(++groupSizeCounter);
            roundRobinRound.Groups.First().Matches.Should().HaveCount(3);

            roundRobinRound.SetPlayersPerGroupCount(++groupSizeCounter);
            roundRobinRound.Groups.First().Matches.Should().HaveCount(6);

            roundRobinRound.SetPlayersPerGroupCount(++groupSizeCounter);
            roundRobinRound.Groups.First().Matches.Should().HaveCount(10);

            roundRobinRound.SetPlayersPerGroupCount(++groupSizeCounter);
            roundRobinRound.Groups.First().Matches.Should().HaveCount(15);

            roundRobinRound.SetPlayersPerGroupCount(++groupSizeCounter);
            roundRobinRound.Groups.First().Matches.Should().HaveCount(21);

            roundRobinRound.SetPlayersPerGroupCount(++groupSizeCounter);
            roundRobinRound.Groups.First().Matches.Should().HaveCount(28);
        }

        [Fact]
        public void CanConstructRoundRobinMatchLayout()
        {
            List<string> playerNames = new List<string> { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };

            roundRobinRound.SetPlayersPerGroupCount(playerNames.Count);

            tournament.RegisterPlayerReference(playerNames[0]).Should().NotBeNull();
            RunTestsWithOnePlayer(roundRobinRound.Groups.First());

            tournament.RegisterPlayerReference(playerNames[1]).Should().NotBeNull();
            RunTestsWithTwoPlayers(roundRobinRound.Groups.First());

            tournament.RegisterPlayerReference(playerNames[2]).Should().NotBeNull();
            RunTestsWithThreePlayers(roundRobinRound.Groups.First());

            tournament.RegisterPlayerReference(playerNames[3]).Should().NotBeNull();
            RunTestsWithFourPlayers(roundRobinRound.Groups.First());

            tournament.RegisterPlayerReference(playerNames[4]).Should().NotBeNull();
            RunTestsWithFivePlayers(roundRobinRound.Groups.First());

            tournament.RegisterPlayerReference(playerNames[5]).Should().NotBeNull();
            RunTestsWithSixPlayers(roundRobinRound.Groups.First());

            tournament.RegisterPlayerReference(playerNames[6]).Should().NotBeNull();
            RunTestsWithSevenPlayers(roundRobinRound.Groups.First());

            tournament.RegisterPlayerReference(playerNames[7]).Should().NotBeNull();
            RunTestsWithEightPlayers(roundRobinRound.Groups.First());
        }

        private void RunTestsWithOnePlayer(GroupBase group)
        {
            group.Matches[0].GetPlayer1Name().Should().Be("Maru");
            group.Matches[0].PlayerReference2Id.Should().BeEmpty();
        }

        private void RunTestsWithTwoPlayers(GroupBase group)
        {
            group.Matches[0].GetPlayer1Name().Should().Be("Maru");
            group.Matches[0].GetPlayer2Name().Should().Be("Stork");
        }

        private void RunTestsWithThreePlayers(GroupBase group)
        {
            group.Matches[0].GetPlayer1Name().Should().Be("Maru");
            group.Matches[0].GetPlayer2Name().Should().Be("Taeja");

            group.Matches[1].GetPlayer1Name().Should().Be("Taeja");
            group.Matches[1].GetPlayer2Name().Should().Be("Stork");

            group.Matches[2].GetPlayer1Name().Should().Be("Stork");
            group.Matches[2].GetPlayer2Name().Should().Be("Maru");
        }

        private void RunTestsWithFourPlayers(GroupBase group)
        {
            group.Matches[0].GetPlayer1Name().Should().Be("Maru");
            group.Matches[0].GetPlayer2Name().Should().Be("Taeja");

            group.Matches[1].GetPlayer1Name().Should().Be("Stork");
            group.Matches[1].GetPlayer2Name().Should().Be("Rain");

            group.Matches[2].GetPlayer1Name().Should().Be("Maru");
            group.Matches[2].GetPlayer2Name().Should().Be("Rain");

            group.Matches[3].GetPlayer1Name().Should().Be("Taeja");
            group.Matches[3].GetPlayer2Name().Should().Be("Stork");

            group.Matches[4].GetPlayer1Name().Should().Be("Maru");
            group.Matches[4].GetPlayer2Name().Should().Be("Stork");

            group.Matches[5].GetPlayer1Name().Should().Be("Rain");
            group.Matches[5].GetPlayer2Name().Should().Be("Taeja");
        }

        private void RunTestsWithFivePlayers(GroupBase group)
        {
            group.Matches[0].GetPlayer1Name().Should().Be("Maru");
            group.Matches[0].GetPlayer2Name().Should().Be("Rain");

            group.Matches[1].GetPlayer1Name().Should().Be("Stork");
            group.Matches[1].GetPlayer2Name().Should().Be("Bomber");

            group.Matches[2].GetPlayer1Name().Should().Be("Rain");
            group.Matches[2].GetPlayer2Name().Should().Be("Bomber");

            group.Matches[3].GetPlayer1Name().Should().Be("Maru");
            group.Matches[3].GetPlayer2Name().Should().Be("Taeja");

            group.Matches[4].GetPlayer1Name().Should().Be("Bomber");
            group.Matches[4].GetPlayer2Name().Should().Be("Taeja");

            group.Matches[5].GetPlayer1Name().Should().Be("Rain");
            group.Matches[5].GetPlayer2Name().Should().Be("Stork");

            group.Matches[6].GetPlayer1Name().Should().Be("Taeja");
            group.Matches[6].GetPlayer2Name().Should().Be("Stork");

            group.Matches[7].GetPlayer1Name().Should().Be("Bomber");
            group.Matches[7].GetPlayer2Name().Should().Be("Maru");

            group.Matches[8].GetPlayer1Name().Should().Be("Stork");
            group.Matches[8].GetPlayer2Name().Should().Be("Maru");

            group.Matches[9].GetPlayer1Name().Should().Be("Taeja");
            group.Matches[9].GetPlayer2Name().Should().Be("Rain");
        }

        private void RunTestsWithSixPlayers(GroupBase group)
        {
            group.Matches[0].GetPlayer1Name().Should().Be("Maru");
            group.Matches[0].GetPlayer2Name().Should().Be("Rain");

            group.Matches[1].GetPlayer1Name().Should().Be("Stork");
            group.Matches[1].GetPlayer2Name().Should().Be("Bomber");

            group.Matches[2].GetPlayer1Name().Should().Be("Taeja");
            group.Matches[2].GetPlayer2Name().Should().Be("FanTaSy");

            group.Matches[3].GetPlayer1Name().Should().Be("Maru");
            group.Matches[3].GetPlayer2Name().Should().Be("Bomber");

            group.Matches[4].GetPlayer1Name().Should().Be("Rain");
            group.Matches[4].GetPlayer2Name().Should().Be("FanTaSy");

            group.Matches[5].GetPlayer1Name().Should().Be("Stork");
            group.Matches[5].GetPlayer2Name().Should().Be("Taeja");

            group.Matches[6].GetPlayer1Name().Should().Be("Maru");
            group.Matches[6].GetPlayer2Name().Should().Be("FanTaSy");

            group.Matches[7].GetPlayer1Name().Should().Be("Bomber");
            group.Matches[7].GetPlayer2Name().Should().Be("Taeja");

            group.Matches[8].GetPlayer1Name().Should().Be("Rain");
            group.Matches[8].GetPlayer2Name().Should().Be("Stork");

            group.Matches[9].GetPlayer1Name().Should().Be("Maru");
            group.Matches[9].GetPlayer2Name().Should().Be("Taeja");

            group.Matches[10].GetPlayer1Name().Should().Be("FanTaSy");
            group.Matches[10].GetPlayer2Name().Should().Be("Stork");

            group.Matches[11].GetPlayer1Name().Should().Be("Bomber");
            group.Matches[11].GetPlayer2Name().Should().Be("Rain");

            group.Matches[12].GetPlayer1Name().Should().Be("Maru");
            group.Matches[12].GetPlayer2Name().Should().Be("Stork");

            group.Matches[13].GetPlayer1Name().Should().Be("Taeja");
            group.Matches[13].GetPlayer2Name().Should().Be("Rain");

            group.Matches[14].GetPlayer1Name().Should().Be("FanTaSy");
            group.Matches[14].GetPlayer2Name().Should().Be("Bomber");
        }

        private void RunTestsWithSevenPlayers(GroupBase group)
        {
            group.Matches[0].GetPlayer1Name().Should().Be("Maru");
            group.Matches[0].GetPlayer2Name().Should().Be("Bomber");

            group.Matches[1].GetPlayer1Name().Should().Be("Stork");
            group.Matches[1].GetPlayer2Name().Should().Be("FanTaSy");

            group.Matches[2].GetPlayer1Name().Should().Be("Taeja");
            group.Matches[2].GetPlayer2Name().Should().Be("Stephano");

            group.Matches[3].GetPlayer1Name().Should().Be("Bomber");
            group.Matches[3].GetPlayer2Name().Should().Be("FanTaSy");

            group.Matches[4].GetPlayer1Name().Should().Be("Maru");
            group.Matches[4].GetPlayer2Name().Should().Be("Stephano");

            group.Matches[5].GetPlayer1Name().Should().Be("Stork");
            group.Matches[5].GetPlayer2Name().Should().Be("Rain");

            group.Matches[6].GetPlayer1Name().Should().Be("FanTaSy");
            group.Matches[6].GetPlayer2Name().Should().Be("Stephano");

            group.Matches[7].GetPlayer1Name().Should().Be("Bomber");
            group.Matches[7].GetPlayer2Name().Should().Be("Rain");

            group.Matches[8].GetPlayer1Name().Should().Be("Maru");
            group.Matches[8].GetPlayer2Name().Should().Be("Taeja");

            group.Matches[9].GetPlayer1Name().Should().Be("Stephano");
            group.Matches[9].GetPlayer2Name().Should().Be("Rain");

            group.Matches[10].GetPlayer1Name().Should().Be("FanTaSy");
            group.Matches[10].GetPlayer2Name().Should().Be("Taeja");

            group.Matches[11].GetPlayer1Name().Should().Be("Bomber");
            group.Matches[11].GetPlayer2Name().Should().Be("Stork");

            group.Matches[12].GetPlayer1Name().Should().Be("Rain");
            group.Matches[12].GetPlayer2Name().Should().Be("Taeja");

            group.Matches[13].GetPlayer1Name().Should().Be("Stephano");
            group.Matches[13].GetPlayer2Name().Should().Be("Stork");

            group.Matches[14].GetPlayer1Name().Should().Be("FanTaSy");
            group.Matches[14].GetPlayer2Name().Should().Be("Maru");

            group.Matches[15].GetPlayer1Name().Should().Be("Taeja");
            group.Matches[15].GetPlayer2Name().Should().Be("Stork");

            group.Matches[16].GetPlayer1Name().Should().Be("Rain");
            group.Matches[16].GetPlayer2Name().Should().Be("Maru");

            group.Matches[17].GetPlayer1Name().Should().Be("Stephano");
            group.Matches[17].GetPlayer2Name().Should().Be("Bomber");

            group.Matches[18].GetPlayer1Name().Should().Be("Stork");
            group.Matches[18].GetPlayer2Name().Should().Be("Maru");

            group.Matches[19].GetPlayer1Name().Should().Be("Taeja");
            group.Matches[19].GetPlayer2Name().Should().Be("Bomber");

            group.Matches[20].GetPlayer1Name().Should().Be("Rain");
            group.Matches[20].GetPlayer2Name().Should().Be("FanTaSy");
        }

        private void RunTestsWithEightPlayers(GroupBase group)
        {
            group.Matches[0].GetPlayer1Name().Should().Be("Maru");
            group.Matches[0].GetPlayer2Name().Should().Be("Bomber");

            group.Matches[1].GetPlayer1Name().Should().Be("Stork");
            group.Matches[1].GetPlayer2Name().Should().Be("FanTaSy");

            group.Matches[2].GetPlayer1Name().Should().Be("Taeja");
            group.Matches[2].GetPlayer2Name().Should().Be("Stephano");

            group.Matches[3].GetPlayer1Name().Should().Be("Rain");
            group.Matches[3].GetPlayer2Name().Should().Be("Thorzain");

            group.Matches[4].GetPlayer1Name().Should().Be("Maru");
            group.Matches[4].GetPlayer2Name().Should().Be("FanTaSy");

            group.Matches[5].GetPlayer1Name().Should().Be("Bomber");
            group.Matches[5].GetPlayer2Name().Should().Be("Stephano");

            group.Matches[6].GetPlayer1Name().Should().Be("Stork");
            group.Matches[6].GetPlayer2Name().Should().Be("Thorzain");

            group.Matches[7].GetPlayer1Name().Should().Be("Taeja");
            group.Matches[7].GetPlayer2Name().Should().Be("Rain");

            group.Matches[8].GetPlayer1Name().Should().Be("Maru");
            group.Matches[8].GetPlayer2Name().Should().Be("Stephano");

            group.Matches[9].GetPlayer1Name().Should().Be("FanTaSy");
            group.Matches[9].GetPlayer2Name().Should().Be("Thorzain");

            group.Matches[10].GetPlayer1Name().Should().Be("Bomber");
            group.Matches[10].GetPlayer2Name().Should().Be("Rain");

            group.Matches[11].GetPlayer1Name().Should().Be("Stork");
            group.Matches[11].GetPlayer2Name().Should().Be("Taeja");

            group.Matches[12].GetPlayer1Name().Should().Be("Maru");
            group.Matches[12].GetPlayer2Name().Should().Be("Thorzain");

            group.Matches[13].GetPlayer1Name().Should().Be("Stephano");
            group.Matches[13].GetPlayer2Name().Should().Be("Rain");

            group.Matches[14].GetPlayer1Name().Should().Be("FanTaSy");
            group.Matches[14].GetPlayer2Name().Should().Be("Taeja");

            group.Matches[15].GetPlayer1Name().Should().Be("Bomber");
            group.Matches[15].GetPlayer2Name().Should().Be("Stork");

            group.Matches[16].GetPlayer1Name().Should().Be("Maru");
            group.Matches[16].GetPlayer2Name().Should().Be("Rain");

            group.Matches[17].GetPlayer1Name().Should().Be("Thorzain");
            group.Matches[17].GetPlayer2Name().Should().Be("Taeja");

            group.Matches[18].GetPlayer1Name().Should().Be("Stephano");
            group.Matches[18].GetPlayer2Name().Should().Be("Stork");

            group.Matches[19].GetPlayer1Name().Should().Be("FanTaSy");
            group.Matches[19].GetPlayer2Name().Should().Be("Bomber");

            group.Matches[20].GetPlayer1Name().Should().Be("Maru");
            group.Matches[20].GetPlayer2Name().Should().Be("Taeja");

            group.Matches[21].GetPlayer1Name().Should().Be("Rain");
            group.Matches[21].GetPlayer2Name().Should().Be("Stork");

            group.Matches[22].GetPlayer1Name().Should().Be("Thorzain");
            group.Matches[22].GetPlayer2Name().Should().Be("Bomber");

            group.Matches[23].GetPlayer1Name().Should().Be("Stephano");
            group.Matches[23].GetPlayer2Name().Should().Be("FanTaSy");

            group.Matches[24].GetPlayer1Name().Should().Be("Maru");
            group.Matches[24].GetPlayer2Name().Should().Be("Stork");

            group.Matches[25].GetPlayer1Name().Should().Be("Taeja");
            group.Matches[25].GetPlayer2Name().Should().Be("Bomber");

            group.Matches[26].GetPlayer1Name().Should().Be("Rain");
            group.Matches[26].GetPlayer2Name().Should().Be("FanTaSy");

            group.Matches[27].GetPlayer1Name().Should().Be("Thorzain");
            group.Matches[27].GetPlayer2Name().Should().Be("Stephano");
        }

        private RoundRobinGroup RegisterPlayers(List<string> playerNames)
        {
            foreach (string playerName in playerNames)
            {
                tournament.RegisterPlayerReference(playerName);
            }

            return roundRobinRound.Groups.First() as RoundRobinGroup;
        }
    }
}
