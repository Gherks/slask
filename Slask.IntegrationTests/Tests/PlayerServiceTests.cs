using Xunit;

namespace Slask.IntegrationTests
{
    [Collection("Integration test collection")]
    public class PlayerServiceTests
    {
        [Fact]
        public void CanCreatePlayer()
        {
            //using (SlaskContext slaskContext = CreateSlaskTestContext())
            //{
            //    var playerService = new PlayerService(slaskContext);

            //    Player player = playerService.AddPlayer("Maru");
            //    slaskContext.SaveChanges();

            //    player.Id.Should().NotBe(Guid.Empty);
            //    player.Name.Should().Be("Maru");
            //}
        }
    }
}
