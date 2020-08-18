using Slask.Domain.ObjectState;

namespace Slask.Domain.Bets.BetTypes
{
    public class MiscBet : BetBase
    {
        private MiscBet()
        {
        }

        public Player Player { get; private set; }

        public static MiscBet Create(Player player)
        {
            if (player == null)
            {
                return null;
            }

            return new MiscBet
            {
                Player = player
            };
        }

        public override bool IsWon()
        {
            throw new System.NotImplementedException();
        }
    }
}
