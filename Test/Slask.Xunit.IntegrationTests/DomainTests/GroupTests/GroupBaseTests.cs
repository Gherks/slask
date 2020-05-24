using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds.RoundTypes;
using Xunit;

namespace Slask.Xunit.IntegrationTests.DomainTests.GroupTests
{
    public class GroupBaseTests
    {
        private readonly Tournament tournament;
        private readonly BracketRound round;

        public GroupBaseTests()
        {
            tournament = Tournament.Create("GSL 2019");
            round = tournament.AddBracketRound();
            round.SetPlayersPerGroupCount(2);
        }

        [Fact]
        public void AddingSeveralRoundsYieldsRoundsWithExpectedNames()
        {
            round.RegisterPlayerReference("Group A participant 1");
            round.RegisterPlayerReference("Group A participant 2");

            round.RegisterPlayerReference("Group B participant 1");
            round.RegisterPlayerReference("Group B participant 2");

            round.RegisterPlayerReference("Group C participant 1");
            round.RegisterPlayerReference("Group C participant 2");

            round.RegisterPlayerReference("Group D participant 1");
            round.RegisterPlayerReference("Group D participant 2");

            round.RegisterPlayerReference("Group E participant 1");
            round.RegisterPlayerReference("Group E participant 2");

            round.Groups[0].Name.Should().Be("Group A");
            round.Groups[1].Name.Should().Be("Group B");
            round.Groups[2].Name.Should().Be("Group C");
            round.Groups[3].Name.Should().Be("Group D");
            round.Groups[4].Name.Should().Be("Group E");
        }

        [Fact]
        public void AddingEnoughRoundsToUseAllSingleLetterRestartsLetteringWithTwoLetters()
        {
            for (int index = 0; index < 60; ++index)
            {
                round.RegisterPlayerReference("Participant" + index.ToString());
            }

            round.Groups[26].Name.Should().Be("Group AA");
            round.Groups[27].Name.Should().Be("Group AB");
            round.Groups[28].Name.Should().Be("Group AC");
            round.Groups[29].Name.Should().Be("Group AD");
        }
    }
}