using Slask.Domain;
using Slask.Domain.Groups;
using System;
using System.Collections.Generic;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class PlayerSwitcherTests
    {
        public PlayerSwitcherTests()
        {

        }

        //[Fact]
        //public void CanSwitchPlacesOnPlayerReferencesThatAreInSameMatch()
        //{
        //    RegisterFirstTwoPlayers();

        //    PlayerSwitcher.SwitchMatchesOn(bracketGroup.Matches.First().Player1, bracketGroup.Matches.First().Player2);

        //    bracketGroup.Matches.First().Player1.PlayerReference.Should().Be(secondPlayerReference);
        //    bracketGroup.Matches.First().Player2.PlayerReference.Should().Be(firstPlayerReference);
        //}

        //[Fact]
        //public void CanSwitchPlacesOnPlayerReferencesThatAreInSameGroup()
        //{
        //    BracketGroup group = bracketRound.AddGroup() as BracketGroup;

        //    PlayerReference firstPlayerReference = group.AddNewPlayerReference("Maru");
        //    PlayerReference secondPlayerReference = group.AddNewPlayerReference("Stork");
        //    PlayerReference thirdPlayerReference = group.AddNewPlayerReference("Taeja");
        //    PlayerReference fourthPlayerReference = group.AddNewPlayerReference("Rain");

        //    Match firstMatch = group.Matches[0];
        //    Match secondMatch = group.Matches[1];

        //    PlayerSwitcher.SwitchMatchesOn(firstMatch.Player1, secondMatch.Player2);

        //    firstMatch.Player1.PlayerReference.Should().Be(fourthPlayerReference);
        //    firstMatch.Player2.PlayerReference.Should().Be(secondPlayerReference);
        //    secondMatch.Player1.PlayerReference.Should().Be(thirdPlayerReference);
        //    secondMatch.Player2.PlayerReference.Should().Be(firstPlayerReference);
        //}

        //[Fact]
        //public void CanSwitchPlacesOnPlayerReferencesThatAreInDifferentGroups()
        //{
        //    BracketGroup group = bracketRound.AddGroup() as BracketGroup;

        //    PlayerReference firstPlayerReference = group.AddNewPlayerReference("Maru");
        //    PlayerReference secondPlayerReference = group.AddNewPlayerReference("Stork");
        //    PlayerReference thirdPlayerReference = group.AddNewPlayerReference("Taeja");
        //    PlayerReference fourthPlayerReference = group.AddNewPlayerReference("Rain");

        //    Match firstMatch = group.Matches[0];
        //    Match secondMatch = group.Matches[1];

        //    PlayerSwitcher.SwitchMatchesOn(firstMatch.Player1, secondMatch.Player2);

        //    firstMatch.Player1.PlayerReference.Should().Be(fourthPlayerReference);
        //    firstMatch.Player2.PlayerReference.Should().Be(secondPlayerReference);
        //    secondMatch.Player1.PlayerReference.Should().Be(thirdPlayerReference);
        //    secondMatch.Player2.PlayerReference.Should().Be(firstPlayerReference);
        //}

        //[Fact]
        //public void CannotSwitchPlacesOnPlayerReferenceWhenAnyPlayerIsNull()
        //{
        //    BracketGroup group = bracketRound.AddGroup() as BracketGroup;

        //    PlayerReference firstPlayerReference = group.AddNewPlayerReference("Maru");
        //    PlayerReference secondPlayerReference = group.AddNewPlayerReference("Stork");
        //    PlayerReference thirdPlayerReference = group.AddNewPlayerReference("Taeja");
        //    PlayerReference fourthPlayerReference = group.AddNewPlayerReference("Rain");

        //    Match firstMatch = group.Matches[0];
        //    Match secondMatch = group.Matches[1];
        //    Match thirdMatch = group.Matches[2];

        //    PlayerSwitcher.SwitchMatchesOn(thirdMatch.Player1, secondMatch.Player2);
        //    PlayerSwitcher.SwitchMatchesOn(firstMatch.Player1, thirdMatch.Player2);

        //    firstMatch.Player1.PlayerReference.Should().Be(firstPlayerReference);
        //    firstMatch.Player2.PlayerReference.Should().Be(secondPlayerReference);
        //    secondMatch.Player1.PlayerReference.Should().Be(thirdPlayerReference);
        //    secondMatch.Player2.PlayerReference.Should().Be(fourthPlayerReference);
        //}


        //[Fact]
        //public void CannotSwitchPlacesOnAnyPlayerReferencesWhenAMatchInGroupHasBegun()
        //{
        //    PlayerReference thirdPlayerReference = bracketRound.RegisterPlayerReference("Taeja");
        //    PlayerReference fourthPlayerReference = bracketRound.RegisterPlayerReference("Rain");

        //    Match firstMatch = bracketGroup.Matches[0];
        //    Match secondMatch = group.Matches[1];

        //    SystemTimeMocker.SetOneSecondAfter(firstMatch.StartDateTime);

        //    PlayerSwitcher.SwitchMatchesOn(firstMatch.Player1, secondMatch.Player2);

        //    firstMatch.Player1.PlayerReference.Should().Be(firstPlayerReference);
        //    firstMatch.Player2.PlayerReference.Should().Be(secondPlayerReference);
        //    secondMatch.Player1.PlayerReference.Should().Be(thirdPlayerReference);
        //    secondMatch.Player2.PlayerReference.Should().Be(fourthPlayerReference);
        //}


        //[Fact]
        //public void CannotSwitchPlacesOnAnyPlayerReferencesWithinRoundRobinGroup()
        //{
    }
}
