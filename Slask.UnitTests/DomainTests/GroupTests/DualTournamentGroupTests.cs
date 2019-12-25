using FluentAssertions;
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
    public class DualTournamentGroupTests : IDisposable
    {
        private readonly Tournament tournament;
        private readonly DualTournamentRound dualTournamentRound;

        public DualTournamentGroupTests()
        {
            tournament = Tournament.Create("GSL 2019");
            dualTournamentRound = tournament.AddDualTournamentRound("Dual tournament round", 3) as DualTournamentRound;
        }

        public void Dispose()
        {
            SystemTimeMocker.Reset();
        }

        [Fact]
        public void CanCreateGroup()
        {
            DualTournamentGroup group = DualTournamentGroup.Create(dualTournamentRound);

            group.Should().NotBeNull();
            group.Id.Should().NotBeEmpty();
            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().HaveCount(5);
            group.RoundId.Should().Be(dualTournamentRound.Id);
            group.Round.Should().Be(dualTournamentRound);
        }

        [Fact]
        public void CanAddPlayerReferenceToGroup()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string playerName = "Maru";

            group.AddPlayerReference(playerName);

            group.ParticipatingPlayers.Should().HaveCount(1);
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
        }

        [Fact]
        public void PlayerReferenceIsReturnedWhenSuccessfullyAddedPlayerReference()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string playerName = "Maru";

            PlayerReference returnedPlayerReference = group.AddPlayerReference(playerName);

            returnedPlayerReference.Should().NotBeNull();
            returnedPlayerReference.Name.Should().Be(playerName);
        }

        [Fact]
        public void CannotAddPlayerReferenceToGroupTwice()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string playerName = "Maru";

            group.AddPlayerReference(playerName);
            group.AddPlayerReference(playerName);

            group.ParticipatingPlayers.Should().HaveCount(1);
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
        }

        [Fact]
        public void NullIsReturnedWhenNotSuccessfullyAddedPlayerReference()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string playerName = "Maru";

            PlayerReference firstReturnedPlayerReference = group.AddPlayerReference(playerName);
            PlayerReference secondReturnedPlayerReference = group.AddPlayerReference(playerName);

            firstReturnedPlayerReference.Should().NotBeNull();
            firstReturnedPlayerReference.Name.Should().Be(playerName);

            secondReturnedPlayerReference.Should().BeNull();
        }

        [Fact]
        public void CanRemovePlayerReferenceFromGroup()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string playerName = "Maru";

            group.AddPlayerReference(playerName);
            group.RemovePlayerReference(playerName);

            group.ParticipatingPlayers.Should().BeEmpty();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().BeNull();
        }

        [Fact]
        public void ReturnsTrueFlagWhenSuccessfullyRemovingPlayerReference()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string playerName = "Maru";

            group.AddPlayerReference(playerName);
            bool result = group.RemovePlayerReference(playerName);

            result.Should().BeTrue();
        }

        [Fact]
        public void CannotAddPlayerReferenceAfterFirstMatchStartDateTimeHasPassed()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string firstPlayerName = "Maru";
            string secondPlayerName = "Stork";
            string thirdPlayerName = "Rain";

            group.AddPlayerReference(firstPlayerName);
            group.AddPlayerReference(secondPlayerName);

            SystemTimeMocker.SetOneSecondAfter(group.Matches.First().StartDateTime);

            group.AddPlayerReference(thirdPlayerName);

            group.ParticipatingPlayers.Should().HaveCount(2);
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == firstPlayerName).Should().NotBeNull();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == secondPlayerName).Should().NotBeNull();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == thirdPlayerName).Should().BeNull();
        }

        [Fact]
        public void CannotRemovePlayerReferenceAfterFirstMatchStartDateTimeHasPassed()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string firstPlayerName = "Maru";
            string secondPlayerName = "Stork";

            group.AddPlayerReference(firstPlayerName);
            group.AddPlayerReference(secondPlayerName);

            SystemTimeMocker.SetOneSecondAfter(group.Matches.First().StartDateTime);

            group.RemovePlayerReference(firstPlayerName);

            group.ParticipatingPlayers.Should().HaveCount(2);
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == firstPlayerName).Should().NotBeNull();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == secondPlayerName).Should().NotBeNull();
        }

        [Fact]
        public void ReturnsFalseFlagWhenNotSuccessfullyRemovingPlayerReference()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string firstPlayerName = "Maru";
            string secondPlayerName = "Stork";

            group.AddPlayerReference(firstPlayerName);
            group.AddPlayerReference(secondPlayerName);

            SystemTimeMocker.SetOneSecondAfter(group.Matches.First().StartDateTime);

            bool result = group.RemovePlayerReference(firstPlayerName);

            result.Should().BeFalse();
        }

        [Fact]
        public void CanConstructDualTournamentMatchLayout()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string firstPlayerName = "Maru";
            string secondPlayerName = "Stork";
            string thirdPlayerName = "Taeja";
            string fourthPlayerName = "Rain";

            group.AddPlayerReference(firstPlayerName);
            group.AddPlayerReference(secondPlayerName);
            group.AddPlayerReference(thirdPlayerName);
            group.AddPlayerReference(fourthPlayerName);

            group.Matches.Should().HaveCount(5);

            group.Matches[0].Player1.Name.Should().Be(firstPlayerName);
            group.Matches[0].Player2.Name.Should().Be(secondPlayerName);

            group.Matches[1].Player1.Name.Should().Be(thirdPlayerName);
            group.Matches[1].Player2.Name.Should().Be(fourthPlayerName);

            group.Matches[2].Player1.Should().BeNull();
            group.Matches[2].Player2.Should().BeNull();

            group.Matches[3].Player1.Should().BeNull();
            group.Matches[3].Player2.Should().BeNull();

            group.Matches[4].Player1.Should().BeNull();
            group.Matches[4].Player2.Should().BeNull();
        }

        [Fact]
        public void CannotAddMoreThanFourPlayerReferencesToGroup()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;

            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber" };

            foreach (string playerName in playerNames)
            {
                group.AddPlayerReference(playerName);
            }

            group.ParticipatingPlayers.Single(playerReference => playerReference.Name == playerNames[0]).Should().NotBeNull();
            group.ParticipatingPlayers.Single(playerReference => playerReference.Name == playerNames[1]).Should().NotBeNull();
            group.ParticipatingPlayers.Single(playerReference => playerReference.Name == playerNames[2]).Should().NotBeNull();
            group.ParticipatingPlayers.Single(playerReference => playerReference.Name == playerNames[3]).Should().NotBeNull();
            group.ParticipatingPlayers.SingleOrDefault(playerReference => playerReference.Name == playerNames[4]).Should().BeNull();

            group.Matches.Should().HaveCount(5);

            group.Matches[0].Player1.Name.Should().NotBeNull();
            group.Matches[0].Player2.Name.Should().NotBeNull();

            group.Matches[1].Player1.Name.Should().NotBeNull();
            group.Matches[1].Player2.Name.Should().NotBeNull();

            group.Matches[2].Player1.Should().BeNull();
            group.Matches[2].Player2.Should().BeNull();

            group.Matches[3].Player1.Should().BeNull();
            group.Matches[3].Player2.Should().BeNull();

            group.Matches[4].Player1.Should().BeNull();
            group.Matches[4].Player2.Should().BeNull();
        }

        [Fact]
        public void CanSwitchPlacesOnPlayerReferencesThatAreInSameMatch()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;

            PlayerReference firstPlayerReference = group.AddPlayerReference("Maru");
            PlayerReference secondPlayerReference = group.AddPlayerReference("Stork");

            group.SwitchPlayerReferences(group.Matches.First().Player1, group.Matches.First().Player2);

            group.Matches.First().Player1.PlayerReference.Should().Be(secondPlayerReference);
            group.Matches.First().Player2.PlayerReference.Should().Be(firstPlayerReference);
        }

        [Fact]
        public void CanSwitchPlacesOnPlayerReferencesThatAreInSameGroup()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;

            PlayerReference firstPlayerReference = group.AddPlayerReference("Maru");
            PlayerReference secondPlayerReference = group.AddPlayerReference("Stork");
            PlayerReference thirdPlayerReference = group.AddPlayerReference("Taeja");
            PlayerReference fourthPlayerReference = group.AddPlayerReference("Rain");

            Match firstMatch = group.Matches[0];
            Match secondMatch = group.Matches[1];

            group.SwitchPlayerReferences(firstMatch.Player1, secondMatch.Player2);

            firstMatch.Player1.PlayerReference.Should().Be(fourthPlayerReference);
            firstMatch.Player2.PlayerReference.Should().Be(secondPlayerReference);
            secondMatch.Player1.PlayerReference.Should().Be(thirdPlayerReference);
            secondMatch.Player2.PlayerReference.Should().Be(firstPlayerReference);
        }

        [Fact]
        public void CannotSwitchPlacesOnPlayerReferenceWhenAnyPlayerIsNull()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;

            PlayerReference firstPlayerReference = group.AddPlayerReference("Maru");
            PlayerReference secondPlayerReference = group.AddPlayerReference("Stork");
            PlayerReference thirdPlayerReference = group.AddPlayerReference("Taeja");
            PlayerReference fourthPlayerReference = group.AddPlayerReference("Rain");

            Match firstMatch = group.Matches[0];
            Match secondMatch = group.Matches[1];
            Match thirdMatch = group.Matches[2];

            group.SwitchPlayerReferences(thirdMatch.Player1, secondMatch.Player2);
            group.SwitchPlayerReferences(firstMatch.Player1, thirdMatch.Player2);

            firstMatch.Player1.PlayerReference.Should().Be(firstPlayerReference);
            firstMatch.Player2.PlayerReference.Should().Be(secondPlayerReference);
            secondMatch.Player1.PlayerReference.Should().Be(thirdPlayerReference);
            secondMatch.Player2.PlayerReference.Should().Be(fourthPlayerReference);
        }

        [Fact]
        public void CannotSwitchPlacesOnAnyPlayerReferencesWhenAMatchInGroupHasBegun()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;

            PlayerReference firstPlayerReference = group.AddPlayerReference("Maru");
            PlayerReference secondPlayerReference = group.AddPlayerReference("Stork");
            PlayerReference thirdPlayerReference = group.AddPlayerReference("Taeja");
            PlayerReference fourthPlayerReference = group.AddPlayerReference("Rain");

            Match firstMatch = group.Matches[0];
            Match secondMatch = group.Matches[1];

            SystemTimeMocker.SetOneSecondAfter(firstMatch.StartDateTime);

            group.SwitchPlayerReferences(firstMatch.Player1, secondMatch.Player2);

            firstMatch.Player1.PlayerReference.Should().Be(firstPlayerReference);
            firstMatch.Player2.PlayerReference.Should().Be(secondPlayerReference);
            secondMatch.Player1.PlayerReference.Should().Be(thirdPlayerReference);
            secondMatch.Player2.PlayerReference.Should().Be(fourthPlayerReference);
        }

        [Fact]
        public void CannotSwitchPlacesOnPlayerReferencesThatResidesInDifferentGroups()
        {
            DualTournamentGroup firstGroup = dualTournamentRound.AddGroup() as DualTournamentGroup;
            DualTournamentGroup secondGroup = dualTournamentRound.AddGroup() as DualTournamentGroup;

            PlayerReference firstPlayerReference = firstGroup.AddPlayerReference("Maru");
            PlayerReference secondPlayerReference = firstGroup.AddPlayerReference("Stork");

            PlayerReference thirdPlayerReference = secondGroup.AddPlayerReference("Taeja");
            PlayerReference fourthPlayerReference = secondGroup.AddPlayerReference("Rain");

            Match firstGroupMatch = firstGroup.Matches.First();
            Match secondGroupMatch = secondGroup.Matches.First();

            firstGroup.SwitchPlayerReferences(firstGroupMatch.Player1, secondGroupMatch.Player2);

            firstGroupMatch.Player1.PlayerReference.Should().Be(firstPlayerReference);
            firstGroupMatch.Player2.PlayerReference.Should().Be(secondPlayerReference);
            secondGroupMatch.Player1.PlayerReference.Should().Be(thirdPlayerReference);
            secondGroupMatch.Player2.PlayerReference.Should().Be(fourthPlayerReference);
        }

        [Fact]
        public void StartDateTimeForMatchesMustBeInMatchOrder()
        {
            DualTournamentGroup dualTournamentGroup = dualTournamentRound.AddGroup() as DualTournamentGroup;

            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain" };

            foreach (string playerName in playerNames)
            {
                dualTournamentGroup.AddPlayerReference(playerName);
            }

            List<DateTime> dateTimesBeforeChange = new List<DateTime>();

            foreach (Match match in dualTournamentGroup.Matches)
            {
                dateTimesBeforeChange.Add(match.StartDateTime);
            }

            DateTime twoHoursLater = SystemTime.Now.AddHours(2);
            DateTime oneHourLater = SystemTime.Now.AddHours(1);
            DateTime fourHoursLater = SystemTime.Now.AddHours(4);
            DateTime threeHoursLater = SystemTime.Now.AddHours(3);
            DateTime twelveHoursLater = SystemTime.Now.AddHours(12);

            dualTournamentGroup.Matches[0].SetStartDateTime(twoHoursLater);
            dualTournamentGroup.Matches[1].SetStartDateTime(oneHourLater);
            dualTournamentGroup.Matches[2].SetStartDateTime(fourHoursLater);
            dualTournamentGroup.Matches[3].SetStartDateTime(threeHoursLater);
            dualTournamentGroup.Matches[4].SetStartDateTime(twelveHoursLater);

            dualTournamentGroup.Matches[0].StartDateTime.Should().Be(dateTimesBeforeChange[0]);
            dualTournamentGroup.Matches[1].StartDateTime.Should().Be(dateTimesBeforeChange[1]);
            dualTournamentGroup.Matches[2].StartDateTime.Should().Be(dateTimesBeforeChange[2]);
            dualTournamentGroup.Matches[3].StartDateTime.Should().Be(dateTimesBeforeChange[3]);
            dualTournamentGroup.Matches[4].StartDateTime.Should().Be(dateTimesBeforeChange[4]);
        }
    }
}
