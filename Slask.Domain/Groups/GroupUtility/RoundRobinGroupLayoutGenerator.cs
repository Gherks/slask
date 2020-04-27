using Slask.Domain.Groups.Bases;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain.Groups.GroupUtility
{
    // CREATE TESTS
    public static class RoundRobinGroupLayoutGenerator
    {
        public static List<Match> GenerateMatches(int participatingPlayerCount, GroupBase parentGroup)
        {
            int matchCount= CalculateMatchCount(participatingPlayerCount);

            List<Match> matches = new List<Match>();

            while (matches.Count < matchCount)
            {
                matches.Add(Match.Create(parentGroup));
            }

            return matches;
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
        public static void FillMatchesWithPlayers(List<PlayerReference> participants, List<Match> matches, GroupBase parentGroup)
        {
            bool groupContainsOneParticipant = participants.Count == 1;

            if (groupContainsOneParticipant)
            {
                matches.First().SetPlayers(participants.First(), null);
                return;
            }

            bool hasEvenCountOfPlayers = (participants.Count % 2) == 0;

            if (hasEvenCountOfPlayers)
            {
                UseEvenPlayerCountAlgorithm(new List<PlayerReference>(participants), matches, parentGroup);
            }
            else
            {
                UseUnevenPlayerCountAlgorithm(new List<PlayerReference>(participants), matches, parentGroup);
            }
        }

        private static void UseEvenPlayerCountAlgorithm(List<PlayerReference> participants, List<Match> matches, GroupBase parentGroup)
        {
            int numRounds = participants.Count - 1;
            int numMatchesPerRound = participants.Count / 2;
            int matchCounter = 0;

            for (int round = 0; round < numRounds; ++round)
            {
                for (int index = 0; index < numMatchesPerRound; ++index)
                {
                    matches[matchCounter++].SetPlayers(participants[index], participants[index + numMatchesPerRound]);
                }

                if (matchCounter >= matches.Count)
                {
                    break;
                }

                PlayerReference movedReference1 = participants[numMatchesPerRound - 1];
                PlayerReference movedReference2 = participants[numMatchesPerRound];

                participants.RemoveAt(numMatchesPerRound - 1);
                participants.RemoveAt(numMatchesPerRound - 1);

                participants.Add(movedReference1);
                participants.Insert(1, movedReference2);
            }
        }

        private static void UseUnevenPlayerCountAlgorithm(List<PlayerReference> participants, List<Match> matches, GroupBase parentGroup)
        {
            int numRounds = participants.Count;
            int numMatchesPerRound = participants.Count / 2;
            int matchCounter = 0;

            for (int round = 0; round < numRounds; ++round)
            {
                for (int index = 0; index < numMatchesPerRound; ++index)
                {
                    matches[matchCounter++].SetPlayers(participants[index], participants[index + numMatchesPerRound + 1]);
                }

                if (matchCounter >= matches.Count)
                {
                    break;
                }

                PlayerReference movedReference1 = participants[numMatchesPerRound];
                PlayerReference movedReference2 = participants[numMatchesPerRound + 1];

                participants.RemoveAt(numMatchesPerRound);
                participants.RemoveAt(numMatchesPerRound);

                participants.Add(movedReference1);
                participants.Insert(0, movedReference2);
            }
        }
    }
}
