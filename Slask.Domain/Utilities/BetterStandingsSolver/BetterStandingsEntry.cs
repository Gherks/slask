namespace Slask.Domain.Utilities
{
    public class BetterStandingsEntry
    {
        public Better Better { get; private set; }
        public int Points { get; private set; }

        public static BetterStandingsEntry Create(Better better)
        {
            return new BetterStandingsEntry
            {
                Better = better,
                Points = 0
            };
        }

        public void AddPoint()
        {
            Points += 1;
        }
    }
}
