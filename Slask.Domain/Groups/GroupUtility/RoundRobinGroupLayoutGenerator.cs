using System;
using System.Collections.Generic;
using System.Text;

namespace Slask.Domain.Groups
{
    public static class RoundRobinGroupLayoutGenerator
    {
        public static List<Match> Generate(List<PlayerReference> participants, GroupBase parentGroup)
        {
            int matchAmount = CalculateMatchAmount(participants.Count);

            List<Match> matches = new List<Match>();

            while (matches.Count < matchAmount)
            {
                matches.Add(Match.Create(parentGroup));
            }

            AssignPlayersToMatches(new List<PlayerReference>(participants), matches, parentGroup);

            return matches;
        }

        private static int CalculateMatchAmount(int participantAmount)
        {
            bool evenAmountOfPlayers = (participantAmount % 2) == 0;

            if (evenAmountOfPlayers)
            {
                return (participantAmount / 2) * (participantAmount - 1);
            }
            else
            {
                return ((participantAmount - 1) / 2) * participantAmount;
            }
        }

        /*
         * Excerpt from Wikipedia (https://en.wikipedia.org/wiki/Round-robin_tournament)
         * The circle method is the standard algorithm to create a schedule for a round-robin tournament. All competitors are assigned 
         * a number, and then paired in the first round:
         * 
         * Even amount: first one stays, all else go around in a circle counter-clockwise
         * 
         *  Match 1      Match 3      Match 5
         * | 1 vs 3 |   | 1 vs 4 |   | 1 vs 2 |
         *
         *  Match 2      Match 4      Match 6
         * | 2 vs 4 |   | 3 vs 2 |   | 4 vs 3 |
         * 
         * 
         * Uneven amount: everyone go around in a circle counter-clockwise, one stays out each match round
         * 
         *  Match 1      Match 3      Match 5      Match 7      Match 9
         * | 1 vs 4 |   | 4 vs 5 |   | 5 vs 3 |   | 3 vs 2 |   | 2 vs 1 |
         *
         *  Match 2      Match 4      Match 6      Match 8      Match 10
         * | 2 vs 5 |   | 1 vs 3 |   | 4 vs 2 |   | 5 vs 1 |   | 3 vs 4 |
         *     3            2            1            4            5
         */
        private static void AssignPlayersToMatches(List<PlayerReference> participants, List<Match> matches, GroupBase parentGroup)
        {
            bool hasEvenAmountOfPlayers = (participants.Count % 2) == 0;

            if (hasEvenAmountOfPlayers)
            {
                UseEvenPlayerAmountAlgorithm(participants, matches, parentGroup);
            }
            else
            {
                UseUnevenPlayerAmountAlgorithm(participants, matches, parentGroup);
            }
        }

        private static void UseEvenPlayerAmountAlgorithm(List<PlayerReference> participants, List<Match> matches, GroupBase parentGroup)
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

        private static void UseUnevenPlayerAmountAlgorithm(List<PlayerReference> participants, List<Match> matches, GroupBase parentGroup)
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
