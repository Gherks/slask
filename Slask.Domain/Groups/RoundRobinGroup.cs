using Slask.Domain.Rounds;
using System;
using System.Collections.Generic;

/*
 * Excerpt from Wikipedia (https://en.wikipedia.org/wiki/Round-robin_tournament)
 * If N is the number of competitors, a pure round robin tournament requires N / 2 * (N - 1) games. If N is even, then in each
 * of (N - 1) rounds, N / 2 games can be run concurrently, provided there exist sufficient resources (e.g. courts for a tennis 
 * tournament). If N is odd, there will be N rounds, each with (N - 1) / 2 games, and one competitor having no game in that round. 
 */
namespace Slask.Domain
{
    public class RoundRobinGroup : GroupBase
    {
        private RoundRobinGroup()
        {
        }

        public static RoundRobinGroup Create(RoundRobinRound round)
        {
            if (round == null)
            {
                return null;
            }

            return new RoundRobinGroup()
            {
                Id = Guid.NewGuid(),
                RoundId = round.Id,
                Round = round
            };
        }

        protected override void OnParticipantAdded(PlayerReference playerReference)
        {
            UpdateMatchLayout();
        }

        protected override void OnParticipantRemoved(PlayerReference playerReference)
        {
            UpdateMatchLayout();
        }

        private void UpdateMatchLayout()
        {
            int numMatches = CalculateMatchAmount();

            ChangeMatchAmountTo(numMatches);

            if (numMatches > 0)
            {
                AssignPlayersToMatches();
            }
        }

        private int CalculateMatchAmount()
        {
            int playerAmount = ParticipatingPlayers.Count;
            bool evenAmountOfPlayers = (playerAmount % 2) == 0;

            if (evenAmountOfPlayers)
            {
                return (playerAmount / 2) * (playerAmount - 1);
            }
            else
            {
                return ((playerAmount - 1) / 2) * playerAmount;
            }
        }

        /*
         * Excerpt from Wikipedia (https://en.wikipedia.org/wiki/Round-robin_tournament)
         * The circle method is the standard algorithm to create a schedule for a round-robin tournament. All competitors are assigned 
         * a number, and then paired in the first round:
         * 
         * Even (first one stays, all else go around in a circle counter-clockwise)
         * 
         *  Match 1      Match 3      Match 5
         * | 0 vs 2 |   | 0 vs 3 |   | 0 vs 1 |
         *
         *  Match 2      Match 4      Match 6
         * | 1 vs 3 |   | 2 vs 1 |   | 3 vs 2 |
         * 
         * 
         * Uneven (everyone go around in a circle counter-clockwise, one stays out each match round)
         * 
         *  Match 1      Match 3      Match 5      Match 7      Match 9
         * | 0 vs 3 |   | 3 vs 4 |   | 4 vs 2 |   | 2 vs 1 |   | 1 vs 0 |
         *
         *  Match 2      Match 4      Match 6      Match 8      Match 10
         * | 1 vs 4 |   | 0 vs 2 |   | 3 vs 1 |   | 4 vs 0 |   | 2 vs 3 |
         *     2            1            0            3            4
         */
        private void AssignPlayersToMatches()
        {
            List<PlayerReference> players = new List<PlayerReference>(ParticipatingPlayers);

            bool hasEvenAmountOfPlayers = (players.Count % 2) == 0;

            if (hasEvenAmountOfPlayers)
            {
                UseEvenPlayerAmountAlgorithm(players);
            }
            else
            {
                UseUnevenPlayerAmountAlgorithm(players);
            }
        }

        private void UseEvenPlayerAmountAlgorithm(List<PlayerReference> players)
        {
            int numRounds = players.Count - 1;
            int numMatchesPerRound = ParticipatingPlayers.Count / 2;
            int matchCounter = 0;

            for (int round = 0; round < numRounds; ++round)
            {
                for (int index = 0; index < numMatchesPerRound; ++index)
                {
                    Matches[matchCounter++].AssignPlayerReferences(players[index], players[index + numMatchesPerRound]);
                }

                if (matchCounter >= Matches.Count)
                {
                    break;
                }

                PlayerReference movedReference1 = players[numMatchesPerRound - 1];
                PlayerReference movedReference2 = players[numMatchesPerRound];

                players.RemoveAt(numMatchesPerRound - 1);
                players.RemoveAt(numMatchesPerRound - 1);

                players.Add(movedReference1);
                players.Insert(1, movedReference2);
            }
        }

        private void UseUnevenPlayerAmountAlgorithm(List<PlayerReference> players)
        {
            int numRounds = players.Count;
            int numMatchesPerRound = ParticipatingPlayers.Count / 2;
            int matchCounter = 0;

            for (int round = 0; round < numRounds; ++round)
            {
                for (int index = 0; index < numMatchesPerRound; ++index)
                {
                    Matches[matchCounter++].AssignPlayerReferences(players[index], players[index + numMatchesPerRound + 1]);
                }

                if (matchCounter >= Matches.Count)
                {
                    break;
                }

                PlayerReference movedReference1 = players[numMatchesPerRound];
                PlayerReference movedReference2 = players[numMatchesPerRound + 1];

                players.RemoveAt(numMatchesPerRound);
                players.RemoveAt(numMatchesPerRound);

                players.Add(movedReference1);
                players.Insert(0, movedReference2);
            }
        }
    }
}
