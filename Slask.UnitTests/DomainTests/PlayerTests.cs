using FluentAssertions;
using Slask.Domain;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class PlayerTests
    {
        [Fact]
        public void PlayerInitiallyHasName()
        {
            Player player = WhenPlayerCreated();

            player.Name.Should().Be("Maru");
        }

        [Fact]
        public void PlayerCanBeRenamed()
        {
            Player player = WhenPlayerCreated();
            player.Rename("Stork");

            player.Name.Should().Be("Stork");
        }

        public Player WhenPlayerCreated()
        {
            return Player.Create("Maru");
        }
    }
}
