using System;

namespace Slask.Domain.Bets
{
    public class MiscBet
    {
        private MiscBet()
        {
        }

        public Guid Id { get; private set; }
        public Player Player { get; private set; }

        public static MiscBet Create(Player player)
        {
            return new MiscBet
            {
                Id = Guid.NewGuid(),
                Player = player
            };
        }
    }
}
