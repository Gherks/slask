using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using Slask.Persistence.Services;
using Slask.SpecFlow.IntegrationTests.PersistenceTests;
using Slask.TestCore;
using TechTalk.SpecFlow;

namespace Slask.Persistence.Specflow.IntegrationTests
{
    [Binding, Scope(Feature = "ChangeTournamentSettings")]
    public class ChangeTournamentSettings : PersistenceSteps
    {

        [Given(@"round named ""(.*)"" is removed from tournament named ""(.*)""")]
        [When(@"round named ""(.*)"" is removed from tournament named ""(.*)""")]
        public void GivenRoundNamedIsRemovedFromTournamentNamed(string roundName, string tournamentName)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                tournamentService.RemoveRoundFromTournament(tournament, roundName);
                tournamentService.Save();
            }
        }



        [Given(@"round named ""(.*)"" changes advancing players per group count to ""(.*)"" in tournament named ""(.*)""")]
        [When(@"round named ""(.*)"" changes advancing players per group count to ""(.*)"" in tournament named ""(.*)""")]
        public void GivenRoundNamedChangesAdvancingPlayersPerGroupCountToInTournamentNamed(string roundName, int newAdvancingPlayerCount, string tournamentName)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                RoundBase round = tournament.GetRoundByRoundName(roundName);

                tournamentService.SetAdvancingPerGroupCountInRound(round, newAdvancingPlayerCount);
                tournamentService.Save();
            }
        }

        [Given(@"round named ""(.*)"" changes players per group count to ""(.*)"" in tournament named ""(.*)""")]
        [When(@"round named ""(.*)"" changes players per group count to ""(.*)"" in tournament named ""(.*)""")]
        public void GivenRoundNamedChangesPlayersPerGroupCountToInTournamentNamed(string roundName, int newPlayersPerGroupCount, string tournamentName)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                RoundBase round = tournament.GetRoundByRoundName(roundName);

                tournamentService.SetPlayersPerGroupCountInRound(round, newPlayersPerGroupCount);
                tournamentService.Save();
            }
        }

        [Then(@"round layout in tournament named ""(.*)"" is exactly as follows:")]
        public void ThenRoundLayoutInTournamentNamedIsExactlyAsFollows(string tournamentName, Table table)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                tournament.Rounds.Should().HaveCount(table.Rows.Count);

                for (int rowIndex = 0; rowIndex < table.Rows.Count; ++rowIndex)
                {
                    TableRow row = table.Rows[rowIndex];
                    RoundBase round = tournament.Rounds[rowIndex];

                    TestUtilities.ParseRoundTable(row, out ContestTypeEnum contestType, out string name, out int advancingCount, out int playersPerGroupCount);

                    round.ContestType.Should().Be(contestType);
                    round.Name.Should().Be(name);
                    round.AdvancingPerGroupCount.Should().Be(advancingCount);
                    round.PlayersPerGroupCount.Should().Be(playersPerGroupCount);
                }
            }
        }
    }
}
