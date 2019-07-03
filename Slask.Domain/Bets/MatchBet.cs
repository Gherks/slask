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
        public Player Player { get; private set; }

        public static MatchBet Create(Match match, Player player)
        {
            return new MatchBet
            {
                Id = Guid.NewGuid(),
                Match = match,
                Player = player
            };
        }
    }
}
