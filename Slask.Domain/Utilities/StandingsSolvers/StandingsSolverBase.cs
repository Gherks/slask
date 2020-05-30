using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain.Utilities.StandingsSolvers
{
    public abstract class StandingsSolverBase<SourceType, EntryType> : StandingsSolverInterface<SourceType, EntryType>
    {
        public List<StandingsEntry<EntryType>> FetchFrom(SourceType source)
        {
            List<StandingsEntry<EntryType>> entryStandings = CreateStandingsList(source);

            AggregatePointsForStandingEntries(source, entryStandings);

            return entryStandings.OrderByDescending(entry => entry.Points).ToList();
        }

        protected abstract List<StandingsEntry<EntryType>> CreateStandingsList(SourceType source);
        protected abstract void AggregatePointsForStandingEntries(SourceType source, List<StandingsEntry<EntryType>> playerStandings);
    }
}
