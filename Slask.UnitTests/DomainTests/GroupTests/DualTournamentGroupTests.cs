using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.TestCore;
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
        public void CompletelyNewPlayerReferencesAreAlsoAddedToTournamentPool()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string playerName = "Maru";

            group.AddPlayerReference(playerName);

            List<PlayerReference> playerReferences = tournament.GetPlayerReferencesInTournament();
            playerReferences.Should().HaveCount(1);
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
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
        public void CannotAddPlayerReferenceToTournamentPoolTwice()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string playerName = "Maru";

            group.AddPlayerReference(playerName);
            group.AddPlayerReference(playerName);

            List<PlayerReference> playerReferences = tournament.GetPlayerReferencesInTournament();
            playerReferences.Should().HaveCount(1);
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
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
        public void TournamentRemovesPlayerReferenceFromTournamentPoolWhenLastReferenceIsRemoved()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string playerName = "Maru";

            group.AddPlayerReference(playerName);
            group.RemovePlayerReference(playerName);

            List<PlayerReference> playerReferences = tournament.GetPlayerReferencesInTournament();
            playerReferences.Should().BeEmpty();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().BeNull();
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

            SystemTimeMocker.Set(group.Matches.First().StartDateTime.AddMinutes(1));

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

            SystemTimeMocker.Set(group.Matches.First().StartDateTime.AddMinutes(1));

            group.RemovePlayerReference(firstPlayerName);

            group.ParticipatingPlayers.Should().HaveCount(2);
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == firstPlayerName).Should().NotBeNull();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == secondPlayerName).Should().NotBeNull();
        }

        [Fact]
        public void DoesNotRemovePlayerReferenceFromTournamentPoolWhenNotSuccessfullyRemovingPlayerReference()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string firstPlayerName = "Maru";
            string secondPlayerName = "Stork";

            group.AddPlayerReference(firstPlayerName);
            group.AddPlayerReference(secondPlayerName);

            SystemTimeMocker.Set(group.Matches.First().StartDateTime.AddMinutes(1));

            group.RemovePlayerReference(firstPlayerName);

            List<PlayerReference> playerReferences = tournament.GetPlayerReferencesInTournament();
            playerReferences.Should().HaveCount(2);
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == firstPlayerName).Should().NotBeNull();
            playerReferences.FirstOrDefault(playerReference => playerReference.Name == secondPlayerName).Should().NotBeNull();
        }

        [Fact]
        public void ReturnsFalseFlagWhenNotSuccessfullyRemovingPlayerReference()
        {
            DualTournamentGroup group = dualTournamentRound.AddGroup() as DualTournamentGroup;
            string firstPlayerName = "Maru";
            string secondPlayerName = "Stork";

            group.AddPlayerReference(firstPlayerName);
            group.AddPlayerReference(secondPlayerName);

            SystemTimeMocker.Set(group.Matches.First().StartDateTime.AddMinutes(1));

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
    }
}
