using Slask.Domain.Groups.Bases;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain.Groups.GroupUtility
{
    public static class RoundRobinGroupLayoutAssembler
    {
        public static List<Match> ConstructMathes(int participatingPlayerCount, GroupBase group)
        {
            bool invalidGroup = group == null;

            if (invalidGroup)
            {
                return new List<Match>();
            }

            int matchCount = CalculateMatchCount(participatingPlayerCount);

            List<Match> matches = new List<Match>();

            while (matches.Count < matchCount)
            {
                matches.Add(Match.Create(group));
            }

            return matches;
        }

        /*
         * Excerpt from Wikipedia (https://en.wikipedia.org/wiki/Round-robin_tournament)
         * The circle method is the standard algorithm to create a schedule for a round-robin tournament. All competitors are assigned 
         * a number, and then paired in the first round:
         * 
         * Even count: first one stays, all else go around in a circle counter-clockwise
         * 
         *  Match 1      Match 3      Match 5
         * | 1 vs 3 |   | 1 vs 4 |   | 1 vs 2 |
         *
         *  Match 2      Match 4      Match 6
         * | 2 vs 4 |   | 3 vs 2 |   | 4 vs 3 |
         * 
         * 
         * Uneven count: everyone go around in a circle counter-clockwise, one stays out each match round
         * 
         *  Match 1      Match 3      Match 5      Match 7      Match 9
         * | 1 vs 4 |   | 4 vs 5 |   | 5 vs 3 |   | 3 vs 2 |   | 2 vs 1 |
         *
         *  Match 2      Match 4      Match 6      Match 8      Match 10
         * | 2 vs 5 |   | 1 vs 3 |   | 4 vs 2 |   | 5 vs 1 |   | 3 vs 4 |
         *     3            2            1            4            5
         */
        public static void AssignPlayersToMatches(List<PlayerReference> playerReferences, List<Match> matches)
        {
            if (CannotFillGivenMatchesWithGivenPlayerReferences(playerReferences, matches))
            {
                return;
            }

            bool groupContainsOnePlayerReferences = playerReferences.Count == 1;

            if (groupContainsOnePlayerReferences)
            {
                matches.First().SetPlayers(playerReferences.First(), null);
                return;
            }

            bool hasEvenCountOfPlayers = (playerReferences.Count % 2) == 0;

            if (hasEvenCountOfPlayers)
            {
                UseEvenPlayerCountAlgorithm(new List<PlayerReference>(playerReferences), matches);
            }
            else
            {
                UseUnevenPlayerCountAlgorithm(new List<PlayerReference>(playerReferences), matches);
            }
        }

        private static int CalculateMatchCount(int participantCount)
        {
            bool evenCountOfPlayers = (participantCount % 2) == 0;

            if (evenCountOfPlayers)
            {
                return (participantCount / 2) * (participantCount - 1);
            }
            else
            {
                return ((participantCount - 1) / 2) * participantCount;
            }
        }

        private static bool CannotFillGivenMatchesWithGivenPlayerReferences(List<PlayerReference> playerReferences, List<Match> matches)
        {
            bool invalidPlayerReferencesList = playerReferences == null;
            bool invalidMatchesList = matches == null;

            if (invalidPlayerReferencesList || invalidMatchesList)
            {
                // LOG Error: Invalid parameters given when attempting to fill round robin matches with player references
                return true;
            }

            bool cannotFitAllPlayerReferences = matches.Count < CalculateMatchCount(playerReferences.Count);

            if (cannotFitAllPlayerReferences)
            {
                // LOG Error: Cannot fit provided player references in provided list of matches
                return true;
            }

            return false;
        }

        private static void UseEvenPlayerCountAlgorithm(List<PlayerReference> playerReferences, List<Match> matches)
        {
            int numRounds = playerReferences.Count - 1;
            int numMatchesPerRound = playerReferences.Count / 2;
            int matchCounter = 0;

            for (int round = 0; round < numRounds; ++round)
            {
                for (int index = 0; index < numMatchesPerRound; ++index)
                {
                    matches[matchCounter++].SetPlayers(playerReferences[index], playerReferences[index + numMatchesPerRound]);
                }

                if (matchCounter >= matches.Count)
                {
                    break;
                }

                PlayerReference movedReference1 = playerReferences[numMatchesPerRound - 1];
                PlayerReference movedReference2 = playerReferences[numMatchesPerRound];

                playerReferences.RemoveAt(numMatchesPerRound - 1);
                playerReferences.RemoveAt(numMatchesPerRound - 1);

                playerReferences.Add(movedReference1);
                playerReferences.Insert(1, movedReference2);
            }
        }

        private static void UseUnevenPlayerCountAlgorithm(List<PlayerReference> playerReferences, List<Match> matches)
        {
            int numRounds = playerReferences.Count;
            int numMatchesPerRound = playerReferences.Count / 2;
            int matchCounter = 0;

            for (int round = 0; round < numRounds; ++round)
            {
                for (int index = 0; index < numMatchesPerRound; ++index)
                {
                    matches[matchCounter++].SetPlayers(playerReferences[index], playerReferences[index + numMatchesPerRound + 1]);
                }

                if (matchCounter >= matches.Count)
                {
                    break;
                }

                PlayerReference movedReference1 = playerReferences[numMatchesPerRound];
                PlayerReference movedReference2 = playerReferences[numMatchesPerRound + 1];

                playerReferences.RemoveAt(numMatchesPerRound);
                playerReferences.RemoveAt(numMatchesPerRound);

                playerReferences.Add(movedReference1);
                playerReferences.Insert(0, movedReference2);
            }
        }
    }
}
