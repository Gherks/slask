using FluentAssertions;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Domain.Xunit.IntegrationTests
{
    public class TournamentTests
    {
        private readonly Tournament tournament;

        public TournamentTests()
        {
            tournament = Tournament.Create("GSL 2019");
        }

        [Fact]
        public void CanRegisterPlayerReferencesToTournament()
        {
            string playerName = "Maru";

            RoundRobinRound round = tournament.AddRoundRobinRound();

            PlayerReference playerReference = tournament.RegisterPlayerReference(playerName);

            playerReference.Id.Should().NotBeEmpty();
            playerReference.Name.Should().Be(playerName);
            playerReference.TournamentId.Should().Be(round.TournamentId);
            playerReference.Tournament.Should().Be(round.Tournament);

            tournament.PlayerReferences.First().Should().Be(playerReference);
        }

        [Fact]
        public void CanExcludePlayerReferencesFromTournament()
        {
            string playerName = "Maru";

            RoundRobinRound round = tournament.AddRoundRobinRound();

            tournament.RegisterPlayerReference(playerName);
            bool exclusionResult = tournament.ExcludePlayerReference(playerName);

            exclusionResult.Should().BeTrue();
            tournament.PlayerReferences.Should().BeEmpty();
        }
    }
}
