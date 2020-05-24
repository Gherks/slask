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
            if (player == null)
            {
                return null;
            }

            return new MiscBet
            {
                Player = player
            };
        }
    }
}
