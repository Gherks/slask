using System;

namespace Slask.Domain.Bets
{
    public class MatchBet
    {
        private MatchBet()
        {
        }

        public Guid Id { get; private set; }
        public Match Match { get; private set; }
        public MatchPlayer MatchPlayer { get; private set; }

        public static MatchBet Create(Match match, MatchPlayer matchPlayer)
        {
            return new MatchBet
            {
                Id = Guid.NewGuid(),
                Match = match,
                MatchPlayer = matchPlayer
            };
        }
    }
}
