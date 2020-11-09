using FluentAssertions;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Rounds.RoundTypes;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Domain.Xunit.UnitTests.GroupTests
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
            dualTournamenGroup.RoundId.Should().Be(dualTournamentRound.Id);
            dualTournamenGroup.Round.Should().Be(dualTournamentRound);
        }

        [Fact]
        public void CanConstructDualTournamentMatchLayout()
        {
            List<string> playerNames = new List<string> { "Maru", "Stork", "Taeja", "Rain" };

            RegisterPlayers(playerNames);

            DualTournamentGroup dualTournamentGroup = dualTournamentRound.Groups.First() as DualTournamentGroup;

            dualTournamentGroup.Matches.Should().HaveCount(5);

            dualTournamentGroup.Matches[0].GetPlayer1Name().Should().Be(playerNames[0]);
            dualTournamentGroup.Matches[0].GetPlayer2Name().Should().Be(playerNames[1]);

            dualTournamentGroup.Matches[1].GetPlayer1Name().Should().Be(playerNames[2]);
            dualTournamentGroup.Matches[1].GetPlayer2Name().Should().Be(playerNames[3]);

            dualTournamentGroup.Matches[2].PlayerReference1Id.Should().BeEmpty();
            dualTournamentGroup.Matches[2].PlayerReference2Id.Should().BeEmpty();

            dualTournamentGroup.Matches[3].PlayerReference1Id.Should().BeEmpty();
            dualTournamentGroup.Matches[3].PlayerReference2Id.Should().BeEmpty();

            dualTournamentGroup.Matches[4].PlayerReference1Id.Should().BeEmpty();
            dualTournamentGroup.Matches[4].PlayerReference2Id.Should().BeEmpty();
        }

        [Fact]
        public void CannotAddMoreThanFourPlayerReferencesToGroup()
        {
            List<string> playerNames = new List<string> { "Maru", "Stork", "Taeja", "Rain", "Bomber" };
            RegisterPlayers(playerNames);

            DualTournamentGroup dualTournamentGroup = dualTournamentRound.Groups.First() as DualTournamentGroup;
            List<PlayerReference> playerReferences = dualTournamentGroup.GetPlayerReferences();

            playerReferences.Single(playerReference => playerReference.Name == playerNames[0]).Should().NotBeNull();
            playerReferences.Single(playerReference => playerReference.Name == playerNames[1]).Should().NotBeNull();
            playerReferences.Single(playerReference => playerReference.Name == playerNames[2]).Should().NotBeNull();
            playerReferences.Single(playerReference => playerReference.Name == playerNames[3]).Should().NotBeNull();
            playerReferences.SingleOrDefault(playerReference => playerReference.Name == playerNames[4]).Should().BeNull();

            dualTournamentGroup.Matches.Should().HaveCount(5);

            dualTournamentGroup.Matches[0].GetPlayer1Name().Should().NotBeNullOrEmpty();
            dualTournamentGroup.Matches[0].GetPlayer2Name().Should().NotBeNullOrEmpty();

            dualTournamentGroup.Matches[1].GetPlayer1Name().Should().NotBeNullOrEmpty();
            dualTournamentGroup.Matches[1].GetPlayer2Name().Should().NotBeNullOrEmpty();

            dualTournamentGroup.Matches[2].PlayerReference1Id.Should().BeEmpty();
            dualTournamentGroup.Matches[2].PlayerReference2Id.Should().BeEmpty();

            dualTournamentGroup.Matches[3].PlayerReference1Id.Should().BeEmpty();
            dualTournamentGroup.Matches[3].PlayerReference2Id.Should().BeEmpty();

            dualTournamentGroup.Matches[4].PlayerReference1Id.Should().BeEmpty();
            dualTournamentGroup.Matches[4].PlayerReference2Id.Should().BeEmpty();
        }

        private void RegisterPlayers(List<string> playerNames)
        {
            foreach (string playerName in playerNames)
            {
                tournament.RegisterPlayerReference(playerName);
            }
        }
    }
}
