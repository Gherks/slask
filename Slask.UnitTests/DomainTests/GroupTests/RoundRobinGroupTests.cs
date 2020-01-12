﻿using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.GroupTests
{
    public class RoundRobinGroupTests : IDisposable
    {
        private readonly Tournament tournament;
        private readonly RoundRobinRound roundRobinRound;

        public RoundRobinGroupTests()
        {
            tournament = Tournament.Create("GSL 2019");
            roundRobinRound = tournament.AddRoundRobinRound("Round robin round", 3, 2) as RoundRobinRound;
        }

        public void Dispose()
        {
            SystemTimeMocker.Reset();
        }

        [Fact]
        public void CanCreateGroup()
        {
            RoundRobinGroup group = RoundRobinGroup.Create(roundRobinRound);

            group.Should().NotBeNull();
            group.Id.Should().NotBeEmpty();
            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().BeEmpty();
            group.RoundId.Should().Be(roundRobinRound.Id);
            group.Round.Should().Be(roundRobinRound);
        }

        [Fact]
        public void CanAddPlayerReferenceToGroup()
        {
            RoundRobinGroup group = roundRobinRound.AddGroup() as RoundRobinGroup;
            string playerName = "Maru";

            group.AddNewPlayerReference(playerName);

            group.ParticipatingPlayers.Should().HaveCount(1);
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
        }

        [Fact]
        public void PlayerReferenceIsReturnedWhenSuccessfullyAddedPlayerReference()
        {
            RoundRobinGroup group = roundRobinRound.AddGroup() as RoundRobinGroup;
            string playerName = "Maru";

            PlayerReference returnedPlayerReference = group.AddNewPlayerReference(playerName);

            returnedPlayerReference.Should().NotBeNull();
            returnedPlayerReference.Name.Should().Be(playerName);
        }

        [Fact]
        public void CannotAddPlayerReferenceToGroupTwice()
        {
            RoundRobinGroup group = roundRobinRound.AddGroup() as RoundRobinGroup;
            string playerName = "Maru";

            group.AddNewPlayerReference(playerName);
            group.AddNewPlayerReference(playerName);

            group.ParticipatingPlayers.Should().HaveCount(1);
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
        }

        [Fact]
        public void NullIsReturnedWhenNotSuccessfullyAddedPlayerReference()
        {
            RoundRobinGroup group = roundRobinRound.AddGroup() as RoundRobinGroup;
            string playerName = "Maru";

            PlayerReference firstReturnedPlayerReference = group.AddNewPlayerReference(playerName);
            PlayerReference secondReturnedPlayerReference = group.AddNewPlayerReference(playerName);

            firstReturnedPlayerReference.Should().NotBeNull();
            firstReturnedPlayerReference.Name.Should().Be(playerName);

            secondReturnedPlayerReference.Should().BeNull();
        }

        [Fact]
        public void CanRemovePlayerReferenceFromGroup()
        {
            RoundRobinGroup group = roundRobinRound.AddGroup() as RoundRobinGroup;
            string playerName = "Maru";

            group.AddNewPlayerReference(playerName);
            group.RemovePlayerReference(playerName);

            group.ParticipatingPlayers.Should().BeEmpty();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().BeNull();
        }

        [Fact]
        public void ReturnsTrueFlagWhenSuccessfullyRemovingPlayerReference()
        {
            RoundRobinGroup group = roundRobinRound.AddGroup() as RoundRobinGroup;
            string playerName = "Maru";

            group.AddNewPlayerReference(playerName);
            bool result = group.RemovePlayerReference(playerName);

            result.Should().BeTrue();
        }

        [Fact]
        public void MatchIsCreatedWhenTwoPlayerReferencesAreAddedToGroup()
        {
            RoundRobinGroup group = roundRobinRound.AddGroup() as RoundRobinGroup;
            string firstPlayerName = "Maru";
            string secondPlayerName = "Stork";

            group.AddNewPlayerReference(firstPlayerName);
            group.AddNewPlayerReference(secondPlayerName);

            group.Matches.Should().HaveCount(1);
            group.Matches.FirstOrDefault(match => match.Player1.Name == firstPlayerName).Should().NotBeNull();
            group.Matches.FirstOrDefault(match => match.Player2.Name == secondPlayerName).Should().NotBeNull();
        }

        [Fact]
        public void CannotAddPlayerReferenceAfterFirstMatchStartDateTimeHasPassed()
        {
            RoundRobinGroup group = roundRobinRound.AddGroup() as RoundRobinGroup;
            string firstPlayerName = "Maru";
            string secondPlayerName = "Stork";
            string thirdPlayerName = "Rain";

            group.AddNewPlayerReference(firstPlayerName);
            group.AddNewPlayerReference(secondPlayerName);

            SystemTimeMocker.SetOneSecondAfter(group.Matches.First().StartDateTime);

            group.AddNewPlayerReference(thirdPlayerName);

            group.ParticipatingPlayers.Should().HaveCount(2);
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == firstPlayerName).Should().NotBeNull();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == secondPlayerName).Should().NotBeNull();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == thirdPlayerName).Should().BeNull();
        }

        [Fact]
        public void CannotRemovePlayerReferenceAfterFirstMatchStartDateTimeHasPassed()
        {
            RoundRobinGroup group = roundRobinRound.AddGroup() as RoundRobinGroup;
            string firstPlayerName = "Maru";
            string secondPlayerName = "Stork";

            group.AddNewPlayerReference(firstPlayerName);
            group.AddNewPlayerReference(secondPlayerName);

            SystemTimeMocker.SetOneSecondAfter(group.Matches.First().StartDateTime);

            group.RemovePlayerReference(firstPlayerName);

            group.ParticipatingPlayers.Should().HaveCount(2);
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == firstPlayerName).Should().NotBeNull();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == secondPlayerName).Should().NotBeNull();
        }

        [Fact]
        public void ReturnsFalseFlagWhenNotSuccessfullyRemovingPlayerReference()
        {
            RoundRobinGroup group = roundRobinRound.AddGroup() as RoundRobinGroup;
            string firstPlayerName = "Maru";
            string secondPlayerName = "Stork";

            group.AddNewPlayerReference(firstPlayerName);
            group.AddNewPlayerReference(secondPlayerName);

            SystemTimeMocker.SetOneSecondAfter(group.Matches.First().StartDateTime);

            bool result = group.RemovePlayerReference(firstPlayerName);

            result.Should().BeFalse();
        }

        [Fact]
        public void CannotSwitchPlacesOnPlayerReferences()
        {
            RoundRobinGroup group = roundRobinRound.AddGroup() as RoundRobinGroup;

            group.AddNewPlayerReference("Maru");
            group.AddNewPlayerReference("Stork");
            group.AddNewPlayerReference("Taeja");
            group.AddNewPlayerReference("Rain");

            Match firstMatch = group.Matches[0];
            Match secondMatch = group.Matches[1];

            PlayerReference firstPlayerReference = group.Matches[0].Player1.PlayerReference;
            PlayerReference secondPlayerReference = group.Matches[0].Player2.PlayerReference;
            PlayerReference thirdPlayerReference = group.Matches[1].Player1.PlayerReference;
            PlayerReference fourthPlayerReference = group.Matches[1].Player2.PlayerReference;

            group.SwitchPlayerReferences(firstMatch.Player1, secondMatch.Player2);

            firstMatch.Player1.PlayerReference.Should().Be(firstPlayerReference);
            firstMatch.Player2.PlayerReference.Should().Be(secondPlayerReference);
            secondMatch.Player1.PlayerReference.Should().Be(thirdPlayerReference);
            secondMatch.Player2.PlayerReference.Should().Be(fourthPlayerReference);
        }

        [Fact]
        public void NoStartDateTimeRestrictionIsAppliedToMatches()
        {
            RoundRobinGroup roundRobinGroup = roundRobinRound.AddGroup() as RoundRobinGroup;

            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain" };

            foreach (string playerName in playerNames)
            {
                roundRobinGroup.AddNewPlayerReference(playerName);
            }

            DateTime twoHoursLater = SystemTime.Now.AddHours(2);
            DateTime oneHourLater = SystemTime.Now.AddHours(1);
            DateTime fourHoursLater = SystemTime.Now.AddHours(4);
            DateTime threeHoursLater = SystemTime.Now.AddHours(3);
            DateTime twelveHoursLater = SystemTime.Now.AddHours(12);
            DateTime eightHoursLater = SystemTime.Now.AddHours(8);

            roundRobinGroup.Matches[0].SetStartDateTime(twoHoursLater);
            roundRobinGroup.Matches[1].SetStartDateTime(oneHourLater);
            roundRobinGroup.Matches[2].SetStartDateTime(fourHoursLater);
            roundRobinGroup.Matches[3].SetStartDateTime(threeHoursLater);
            roundRobinGroup.Matches[4].SetStartDateTime(twelveHoursLater);
            roundRobinGroup.Matches[5].SetStartDateTime(eightHoursLater);

            roundRobinGroup.Matches[0].StartDateTime.Should().Be(twoHoursLater);
            roundRobinGroup.Matches[1].StartDateTime.Should().Be(oneHourLater);
            roundRobinGroup.Matches[2].StartDateTime.Should().Be(fourHoursLater);
            roundRobinGroup.Matches[3].StartDateTime.Should().Be(threeHoursLater);
            roundRobinGroup.Matches[4].StartDateTime.Should().Be(twelveHoursLater);
            roundRobinGroup.Matches[5].StartDateTime.Should().Be(eightHoursLater);
        }

        [Fact]
        public void CanConstructRoundRobinMatchLayout()
        {
            RoundRobinGroup group = RoundRobinGroup.Create(roundRobinRound);

            group.AddNewPlayerReference("Maru").Should().NotBeNull();
            RunTestsWithOnePlayer(group);

            group.AddNewPlayerReference("Stork").Should().NotBeNull();
            RunTestsWithTwoPlayers(group);

            group.AddNewPlayerReference("Taeja").Should().NotBeNull();
            RunTestsWithThreePlayers(group);

            group.AddNewPlayerReference("Rain").Should().NotBeNull();
            RunTestsWithFourPlayers(group);

            group.AddNewPlayerReference("Bomber").Should().NotBeNull();
            RunTestsWithFivePlayers(group);

            group.AddNewPlayerReference("FanTaSy").Should().NotBeNull();
            RunTestsWithSixPlayers(group);

            group.AddNewPlayerReference("Stephano").Should().NotBeNull();
            RunTestsWithSevenPlayers(group);

            group.AddNewPlayerReference("Thorzain").Should().NotBeNull();
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
    }
}
