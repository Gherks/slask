namespace Slask.Domain.Utilities
{
    public interface StandingsEntryInterface<Type>
    {
        Type Object { get; }
        int Points { get; }

        void AddPoint();
    }
}
