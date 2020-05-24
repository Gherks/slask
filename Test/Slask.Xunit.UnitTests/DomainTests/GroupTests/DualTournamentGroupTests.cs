using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Rounds.RoundTypes;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Xunit.UnitTests.DomainTests.GroupTests
{
    public class DualTournamentGroupTests
    {
        private readonly Tournament tournament;
        private readonly DualTournamentRound dualTournamentRound;

        public DualTournamentGroupTests()
        {
            tournament = Tournament.Create("GSL 2019");
            dualTournamentRound = tournament.AddDualTournamentRound() as DualTournamentRound;
        }

        [Fact]
        public void CanCreateGroup()
        {
            DualTournamentGroup dualTournamenGroup = DualTournamentGroup.Create(dualTournamentRound);

            dualTournamenGroup.Should().NotBeNull();
            dualTournamenGroup.Id.Should().NotBeEmpty();
            dualTournamenGroup.Matches.Should().BeEmpty();
            dualTournamenGroup.PlayerReferences.Should().BeEmpty();
            dualTournamenGroup.RoundId.Should().Be(dualTournamentRound.Id);
            dualTournamenGroup.Round.Should().Be(dualTournamentRound);
        }

        [Fact]
        public void CanConstructDualTournamentMatchLayout()
        {
            List<string> playerNames = new List<string> { "Maru", "Stork", "Taeja", "Rain" };

            DualTournamentGroup dualTournamentGroup = RegisterPlayers(playerNames);

            dualTournamentGroup.Matches.Should().HaveCount(5);

            dualTournamentGroup.Matches[0].Player1.Name.Should().Be(playerNames[0]);
            dualTournamentGroup.Matches[0].Player2.Name.Should().Be(playerNames[1]);

            dualTournamentGroup.Matches[1].Player1.Name.Should().Be(playerNames[2]);
            dualTournamentGroup.Matches[1].Player2.Name.Should().Be(playerNames[3]);

            dualTournamentGroup.Matches[2].Player1.Should().BeNull();
            dualTournamentGroup.Matches[2].Player2.Should().BeNull();

            dualTournamentGroup.Matches[3].Player1.Should().BeNull();
            dualTournamentGroup.Matches[3].Player2.Should().BeNull();

            dualTournamentGroup.Matches[4].Player1.Should().BeNull();
            dualTournamentGroup.Matches[4].Player2.Should().BeNull();
        }

        [Fact]
        public void CannotAddMoreThanFourPlayerReferencesToGroup()
        {
            List<string> playerNames = new List<string> { "Maru", "Stork", "Taeja", "Rain", "Bomber" };

            DualTournamentGroup dualTournamentGroup = RegisterPlayers(playerNames);

            dualTournamentGroup.PlayerReferences.Single(playerReference => playerReference.Name == playerNames[0]).Should().NotBeNull();
            dualTournamentGroup.PlayerReferences.Single(playerReference => playerReference.Name == playerNames[1]).Should().NotBeNull();
            dualTournamentGroup.PlayerReferences.Single(playerReference => playerReference.Name == playerNames[2]).Should().NotBeNull();
            dualTournamentGroup.PlayerReferences.Single(playerReference => playerReference.Name == playerNames[3]).Should().NotBeNull();
            dualTournamentGroup.PlayerReferences.SingleOrDefault(playerReference => playerReference.Name == playerNames[4]).Should().BeNull();

            dualTournamentGroup.Matches.Should().HaveCount(5);

            dualTournamentGroup.Matches[0].Player1.Name.Should().NotBeNull();
            dualTournamentGroup.Matches[0].Player2.Name.Should().NotBeNull();

            dualTournamentGroup.Matches[1].Player1.Name.Should().NotBeNull();
            dualTournamentGroup.Matches[1].Player2.Name.Should().NotBeNull();

            dualTournamentGroup.Matches[2].Player1.Should().BeNull();
            dualTournamentGroup.Matches[2].Player2.Should().BeNull();

            dualTournamentGroup.Matches[3].Player1.Should().BeNull();
            dualTournamentGroup.Matches[3].Player2.Should().BeNull();

            dualTournamentGroup.Matches[4].Player1.Should().BeNull();
            dualTournamentGroup.Matches[4].Player2.Should().BeNull();
        }

        private DualTournamentGroup RegisterPlayers(List<string> playerNames)
        {
            foreach (string playerName in playerNames)
            {
                dualTournamentRound.RegisterPlayerReference(playerName);
            }

            return dualTournamentRound.Groups.First() as DualTournamentGroup;
        }
    }
}
