using FluentAssertions;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds.RoundTypes;
using System.Linq;
using Xunit;

namespace Slask.Domain.Xunit.UnitTests
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
            PlayerReference maruPlayerReference = tournament.RegisterPlayerReference("Maru");
            PlayerReference storkPlayerReference = tournament.RegisterPlayerReference("Stork");
            PlayerReference taejaPlayerReference = tournament.RegisterPlayerReference("Taeja");

            BracketGroup group = bracketRound.Groups.First() as BracketGroup;

            Match firstMatch = group.Matches[0];
            Match secondMatch = group.Matches[1];

            bool result = PlayerSwitcher.SwitchMatchesOn(firstMatch.Player1, secondMatch.Player2);

            result.Should().BeFalse();
            firstMatch.Player1.PlayerReferenceId.Should().Be(maruPlayerReference.Id);
            firstMatch.Player2.PlayerReferenceId.Should().Be(storkPlayerReference.Id);
            secondMatch.Player1.PlayerReferenceId.Should().Be(taejaPlayerReference.Id);
            secondMatch.Player2.PlayerReferenceId.Should().BeEmpty();
        }
    }
}
