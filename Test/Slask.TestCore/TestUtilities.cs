using Slask.Common;
using Slask.Domain.Utilities;
using System;
using TechTalk.SpecFlow;

namespace Slask.TestCore
{
    public class TestUtilities
    {
        public static void ParseRoundTable(TableRow row, out ContestTypeEnum roundType, out string name, out int advancingCount, out int playersPerGroupCount)
        {
            roundType = ContestTypeEnum.None;
            name = "";
            advancingCount = 1;
            playersPerGroupCount = 2;

            if (row.ContainsKey("Round type"))
            {
                roundType = ParseContestTypeString(row["Round type"]);
            }

            if (row.ContainsKey("Round name"))
            {
                name = row["Round name"];
            }

            if (row.ContainsKey("Advancing per group count"))
            {
                int.TryParse(row["Advancing per group count"], out advancingCount);
            }

            if (row.ContainsKey("Players per group count"))
            {
                int.TryParse(row["Players per group count"], out playersPerGroupCount);
            }
        }

        public static ContestTypeEnum ParseContestTypeString(string type)
        {
            type = StringUtility.ToUpperNoSpaces(type);

            if (type.Contains("BRACKET", StringComparison.CurrentCulture))
            {
                return ContestTypeEnum.Bracket;
            }
            else if (type.Contains("DUALTOURNAMENT", StringComparison.CurrentCulture))
            {
                return ContestTypeEnum.DualTournament;
            }
            else if (type.Contains("ROUNDROBIN", StringComparison.CurrentCulture))
            {
                return ContestTypeEnum.RoundRobin;
            }

            return ContestTypeEnum.None;
        }

        public static void ParseTargetGroupToPlay(TableRow row, out int tournamentIndex, out int roundIndex, out int groupIndex)
        {
            tournamentIndex = 0;
            roundIndex = 0;
            groupIndex = 0;

            if (row.ContainsKey("Tournament index"))
            {
                int.TryParse(row["Tournament index"], out tournamentIndex);
            }

            if (row.ContainsKey("Round index"))
            {
                int.TryParse(row["Round index"], out roundIndex);
            }

            if (row.ContainsKey("Group index"))
            {
                int.TryParse(row["Group index"], out groupIndex);
            }
        }

        public static void ParseBetterMatchBetPlacements(TableRow row, out string betterName, out int roundIndex, out int groupIndex, out int matchIndex, out string playerName)
        {
            betterName = "";
            roundIndex = -1;
            groupIndex = -1;
            matchIndex = -1;
            playerName = "";

            if (row.ContainsKey("Better name"))
            {
                betterName = row["Better name"];
            }

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

            if (row.ContainsKey("Player name"))
            {
                playerName = row["Player name"];
            }
        }

        public static void ParseBetterStandings(TableRow row, out string betterName, out int points)
        {
            betterName = "";
            points = -1;

            if (row.ContainsKey("Better name"))
            {
                betterName = row["Better name"];
            }

            if (row.ContainsKey("Points"))
            {
                int.TryParse(row["Points"], out points);
            }
        }

        public static void ParseScoreAddedToMatchPlayer(TableRow row, out int roundIndex, out int groupIndex, out int matchIndex, out string scoringPlayer, out int scoreAdded)
        {
            roundIndex = 0;
            groupIndex = 0;
            matchIndex = 0;
            scoringPlayer = "";
            scoreAdded = 0;

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

            if (row.ContainsKey("Scoring player"))
            {
                scoringPlayer = row["Scoring player"];
            }

            if (row.ContainsKey("Added score"))
            {
                int.TryParse(row["Added score"], out scoreAdded);
            }
        }
    }
}
