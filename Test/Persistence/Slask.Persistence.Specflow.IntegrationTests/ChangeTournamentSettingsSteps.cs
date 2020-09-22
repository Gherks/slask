using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using Slask.Persistence.Services;
using Slask.SpecFlow.IntegrationTests.PersistenceTests;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.Persistence.Specflow.IntegrationTests
{
    [Binding, Scope(Feature = "ChangeTournamentSettings")]
    public class ChangeTournamentSettings : PersistenceSteps
    {
        private readonly Dictionary<Guid, DateTime> oldMatchStartTimes;

        public ChangeTournamentSettings()
        {
            oldMatchStartTimes = new Dictionary<Guid, DateTime>();
        }

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

                foreach (TableRow row in table.Rows)
                {
                    ParseMatchBestOfSelection(row, out int roundIndex, out int groupIndex, out int matchIndex, out int bestOf);

                    RoundBase roundBase = tournament.Rounds[roundIndex];
                    GroupBase groupBase = roundBase.Groups[groupIndex];
                    Match match = groupBase.Matches[matchIndex];

                    tournamentService.SetBestOfInMatch(match, bestOf);
                }

                tournamentService.Save();
            }
        }

        [Given(@"player layout for matches in tournament named ""(.*)"" looks like this")]
        [When(@"player layout for matches in tournament named ""(.*)"" looks like this")]
        [Then(@"player layout for matches in tournament named ""(.*)"" looks like this")]
        public void GivenPlayerLayoutForMatchesInTournamentNamedLooksLikeThis(string tournamentName, Table table)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                foreach (TableRow row in table.Rows)
                {
                    ParseTableLayoutRow(row, out int roundIndex, out int groupIndex, out int matchIndex, out int playerIndex, out string playerName);

                    RoundBase roundBase = tournament.Rounds[roundIndex];
                    GroupBase groupBase = roundBase.Groups[groupIndex];
                    Match match = groupBase.Matches[matchIndex];
                    Player player = playerIndex == 0 ? match.Player1 : match.Player2;

                    player.GetName().Should().Be(playerName);
                }
            }
        }

        [When(@"move start time three hours forward for matches in tournament named ""(.*)""")]
        public void WhenMoveStartTimeThreeHoursForwardForMatchesInTournamentNamed(string tournamentName, Table table)
        {
            const int addedHours = 3;

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                foreach (TableRow row in table.Rows)
                {
                    ParseMatchBestOfSelection(row, out int roundIndex, out int groupIndex, out int matchIndex, out int _);

                    RoundBase roundBase = tournament.Rounds[roundIndex];
                    GroupBase groupBase = roundBase.Groups[groupIndex];
                    Match match = groupBase.Matches[matchIndex];

                    oldMatchStartTimes[match.Id] = match.StartDateTime;

                    tournamentService.SetStartTimeForMatch(match, match.StartDateTime.AddHours(addedHours));
                }

                tournamentService.Save();
            }
        }

        [When(@"matches in tournament named ""(.*)"" switches player references")]
        public void WhenMatchesInTournamentNamedSwitchesPlayerReferences(string tournamentName, Table table)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                foreach (TableRow row in table.Rows)
                {
                    ParsePlayerSwitch(row,
                        out int roundIndex,
                        out int groupIndex1,
                        out int matchIndex1,
                        out string playerName1,
                        out int groupIndex2,
                        out int matchIndex2,
                        out string playerName2);

                    RoundBase roundBase = tournament.Rounds[roundIndex];

                    GroupBase groupBase1 = roundBase.Groups[groupIndex1];
                    Match match1 = groupBase1.Matches[matchIndex1];
                    Player player1 = match1.FindPlayer(playerName1);

                    GroupBase groupBase2 = roundBase.Groups[groupIndex2];
                    Match match2 = groupBase2.Matches[matchIndex2];
                    Player player2 = match2.FindPlayer(playerName2);

                    tournamentService.SwitchPlayersInMatches(player1, player2);
                }

                tournamentService.Save();
            }
        }

        [When(@"choosing players to solve tie in tournament named ""(.*)""")]
        public void WhenChoosingPlayersToSolveTie(string tournamentName, Table table)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                foreach (TableRow row in table.Rows)
                {
                    ParseTieSolvingRow(row, out int roundIndex, out int groupIndex, out string playerName);

                    RoundBase roundBase = tournament.Rounds[roundIndex];
                    GroupBase groupBase = roundBase.Groups[groupIndex];

                    PlayerReference playerReference = tournament.GetPlayerReferenceByName(playerName);

                    tournamentService.SolveTieByChoosingPlayerInGroup(groupBase, playerReference);
                }

                tournamentService.Save();
            }
        }

        [Then(@"best of for matches in tournament named ""(.*)"" should be set to")]
        public void ThenBestOfForMatchesInTournamentNamedShouldBeSetTo(string tournamentName, Table table)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                foreach (TableRow row in table.Rows)
                {
                    ParseMatchBestOfSelection(row, out int roundIndex, out int groupIndex, out int matchIndex, out int bestOf);

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

        [Then(@"start time has been moved forward three hours for matches in tournament named ""(.*)""")]
        public void ThenStartTimeHasBeenMovedForwardThreeHoursForMatchesInTournamentNamed(string tournamentName, Table table)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                foreach (TableRow row in table.Rows)
                {
                    ParseMatchBestOfSelection(row, out int roundIndex, out int groupIndex, out int matchIndex, out int _);

                    RoundBase roundBase = tournament.Rounds[roundIndex];
                    GroupBase groupBase = roundBase.Groups[groupIndex];
                    Match match = groupBase.Matches[matchIndex];

                    DateTime oldStartDateTime = oldMatchStartTimes[match.Id];
                    match.StartDateTime.Should().BeCloseTo(oldStartDateTime.AddHours(3));
                }
            }
        }

        private void ParseMatchBestOfSelection(TableRow row, out int roundIndex, out int groupIndex, out int matchIndex, out int bestOf)
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

        private void ParseTableLayoutRow(TableRow row, out int roundIndex, out int groupIndex, out int matchIndex, out int playerIndex, out string playerName)
        {
            roundIndex = -1;
            groupIndex = -1;
            matchIndex = -1;
            playerIndex = -1;
            playerName = "";

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

            if (row.ContainsKey("Player index"))
            {
                int.TryParse(row["Player index"], out playerIndex);
            }

            if (row.ContainsKey("Player name"))
            {
                playerName = row["Player name"];
            }
        }

        private void ParsePlayerSwitch(TableRow row, out int roundIndex, out int groupIndex1, out int matchIndex1, out string playerName1, out int groupIndex2, out int matchIndex2, out string playerName2)
        {
            roundIndex = -1;
            groupIndex1 = -1;
            matchIndex1 = -1;
            playerName1 = "";
            groupIndex2 = -1;
            matchIndex2 = -1;
            playerName2 = "";

            if (row.ContainsKey("Round index"))
            {
                int.TryParse(row["Round index"], out roundIndex);
            }

            if (row.ContainsKey("Group index 1"))
            {
                int.TryParse(row["Group index 1"], out groupIndex1);
            }

            if (row.ContainsKey("Match index 1"))
            {
                int.TryParse(row["Match index 1"], out matchIndex1);
            }

            if (row.ContainsKey("Player name 1"))
            {
                playerName1 = row["Player name 1"];
            }

            if (row.ContainsKey("Group index 2"))
            {
                int.TryParse(row["Group index 2"], out groupIndex2);
            }

            if (row.ContainsKey("Match index 2"))
            {
                int.TryParse(row["Match index 2"], out matchIndex2);
            }

            if (row.ContainsKey("Player name 2"))
            {
                playerName2 = row["Player name 2"];
            }
        }

        private void ParseTieSolvingRow(TableRow row, out int roundIndex, out int groupIndex, out string playerName)
        {
            roundIndex = -1;
            groupIndex = -1;
            playerName = "";

            if (row.ContainsKey("Round index"))
            {
                int.TryParse(row["Round index"], out roundIndex);
            }

            if (row.ContainsKey("Group index"))
            {
                int.TryParse(row["Group index"], out groupIndex);
            }

            if (row.ContainsKey("Player name"))
            {
                playerName = row["Player name"];
            }
        }
    }
}
