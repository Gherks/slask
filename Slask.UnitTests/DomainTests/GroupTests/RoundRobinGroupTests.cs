using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.GroupTests
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
            roundRobinGroup.PlayerReferences.Should().BeEmpty();
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
            roundRobinGroup.Matches.FirstOrDefault(match => match.Player1.Name == firstPlayerName).Should().NotBeNull();
            roundRobinGroup.Matches.FirstOrDefault(match => match.Player2.Name == secondPlayerName).Should().NotBeNull();
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

            roundRobinRound.RegisterPlayerReference(playerNames[0]).Should().NotBeNull();
            RunTestsWithOnePlayer(roundRobinRound.Groups.First());

            roundRobinRound.RegisterPlayerReference(playerNames[1]).Should().NotBeNull();
            RunTestsWithTwoPlayers(roundRobinRound.Groups.First());

            roundRobinRound.RegisterPlayerReference(playerNames[2]).Should().NotBeNull();
            RunTestsWithThreePlayers(roundRobinRound.Groups.First());

            roundRobinRound.RegisterPlayerReference(playerNames[3]).Should().NotBeNull();
            RunTestsWithFourPlayers(roundRobinRound.Groups.First());

            roundRobinRound.RegisterPlayerReference(playerNames[4]).Should().NotBeNull();
            RunTestsWithFivePlayers(roundRobinRound.Groups.First());

            roundRobinRound.RegisterPlayerReference(playerNames[5]).Should().NotBeNull();
            RunTestsWithSixPlayers(roundRobinRound.Groups.First());

            roundRobinRound.RegisterPlayerReference(playerNames[6]).Should().NotBeNull();
            RunTestsWithSevenPlayers(roundRobinRound.Groups.First());

            roundRobinRound.RegisterPlayerReference(playerNames[7]).Should().NotBeNull();
            RunTestsWithEightPlayers(roundRobinRound.Groups.First());
        }

        private void RunTestsWithOnePlayer(GroupBase group)
        {
            group.Matches[0].Player1.Name.Should().Be("Maru");
            group.Matches[0].Player2.Should().BeNull();
        }

        private void RunTestsWithTwoPlayers(GroupBase group)
        {
            group.Matches[0].Player1.Name.Should().Be("Maru");
            group.Matches[0].Player2.Name.Should().Be("Stork");
        }

        private void RunTestsWithThreePlayers(GroupBase group)
        {
            group.Matches[0].Player1.Name.Should().Be("Maru");
            group.Matches[0].Player2.Name.Should().Be("Taeja");

            group.Matches[1].Player1.Name.Should().Be("Taeja");
            group.Matches[1].Player2.Name.Should().Be("Stork");

            group.Matches[2].Player1.Name.Should().Be("Stork");
            group.Matches[2].Player2.Name.Should().Be("Maru");
        }

        private void RunTestsWithFourPlayers(GroupBase group)
        {
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

        private void RunTestsWithFivePlayers(GroupBase group)
        {
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

        private void RunTestsWithSixPlayers(GroupBase group)
        {
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

        private void RunTestsWithSevenPlayers(GroupBase group)
        {
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

        private void RunTestsWithEightPlayers(GroupBase group)
        {
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

        private RoundRobinGroup RegisterPlayers(List<string> playerNames)
        {
            foreach (string playerName in playerNames)
            {
                roundRobinRound.RegisterPlayerReference(playerName);
            }

            return roundRobinRound.Groups.First() as RoundRobinGroup;
        }
    }
}
