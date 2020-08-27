using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
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

        [Given(@"matches in tournament named ""(.*)"" changes best of setting")]
        [When(@"matches in tournament named ""(.*)"" changes best of setting")]
        public void WhenMatchesInTournamentNamedChangesBestOfSetting(string tournamentName, Table table)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                foreach(TableRow row in table.Rows)
                {
                    ParseMatchSelection(row, out int roundIndex, out int groupIndex, out int matchIndex, out int bestOf);

                    RoundBase roundBase = tournament.Rounds[roundIndex];
                    GroupBase groupBase = roundBase.Groups[groupIndex];
                    Match match = groupBase.Matches[matchIndex];

                    bool setResult = tournamentService.SetBestOfInMatch(match, bestOf);

                    setResult.Should().BeTrue();
                }

                tournamentService.Save();
            }
        }

        [Then(@"matches in tournament named ""(.*)"" should be set to")]
        public void ThenMatchesInTournamentNamedShouldBeSetTo(string tournamentName, Table table)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                foreach (TableRow row in table.Rows)
                {
                    ParseMatchSelection(row, out int roundIndex, out int groupIndex, out int matchIndex, out int bestOf);

                    RoundBase roundBase = tournament.Rounds[roundIndex];
                    GroupBase groupBase = roundBase.Groups[groupIndex];
                    Match match = groupBase.Matches[matchIndex];

                    match.BestOf.Should().Be(bestOf);
                }
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

        public static void ParseMatchSelection(TableRow row, out int roundIndex, out int groupIndex, out int matchIndex, out int bestOf)
        {
            roundIndex = -1;
            groupIndex = -1;
            matchIndex = -1;
            bestOf = -1;

            if (row.ContainsKey("Round index"))
            {
                int.TryParse(row["Round index"], out roundIndex);
            }

            if (row.ContainsKey("Group index"))
            {
                int.TryParse(row["Group index"], out groupIndex);
            }

            if (row.ContainsKey("Match index"))
            {
                int.TryParse(row["Match index"], out matchIndex);
            }

            if (row.ContainsKey("Best of"))
            {
                int.TryParse(row["Best of"], out bestOf);
            }
        }
    }
}
