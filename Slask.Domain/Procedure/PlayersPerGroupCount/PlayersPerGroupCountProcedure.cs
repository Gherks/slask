using Slask.Domain.Rounds.Bases;

namespace Slask.Domain.Procedure
{
    public abstract class PlayersPerGroupCountProcedure : ProcedureInterface<int, RoundBase>
    {
        public abstract bool Set(int inValue, out int outValue, RoundBase parent);
    }
}
