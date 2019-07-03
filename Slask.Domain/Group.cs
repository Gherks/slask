using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public class Group
    {
        private Group()
        {
            Matches = new List<Match>();
        }

        public Guid Id { get; private set; }
        public List<Match> Matches { get; private set; }
        public Guid RoundId { get; private set; }
        public Round Round { get; private set; }

        public static Group Create(Round parentRound)
        {
            if(parentRound == null)
            {
                return null;
            }

            return new Group
            {
                Id = Guid.NewGuid(),
                Round = parentRound
            };
        }

        public Match AddMatch(string player1Name, string player2Name, DateTime startDateTime)
        {
            Match match = Match.Create(player1Name, player2Name, startDateTime);

            if (match != null)
            {
                Matches.Add(match);
            }

            return match;
        }
    }
}
