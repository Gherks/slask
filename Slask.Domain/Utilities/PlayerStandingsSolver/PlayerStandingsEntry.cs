namespace Slask.Domain.Utilities
{
    public class PlayerStandingEntry
    {
        public PlayerReference PlayerReference { get; private set; }
        public int Wins { get; private set; }

        public static PlayerStandingEntry Create(PlayerReference playerReference)
        {
            return new PlayerStandingEntry
            {
                PlayerReference = playerReference,
                Wins = 0
            };
        }

        public void AddWin()
        {
            Wins += 1;
        }
    }
}
