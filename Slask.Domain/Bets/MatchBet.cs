using System;

namespace Slask.Domain.Bets
{
    public class MatchBet : BetBase
    {
        private MatchBet()
        {
        }

        public Match Match { get; private set; }
        public Player Player { get; private set; }

        public static MatchBet Create(Match match, Player player)
        {
            if (match == null || player == null)
            {
                return null;
            }

            return new MatchBet
            {
                Match = match,
                Player = player
            };
        }
    }
}
