using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Domain.Utilities;
using Slask.Persistence.Services;
using Slask.TestCore;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.PersistenceTests
{
    [Binding, Scope(Feature = "FetchTest")]
    public class FetchTestSteps : FetchTestStepDefinitions
    {

    }

    public class FetchTestStepDefinitions : TournamentServiceStepDefinitions
    {
        [Then(@"fetched tournament (.*) should contain rounds")]
        public void ThenFetchedTournamentShouldContainRounds(int fetchedTournamentIndex, Table table)
        {
            Tournament tournament;
            using (TournamentService tournamentService = CreateTournamentService())
            {
                tournament = tournamentService.GetTournamentByName("Homestory Cup XX");
            }

            for (int index = 0; index < table.Rows.Count; ++index)
            {
                TestUtilities.ParseRoundTable(table.Rows[index], out string roundType, out _, out _, out _);

                if (roundType.Length > 0)
                {
                    roundType = TestUtilities.ParseRoundGroupTypeString(roundType);
                    RoundBase round = tournament.Rounds[index];

                    if (roundType == "BRACKET")
                    {
                        round.ContestType.Should().Be(ContestTypeEnum.Bracket);

                        (round is BracketRound).Should().BeTrue();
                        (round is DualTournamentRound).Should().BeFalse();
                        (round is RoundRobinRound).Should().BeFalse();
                    }
                    else if (roundType == "DUALTOURNAMENT")
                    {
                        round.ContestType.Should().Be(ContestTypeEnum.DualTournament);

                        (round is BracketRound).Should().BeFalse();
                        (round is DualTournamentRound).Should().BeTrue();
                        (round is RoundRobinRound).Should().BeFalse();
                    }
                    else if (roundType == "ROUNDROBIN")
                    {
                        round.ContestType.Should().Be(ContestTypeEnum.RoundRobin);

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
            Tournament tournament;
            using (TournamentService tournamentService = CreateTournamentService())
            {
                tournament = tournamentService.GetTournamentByName("Homestory Cup XX");
            }

            RoundBase round = tournament.Rounds[roundIndex];
            groupType = TestUtilities.ParseRoundGroupTypeString(groupType);

            if (groupType == "BRACKET")
            {
                foreach (GroupBase group in round.Groups)
                {
                    group.ContestType.Should().Be(ContestTypeEnum.Bracket);

                    (group is BracketGroup).Should().BeTrue();
                    (group is DualTournamentGroup).Should().BeFalse();
                    (group is RoundRobinGroup).Should().BeFalse();
                }
            }
            else if (groupType == "DUALTOURNAMENT")
            {
                foreach (GroupBase group in round.Groups)
                {
                    group.ContestType.Should().Be(ContestTypeEnum.DualTournament);

                    (group is BracketGroup).Should().BeFalse();
                    (group is DualTournamentGroup).Should().BeTrue();
                    (group is RoundRobinGroup).Should().BeFalse();
                }
            }
            else if (groupType == "ROUNDROBIN")
            {
                foreach (GroupBase group in round.Groups)
                {
                    group.ContestType.Should().Be(ContestTypeEnum.RoundRobin);

                    (group is BracketGroup).Should().BeFalse();
                    (group is DualTournamentGroup).Should().BeFalse();
                    (group is RoundRobinGroup).Should().BeTrue();
                }
            }
        }
    }
}
