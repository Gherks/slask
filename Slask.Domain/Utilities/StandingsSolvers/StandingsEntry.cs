namespace Slask.Domain.Utilities.StandingsSolvers
{
    public class StandingsEntry<Type> : StandingsEntryInterface<Type>
    {
        public Type Object { get; protected set; }
        public int Points { get; protected set; }

        public static StandingsEntry<Type> Create(Type _object)
        {
            return new StandingsEntry<Type>
            {
                Object = _object,
                Points = 0
            };
        }

        public void AddPoint()
        {
            Points += 1;
        }
    }
}
