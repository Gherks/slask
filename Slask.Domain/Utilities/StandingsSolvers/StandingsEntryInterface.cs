namespace Slask.Domain.Utilities.StandingsSolvers
{
    public interface StandingsEntryInterface<Type>
    {
        Type Object { get; }
        int Points { get; }

        void AddPoint();
    }
}
