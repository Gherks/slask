namespace Slask.Domain.Utilities
{
    public class StandingsEntryBase<Type> : StandingsEntryInterface<Type>
    {
        public Type Object { get; protected set; }
        public int Points { get; protected set; }

        public void AddPoint()
        {
            ++Points;
        }
    }
}
