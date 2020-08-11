using FluentAssertions;
using Slask.Domain.Rounds.RoundTypes;
using Xunit;

namespace Slask.Domain.Xunit.IntegrationTests
{
    public class PlayerReferenceTests
    {
        private readonly Tournament tournament;
        private readonly RoundRobinRound round;

        public PlayerReferenceTests()
        {
            tournament = Tournament.Create("GSL 2019");
            round = tournament.AddRoundRobinRound();
        }

        [Fact]
        public void PlayerReferenceCanBeRenamed()
        {
            PlayerReference playerReference = round.RegisterPlayerReference("Maru");

            playerReference.RenameTo("Idra");

            playerReference.Name.Should().Be("Idra");
        }

        [Fact]
        public void PlayerReferenceCannotBeRenamedToEmptyName()
        {
            PlayerReference playerReference = round.RegisterPlayerReference("Maru");

            playerReference.RenameTo("");

            playerReference.Name.Should().Be("Maru");
        }

        [Fact]
        public void CanCreatePlayerReferencesWithNamesThatOnlyDifferInLetterCasing()
        {
            string playerName1 = "HerO";
            string playerName2 = "herO";

            PlayerReference playerReference1 = round.RegisterPlayerReference(playerName1);
            PlayerReference playerReference2 = round.RegisterPlayerReference(playerName2);

            playerReference1.Name.Should().Be(playerName1);
            playerReference2.Name.Should().Be(playerName2);
        }

        [Fact]
        public void PlayerReferenceCanBeRenamedToNameThatOnlyDiffersInLetterCasing()
        {
            string playerName1 = "HerO";
            string playerName2 = "herO";

            PlayerReference playerReference1 = round.RegisterPlayerReference(playerName1);
            PlayerReference playerReference2 = round.RegisterPlayerReference("Maru");

            playerReference2.RenameTo(playerName2);

            playerReference1.Name.Should().Be(playerName1);
            playerReference2.Name.Should().Be(playerName2);
        }

        [Fact]
        public void PlayerReferenceCannotBeRenamedToNameThatIsExactlyTheSameAsOtherPlayerReference()
        {
            string playerName1 = "Maru";
            string playerName2 = "Idra";

            PlayerReference playerReference1 = round.RegisterPlayerReference(playerName1);
            PlayerReference playerReference2 = round.RegisterPlayerReference(playerName2);

            playerReference2.RenameTo(playerName1);

            playerReference1.Name.Should().Be(playerName1);
            playerReference2.Name.Should().Be(playerName2);
        }

        [Fact]
        public void PlayerReferenceCannotBeRenamedToNameThatOnlyDiffersInWhitespaceToOtherPlayerReference()
        {
            string playerName1 = "Maru";
            string playerName2 = "Idra";

            PlayerReference playerReference1 = round.RegisterPlayerReference(playerName1);
            PlayerReference playerReference2 = round.RegisterPlayerReference(playerName2);

            playerReference2.RenameTo(playerName1 + " ");

            playerReference1.Name.Should().Be(playerName1);
            playerReference2.Name.Should().Be(playerName2);
        }
    }
}
