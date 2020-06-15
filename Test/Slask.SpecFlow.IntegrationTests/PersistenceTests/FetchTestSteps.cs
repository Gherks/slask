using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using Slask.SpecFlow.IntegrationTests.DomainTests;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.PersistenceTests
{
    [Binding, Scope(Feature = "FetchTest")]
    public class FetchTestSteps : FetchTestStepDefinitions
    {

    }

    public class FetchTestStepDefinitions : BetterStepDefinitions
    {
        [Then(@"fetched tournament (.*) should contain rounds")]
        public void ThenFetchedTournamentShouldContainRounds(int fetchedTournamentIndex, Table table)
        {
            Tournament fetchedTournament = fetchedTournaments[fetchedTournamentIndex];

            for (int index = 0; index < table.Rows.Count; ++index)
            {
                ParseRoundTable(table.Rows[index], out string roundType, out string name, out int advancingCount, out int playersPerGroupCount);

                if (roundType.Length > 0)
                {
                    roundType = ParseRoundGroupTypeString(roundType);
                    RoundBase round = fetchedTournament.Rounds[index];

                    if (roundType == "BRACKET")
                    {
                        (round is BracketRound).Should().BeTrue();
                        (round is DualTournamentRound).Should().BeFalse();
                        (round is RoundRobinRound).Should().BeFalse();
                    }
                    else if (roundType == "DUALTOURNAMENT")
                    {
                        (round is BracketRound).Should().BeFalse();
                        (round is DualTournamentRound).Should().BeTrue();
                        (round is RoundRobinRound).Should().BeFalse();
                    }
                    else if (roundType == "ROUNDROBIN")
                    {
                        (round is BracketRound).Should().BeFalse();
                        (round is DualTournamentRound).Should().BeFalse();
                        (round is RoundRobinRound).Should().BeTrue();
                    }
                }
            }
        }

        [Then(@"groups within round (.*) in fetched tournament (.*) is of type ""(.*)""")]
        public void ThenGroupsWithinRoundInFetchedTournamentIsOfType(int roundIndex, int fetchedTournamentIndex, string groupType)
        {
            Tournament fetchedTournament = fetchedTournaments[fetchedTournamentIndex];
            RoundBase round = fetchedTournament.Rounds[roundIndex];
            groupType = ParseRoundGroupTypeString(groupType);

            if (groupType == "BRACKET")
            {
                foreach (GroupBase group in round.Groups)
                {
                    (group is BracketGroup).Should().BeTrue();
                    (group is DualTournamentGroup).Should().BeFalse();
                    (group is RoundRobinGroup).Should().BeFalse();
                }
            }
            else if (groupType == "DUALTOURNAMENT")
            {
                foreach (GroupBase group in round.Groups)
                {
                    (group is BracketGroup).Should().BeFalse();
                    (group is DualTournamentGroup).Should().BeTrue();
                    (group is RoundRobinGroup).Should().BeFalse();
                }
            }
            else if (groupType == "ROUNDROBIN")
            {
                foreach (GroupBase group in round.Groups)
                {
                    (group is BracketGroup).Should().BeFalse();
                    (group is DualTournamentGroup).Should().BeFalse();
                    (group is RoundRobinGroup).Should().BeTrue();
                }
            }
        }
    }
}
