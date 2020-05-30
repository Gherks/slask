using System.Collections.Generic;

namespace Slask.Domain.Utilities.StandingsSolvers
{
    public interface StandingsSolverInterface<SourceType, EntryType>
    {
        List<StandingsEntry<EntryType>> FetchFrom(SourceType source);
    }
}
