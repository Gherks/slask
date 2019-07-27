using System;

namespace Slask.Domain.Bets
{
    public class MiscBet : BetBase
    {
        private MiscBet()
        {
        }

        public Player Player { get; private set; }

        public static MiscBet Create(Player player)
        {
            return new MiscBet
            {
                Player = player
            };
        }
    }
}
