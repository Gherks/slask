using Slask.Domain;
using Slask.TestCore;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class PlayerReferenceTests
    {

        [Fact]
        public void PlayerCanBeRenamed()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Player player = tournament.Rounds.First().Groups.First().Matches.First().Player1.;

            player.RenameTo("Taeja");

            player.Name.Should().Be("Taeja");
        }

        [Fact]
        public void PlayerCannotBeRenamedToEmptyName()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.Rounds.First().Groups.First().Matches.First();

            match.Player1.RenameTo("");

            match.Player1.Name.Should().Be("Maru");
        }

        [Fact]
        public void PlayerCannotBeRenamedToSameNameAsOpponentNoMatterLetterCasing()
        {
            TournamentServiceContext services = GivenServices();
            Tournament tournament = services.WhenCreatedMatchesInRoundRobinRoundInTournament();
            Match match = tournament.Rounds.First().Groups.First().Matches.First();

            match.Player1.RenameTo(match.Player2.Name.ToUpper());

            match.Player1.Name.Should().Be("Maru");
        }

        private TournamentServiceContext GivenServices()
        {
            return TournamentServiceContext.GivenServices(new UnitTestSlaskContextCreator());
        }
    }
}
