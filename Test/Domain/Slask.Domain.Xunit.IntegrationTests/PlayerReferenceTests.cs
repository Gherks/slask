using FluentAssertions;
using Xunit;

namespace Slask.Domain.Xunit.IntegrationTests
{
    public class PlayerReferenceTests
    {
        private readonly Tournament tournament;

        public PlayerReferenceTests()
        {
            tournament = Tournament.Create("GSL 2019");
            tournament.AddRoundRobinRound();
        }

        [Fact]
        public void PlayerReferenceCanBeRenamed()
        {
            PlayerReference playerReference = tournament.RegisterPlayerReference("Maru");

            playerReference.RenameTo("Idra");

            playerReference.Name.Should().Be("Idra");
        }

        [Fact]
        public void PlayerReferenceCannotBeRenamedToEmptyName()
        {
            PlayerReference playerReference = tournament.RegisterPlayerReference("Maru");

            playerReference.RenameTo("");

            playerReference.Name.Should().Be("Maru");
        }

        [Fact]
        public void CanCreatePlayerReferencesWithNamesThatOnlyDifferInLetterCasing()
        {
            string playerName1 = "HerO";
            string playerName2 = "herO";

            PlayerReference playerReference1 = tournament.RegisterPlayerReference(playerName1);
            PlayerReference playerReference2 = tournament.RegisterPlayerReference(playerName2);

            playerReference1.Name.Should().Be(playerName1);
            playerReference2.Name.Should().Be(playerName2);
        }

        [Fact]
        public void PlayerReferenceCanBeRenamedToNameThatOnlyDiffersInLetterCasing()
        {
            string playerName1 = "HerO";
            string playerName2 = "herO";

            PlayerReference playerReference1 = tournament.RegisterPlayerReference(playerName1);
            PlayerReference playerReference2 = tournament.RegisterPlayerReference("Maru");

            playerReference2.RenameTo(playerName2);

            playerReference1.Name.Should().Be(playerName1);
            playerReference2.Name.Should().Be(playerName2);
        }

        [Fact]
        public void PlayerReferenceCannotBeRenamedToNameThatIsExactlyTheSameAsOtherPlayerReference()
        {
            string playerName1 = "Maru";
            string playerName2 = "Idra";

            PlayerReference playerReference1 = tournament.RegisterPlayerReference(playerName1);
            PlayerReference playerReference2 = tournament.RegisterPlayerReference(playerName2);

            playerReference2.RenameTo(playerName1);

            playerReference1.Name.Should().Be(playerName1);
            playerReference2.Name.Should().Be(playerName2);
        }

        [Fact]
        public void PlayerReferenceCannotBeRenamedToNameThatOnlyDiffersInWhitespaceToOtherPlayerReference()
        {
            string playerName1 = "Maru";
            string playerName2 = "Idra";

            PlayerReference playerReference1 = tournament.RegisterPlayerReference(playerName1);
            PlayerReference playerReference2 = tournament.RegisterPlayerReference(playerName2);

            playerReference2.RenameTo(playerName1 + " ");

            playerReference1.Name.Should().Be(playerName1);
            playerReference2.Name.Should().Be(playerName2);
        }
    }
}
