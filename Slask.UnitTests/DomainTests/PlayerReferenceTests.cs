using FluentAssertions;
using Slask.Domain;
using Slask.TestCore;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class PlayerReferenceTests
    {
        [Fact]
        public void CanCreatePlayerReference()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup(services);
            PlayerReference playerReference = group.Matches.First().Player1.PlayerReference;

            playerReference.Should().NotBeNull();
            playerReference.Id.Should().NotBeEmpty();
            playerReference.Name.Should().Be("Maru");
            playerReference.TournamentId.Should().Be(group.Round.Tournament.Id);
            playerReference.Tournament.Should().Be(group.Round.Tournament);
        }

        [Fact]
        public void PlayerReferenceCanBeRenamed()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup(services);
            PlayerReference playerReference = group.Matches.First().Player1.PlayerReference;

            playerReference.RenameTo("Idra");

            playerReference.Name.Should().Be("Idra");
        }

        [Fact]
        public void PlayerReferenceCannotBeRenamedToSameAsOtherPlayerReferenceNoMatterLetterCasing()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup(services);
            Match match = group.Matches.First();

            match.Player1.PlayerReference.RenameTo(match.Player2.Name.ToUpper());

            match.Player1.Name.Should().Be("Maru");
        }

        [Fact]
        public void PlayerReferenceCannotBeRenamedToEmptyName()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part05_AddedPlayersToRoundRobinGroup(services);
            PlayerReference playerReference = group.Matches.First().Player1.PlayerReference;

            playerReference.RenameTo("");

            playerReference.Name.Should().Be("Maru");
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
