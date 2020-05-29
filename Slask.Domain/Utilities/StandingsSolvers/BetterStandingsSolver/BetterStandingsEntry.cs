namespace Slask.Domain.Utilities.StandingsSolvers
{
    public class BetterStandingsEntry : StandingsEntryBase<Better>
    {
        public static BetterStandingsEntry Create(Better better)
        {
            return new BetterStandingsEntry
            {
                Object = better,
                Points = 0
            };
        }
    }
}
