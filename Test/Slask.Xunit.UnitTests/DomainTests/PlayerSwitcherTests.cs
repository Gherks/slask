using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds.RoundTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Xunit.UnitTests.DomainTests
{
    public class PlayerSwitcherTests
    {
        private readonly Tournament tournament;
        private readonly BracketRound bracketRound;

        public PlayerSwitcherTests()
        {
            tournament = Tournament.Create("GSL 2019");
            bracketRound = tournament.AddBracketRound() as BracketRound;
        }

        [Fact]
        public void CannotSwitchPlacesOnPlayerReferenceWhenAnyPlayerIsNull()
        {
            bracketRound.SetPlayersPerGroupCount(4);
            PlayerReference maruPlayerReference = bracketRound.RegisterPlayerReference("Maru");
            PlayerReference storkPlayerReference = bracketRound.RegisterPlayerReference("Stork");
            PlayerReference taejaPlayerReference = bracketRound.RegisterPlayerReference("Taeja");

            BracketGroup group = bracketRound.Groups.First() as BracketGroup;

            Match firstMatch = group.Matches[0];
            Match secondMatch = group.Matches[1];

            bool result = PlayerSwitcher.SwitchMatchesOn(firstMatch.Player1, secondMatch.Player2);

            result.Should().BeFalse();
            firstMatch.Player1.PlayerReference.Should().Be(maruPlayerReference);
            firstMatch.Player2.PlayerReference.Should().Be(storkPlayerReference);
            secondMatch.Player1.PlayerReference.Should().Be(taejaPlayerReference);
            secondMatch.Player2.Should().Be(null);
        }
    }
}
