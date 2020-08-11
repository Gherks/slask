using Slask.Common;
using System;
using TechTalk.SpecFlow;

namespace Slask.TestCore
{
    public class TestUtilities
    {
        public static void ParseRoundTable(TableRow row, out string roundType, out string name, out int advancingCount, out int playersPerGroupCount)
        {
            roundType = "";
            name = "";
            advancingCount = 1;
            playersPerGroupCount = 2;

            if (row.ContainsKey("Round type"))
            {
                roundType = row["Round type"];
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

        public static string ParseRoundGroupTypeString(string type)
        {
            type = StringUtility.ToUpperNoSpaces(type);

            if (type.Contains("BRACKET", StringComparison.CurrentCulture))
            {
                return "BRACKET";
            }
            else if (type.Contains("DUALTOURNAMENT", StringComparison.CurrentCulture))
            {
                return "DUALTOURNAMENT";
            }
            else if (type.Contains("ROUNDROBIN", StringComparison.CurrentCulture))
            {
                return "ROUNDROBIN";
            }

            return "";
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
    }
}
