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
    public class BracketGroupTests : IDisposable
    {
        private readonly Tournament tournament;
        private readonly BracketRound bracketRound;

        // StartDateTime for matches is properly set up according to layout (child nodes needs to be resolved before current node can start)

        public BracketGroupTests()
        {
            tournament = Tournament.Create("GSL 2019");
            bracketRound = tournament.AddBracketRound("Bracket round", 3) as BracketRound;
        }

        public void Dispose()
        {
            SystemTimeMocker.Reset();
        }

        [Fact]
        public void CanCreateGroup()
        {
            BracketGroup group = BracketGroup.Create(bracketRound);

            group.Should().NotBeNull();
            group.Id.Should().NotBeEmpty();
            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().BeEmpty();
            group.RoundId.Should().Be(bracketRound.Id);
            group.Round.Should().Be(bracketRound);
            group.BracketNodeSystem.Should().BeNull();
        }

        [Fact]
        public void CanAddPlayerReferenceToGroup()
        {
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;
            string playerName = "Maru";

            group.AddNewPlayerReference(playerName);

            group.ParticipatingPlayers.Should().HaveCount(1);
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
        }

        [Fact]
        public void PlayerReferenceIsReturnedWhenSuccessfullyAddedPlayerReference()
        {
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;
            string playerName = "Maru";

            PlayerReference returnedPlayerReference = group.AddNewPlayerReference(playerName);

            returnedPlayerReference.Should().NotBeNull();
            returnedPlayerReference.Name.Should().Be(playerName);
        }

        [Fact]
        public void CannotAddPlayerReferenceToGroupTwice()
        {
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;
            string playerName = "Maru";

            group.AddNewPlayerReference(playerName);
            group.AddNewPlayerReference(playerName);

            group.ParticipatingPlayers.Should().HaveCount(1);
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
        }

        [Fact]
        public void NullIsReturnedWhenNotSuccessfullyAddedPlayerReference()
        {
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;
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
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;
            string playerName = "Maru";

            group.AddNewPlayerReference(playerName);
            group.RemovePlayerReference(playerName);

            group.ParticipatingPlayers.Should().BeEmpty();
            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().BeNull();
        }

        [Fact]
        public void ReturnsTrueFlagWhenSuccessfullyRemovingPlayerReference()
        {
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;
            string playerName = "Maru";

            group.AddNewPlayerReference(playerName);
            bool result = group.RemovePlayerReference(playerName);

            result.Should().BeTrue();
        }

        [Fact]
        public void MatchIsCreatedWhenTwoPlayerReferencesAreAddedToGroup()
        {
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;
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
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;
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
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;
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
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;
            string firstPlayerName = "Maru";
            string secondPlayerName = "Stork";

            group.AddNewPlayerReference(firstPlayerName);
            group.AddNewPlayerReference(secondPlayerName);

            SystemTimeMocker.SetOneSecondAfter(group.Matches.First().StartDateTime);

            bool result = group.RemovePlayerReference(firstPlayerName);

            result.Should().BeFalse();
        }

        [Fact]
        public void CanSwitchPlacesOnPlayerReferencesThatAreInSameMatch()
        {
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;

            PlayerReference firstPlayerReference = group.AddNewPlayerReference("Maru");
            PlayerReference secondPlayerReference = group.AddNewPlayerReference("Stork");

            PlayerSwitcher.SwitchMatchesOn(group.Matches.First().Player1, group.Matches.First().Player2);

            group.Matches.First().Player1.PlayerReference.Should().Be(secondPlayerReference);
            group.Matches.First().Player2.PlayerReference.Should().Be(firstPlayerReference);
        }

        [Fact]
        public void CanSwitchPlacesOnPlayerReferencesThatAreInSameGroup()
        {
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;

            PlayerReference firstPlayerReference = group.AddNewPlayerReference("Maru");
            PlayerReference secondPlayerReference = group.AddNewPlayerReference("Stork");
            PlayerReference thirdPlayerReference = group.AddNewPlayerReference("Taeja");
            PlayerReference fourthPlayerReference = group.AddNewPlayerReference("Rain");

            Match firstMatch = group.Matches[0];
            Match secondMatch = group.Matches[1];

            PlayerSwitcher.SwitchMatchesOn(firstMatch.Player1, secondMatch.Player2);

            firstMatch.Player1.PlayerReference.Should().Be(fourthPlayerReference);
            firstMatch.Player2.PlayerReference.Should().Be(secondPlayerReference);
            secondMatch.Player1.PlayerReference.Should().Be(thirdPlayerReference);
            secondMatch.Player2.PlayerReference.Should().Be(firstPlayerReference);
        }

        [Fact]
        public void CanSwitchPlacesOnPlayerReferencesThatAreInDifferentGroups()
        {
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;

            PlayerReference firstPlayerReference = group.AddNewPlayerReference("Maru");
            PlayerReference secondPlayerReference = group.AddNewPlayerReference("Stork");
            PlayerReference thirdPlayerReference = group.AddNewPlayerReference("Taeja");
            PlayerReference fourthPlayerReference = group.AddNewPlayerReference("Rain");

            Match firstMatch = group.Matches[0];
            Match secondMatch = group.Matches[1];

            PlayerSwitcher.SwitchMatchesOn(firstMatch.Player1, secondMatch.Player2);

            firstMatch.Player1.PlayerReference.Should().Be(fourthPlayerReference);
            firstMatch.Player2.PlayerReference.Should().Be(secondPlayerReference);
            secondMatch.Player1.PlayerReference.Should().Be(thirdPlayerReference);
            secondMatch.Player2.PlayerReference.Should().Be(firstPlayerReference);
        }

        [Fact]
        public void CannotSwitchPlacesOnPlayerReferenceWhenAnyPlayerIsNull()
        {
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;

            PlayerReference firstPlayerReference = group.AddNewPlayerReference("Maru");
            PlayerReference secondPlayerReference = group.AddNewPlayerReference("Stork");
            PlayerReference thirdPlayerReference = group.AddNewPlayerReference("Taeja");
            PlayerReference fourthPlayerReference = group.AddNewPlayerReference("Rain");

            Match firstMatch = group.Matches[0];
            Match secondMatch = group.Matches[1];
            Match thirdMatch = group.Matches[2];

            PlayerSwitcher.SwitchMatchesOn(thirdMatch.Player1, secondMatch.Player2);
            PlayerSwitcher.SwitchMatchesOn(firstMatch.Player1, thirdMatch.Player2);

            firstMatch.Player1.PlayerReference.Should().Be(firstPlayerReference);
            firstMatch.Player2.PlayerReference.Should().Be(secondPlayerReference);
            secondMatch.Player1.PlayerReference.Should().Be(thirdPlayerReference);
            secondMatch.Player2.PlayerReference.Should().Be(fourthPlayerReference);
        }

        [Fact]
        public void CannotSwitchPlacesOnAnyPlayerReferencesWhenAMatchInGroupHasBegun()
        {
            BracketGroup group = bracketRound.AddGroup() as BracketGroup;

            PlayerReference firstPlayerReference = group.AddNewPlayerReference("Maru");
            PlayerReference secondPlayerReference = group.AddNewPlayerReference("Stork");
            PlayerReference thirdPlayerReference = group.AddNewPlayerReference("Taeja");
            PlayerReference fourthPlayerReference = group.AddNewPlayerReference("Rain");

            Match firstMatch = group.Matches[0];
            Match secondMatch = group.Matches[1];

            SystemTimeMocker.SetOneSecondAfter(firstMatch.StartDateTime);

            PlayerSwitcher.SwitchMatchesOn(firstMatch.Player1, secondMatch.Player2);

            firstMatch.Player1.PlayerReference.Should().Be(firstPlayerReference);
            firstMatch.Player2.PlayerReference.Should().Be(secondPlayerReference);
            secondMatch.Player1.PlayerReference.Should().Be(thirdPlayerReference);
            secondMatch.Player2.PlayerReference.Should().Be(fourthPlayerReference);
        }

        [Fact]
        public void CanConstructBracketLayoutWithEvenPlayers()
        {
            BracketGroup bracketGroup = bracketRound.AddGroup() as BracketGroup;

            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };

            foreach (string playerName in playerNames)
            {
                bracketGroup.AddNewPlayerReference(playerName);
            }

            bracketGroup.Matches.Should().HaveCount(playerNames.Count - 1);

            foreach (Match match in bracketGroup.Matches)
            {
                match.Should().NotBeNull();
            }

            bracketGroup.Matches[0].Player1.Name.Should().Be("Maru");
            bracketGroup.Matches[0].Player2.Name.Should().Be("Stork");

            bracketGroup.Matches[1].Player1.Name.Should().Be("Taeja");
            bracketGroup.Matches[1].Player2.Name.Should().Be("Rain");

            bracketGroup.Matches[2].Player1.Name.Should().Be("Bomber");
            bracketGroup.Matches[2].Player2.Name.Should().Be("FanTaSy");

            bracketGroup.Matches[3].Player1.Name.Should().Be("Stephano");
            bracketGroup.Matches[3].Player2.Name.Should().Be("Thorzain");

            bracketGroup.Matches[4].Player1.Should().BeNull();
            bracketGroup.Matches[4].Player2.Should().BeNull();

            bracketGroup.Matches[5].Player1.Should().BeNull();
            bracketGroup.Matches[5].Player2.Should().BeNull();

            bracketGroup.Matches[6].Player1.Should().BeNull();
            bracketGroup.Matches[6].Player2.Should().BeNull();
        }

        [Fact]
        public void CanConstructBracketLayoutWithUnevenPlayers()
        {
            BracketGroup bracketGroup = bracketRound.AddGroup() as BracketGroup;

            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano" };

            foreach (string playerName in playerNames)
            {
                bracketGroup.AddNewPlayerReference(playerName);
            }

            bracketGroup.Matches.Should().HaveCount(playerNames.Count - 1);

            foreach (Match match in bracketGroup.Matches)
            {
                match.Should().NotBeNull();
            }

            bracketGroup.Matches[0].Player1.Name.Should().Be("Maru");
            bracketGroup.Matches[0].Player2.Name.Should().Be("Stork");

            bracketGroup.Matches[1].Player1.Name.Should().Be("Taeja");
            bracketGroup.Matches[1].Player2.Name.Should().Be("Rain");

            bracketGroup.Matches[2].Player1.Name.Should().Be("Bomber");
            bracketGroup.Matches[2].Player2.Name.Should().Be("FanTaSy");

            bracketGroup.Matches[3].Player1.Name.Should().Be("Stephano");
            bracketGroup.Matches[3].Player2.Should().BeNull();

            bracketGroup.Matches[4].Player1.Should().BeNull();
            bracketGroup.Matches[4].Player2.Should().BeNull();

            bracketGroup.Matches[5].Player1.Should().BeNull();
            bracketGroup.Matches[5].Player2.Should().BeNull();
        }

        [Fact]
        public void CanConstructBracketNodeSystemWithEvenPlayers()
        {
            BracketGroup bracketGroup = bracketRound.AddGroup() as BracketGroup;

            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };

            foreach (string playerName in playerNames)
            {
                bracketGroup.AddNewPlayerReference(playerName);
            }

            BracketNode finalNode = bracketGroup.BracketNodeSystem.FinalNode;

            ValidateBracketNodeConnections(finalNode, null, bracketGroup.Matches[5], bracketGroup.Matches[4]);

            BracketNode semifinalNode1 = finalNode.Children[0];
            BracketNode semifinalNode2 = finalNode.Children[1];

            ValidateBracketNodeConnections(semifinalNode1, bracketGroup.Matches[6], bracketGroup.Matches[3], bracketGroup.Matches[2]);
            ValidateBracketNodeConnections(semifinalNode2, bracketGroup.Matches[6], bracketGroup.Matches[1], bracketGroup.Matches[0]);

            BracketNode quarterfinalNode1 = semifinalNode1.Children[0];
            BracketNode quarterfinalNode2 = semifinalNode1.Children[1];

            BracketNode quarterfinalNode3 = semifinalNode2.Children[0];
            BracketNode quarterfinalNode4 = semifinalNode2.Children[1];

            ValidateBracketNodeConnections(quarterfinalNode1, bracketGroup.Matches[5], null, null);
            ValidateBracketNodeConnections(quarterfinalNode2, bracketGroup.Matches[5], null, null);

            ValidateBracketNodeConnections(quarterfinalNode3, bracketGroup.Matches[4], null, null);
            ValidateBracketNodeConnections(quarterfinalNode4, bracketGroup.Matches[4], null, null);
        }

        [Fact]
        public void CanConstructBracketNodeSystemWithUnevenPlayers()
        {
            BracketGroup bracketGroup = bracketRound.AddGroup() as BracketGroup;

            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano" };

            foreach (string playerName in playerNames)
            {
                bracketGroup.AddNewPlayerReference(playerName);
            }

            BracketNode finalNode = bracketGroup.BracketNodeSystem.FinalNode;

            ValidateBracketNodeConnections(finalNode, null, bracketGroup.Matches[4], bracketGroup.Matches[3]);

            BracketNode semifinalNode1 = finalNode.Children[0];
            BracketNode semifinalNode2 = finalNode.Children[1];

            ValidateBracketNodeConnections(semifinalNode1, bracketGroup.Matches[5], bracketGroup.Matches[2], bracketGroup.Matches[1]);
            ValidateBracketNodeConnections(semifinalNode2, bracketGroup.Matches[5], bracketGroup.Matches[0], null);

            BracketNode quarterfinalNode1 = semifinalNode1.Children[0];
            BracketNode quarterfinalNode2 = semifinalNode1.Children[1];

            BracketNode quarterfinalNode3 = semifinalNode2.Children[0];
            BracketNode quarterfinalNode4 = semifinalNode2.Children[1];

            ValidateBracketNodeConnections(quarterfinalNode1, bracketGroup.Matches[4], null, null);
            ValidateBracketNodeConnections(quarterfinalNode2, bracketGroup.Matches[4], null, null);

            ValidateBracketNodeConnections(quarterfinalNode3, bracketGroup.Matches[3], null, null);
            quarterfinalNode4.Should().BeNull();
        }

        [Fact]
        public void CreatesMatchTierListWhenBracketMatchesAreAdded()
        {
            BracketGroup bracketGroup = bracketRound.AddGroup() as BracketGroup;

            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano" };

            foreach (string playerName in playerNames)
            {
                bracketGroup.AddNewPlayerReference(playerName);
            }

            bracketGroup.BracketNodeSystem.TierCount.Should().Be(3);

            List<BracketNode> finalTier = bracketGroup.BracketNodeSystem.GetBracketNodesInTier(0);
            List<BracketNode> semifinalTier = bracketGroup.BracketNodeSystem.GetBracketNodesInTier(1);
            List<BracketNode> quarterfinalTier = bracketGroup.BracketNodeSystem.GetBracketNodesInTier(2);

            finalTier.Count.Should().Be(1);
            finalTier[0].Match.Should().Be(bracketGroup.Matches[5]);

            semifinalTier.Count.Should().Be(2);
            semifinalTier[0].Match.Should().Be(bracketGroup.Matches[4]);
            semifinalTier[1].Match.Should().Be(bracketGroup.Matches[3]);

            quarterfinalTier.Count.Should().Be(3);
            quarterfinalTier[0].Match.Should().Be(bracketGroup.Matches[2]);
            quarterfinalTier[1].Match.Should().Be(bracketGroup.Matches[1]);
            quarterfinalTier[2].Match.Should().Be(bracketGroup.Matches[0]);
        }

        [Fact]
        public void StartDateTimeOnMatchesWithinATierDoesNotHaveToBeInOrder()
        {
            BracketGroup bracketGroup = bracketRound.AddGroup() as BracketGroup;

            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };

            foreach (string playerName in playerNames)
            {
                bracketGroup.AddNewPlayerReference(playerName);
            }

            List<BracketNode> quarterfinalTier = bracketGroup.BracketNodeSystem.GetBracketNodesInTier(2);

            DateTime twoHoursEarlier = quarterfinalTier[0].Match.StartDateTime.AddHours(-2);
            DateTime threeHoursEarlier = quarterfinalTier[1].Match.StartDateTime.AddHours(-3);
            DateTime oneHourEarlier = quarterfinalTier[2].Match.StartDateTime.AddHours(-1);
            DateTime twoHoursLater = quarterfinalTier[3].Match.StartDateTime.AddHours(2);

            quarterfinalTier[0].Match.SetStartDateTime(twoHoursEarlier);
            quarterfinalTier[1].Match.SetStartDateTime(threeHoursEarlier);
            quarterfinalTier[2].Match.SetStartDateTime(oneHourEarlier);
            quarterfinalTier[3].Match.SetStartDateTime(twoHoursLater);

            quarterfinalTier[0].Match.StartDateTime.Should().Be(twoHoursEarlier);
            quarterfinalTier[1].Match.StartDateTime.Should().Be(threeHoursEarlier);
            quarterfinalTier[2].Match.StartDateTime.Should().Be(oneHourEarlier);
            quarterfinalTier[3].Match.StartDateTime.Should().Be(twoHoursLater);
        }

        [Fact]
        public void StartDateTimeForMatchesInACertainMatchTierMustAlwaysBeLaterThanLatestStartDateTimeOfPreviousTier()
        {
            BracketGroup bracketGroup = bracketRound.AddGroup() as BracketGroup;

            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };

            foreach (string playerName in playerNames)
            {
                bracketGroup.AddNewPlayerReference(playerName);
            }

            List<BracketNode> finalTier = bracketGroup.BracketNodeSystem.GetBracketNodesInTier(0);
            List<BracketNode> semifinalTier = bracketGroup.BracketNodeSystem.GetBracketNodesInTier(1);
            List<BracketNode> quarterfinalTier = bracketGroup.BracketNodeSystem.GetBracketNodesInTier(2);

            DateTime finalStartDateTimeBeforeChange = finalTier[0].Match.StartDateTime;
            DateTime quarterfinalStartDateTimeBeforeChange = quarterfinalTier[0].Match.StartDateTime;

            finalTier[0].Match.SetStartDateTime(semifinalTier[0].Match.StartDateTime.AddHours(-4));
            quarterfinalTier[0].Match.SetStartDateTime(semifinalTier[0].Match.StartDateTime.AddHours(4));

            finalTier[0].Match.StartDateTime.Should().Be(finalStartDateTimeBeforeChange);
            quarterfinalTier[0].Match.StartDateTime.Should().Be(quarterfinalStartDateTimeBeforeChange);
        }

        private void ValidateBracketNodeConnections(BracketNode bracketNode, Match correctParentMatch, Match correctChildMatch1, Match correctChildMatch2)
        {
            ValidateBracketNodeMatch(bracketNode.Parent, correctParentMatch);
            ValidateBracketNodeMatch(bracketNode.Children[0], correctChildMatch1);
            ValidateBracketNodeMatch(bracketNode.Children[1], correctChildMatch2);
        }

        private void ValidateBracketNodeMatch(BracketNode bracketNode, Match correctMatch)
        {
            if (correctMatch == null)
            {
                bracketNode.Should().BeNull();
            }
            else
            {
                bracketNode.Match.Should().Be(correctMatch);
            }
        }
    }
}