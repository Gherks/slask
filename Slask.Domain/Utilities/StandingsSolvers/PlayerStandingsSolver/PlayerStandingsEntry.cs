namespace Slask.Domain.Utilities.StandingsSolvers
{
    public class PlayerStandingEntry : StandingsEntryBase<PlayerReference>
    {
        public static PlayerStandingEntry Create(PlayerReference playerReference)
        {
            return new PlayerStandingEntry
            {
                Object = playerReference,
                Points = 0
            };
        }
    }
}
