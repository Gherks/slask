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

        public static Group Create()
        {
            return new Group
            {
                Id = Guid.NewGuid()
            };
        }

        public Match AddMatch(MatchPlayer matchPlayer1, MatchPlayer matchPlayer2)
        {
            throw new NotImplementedException();
        }
    }
}
