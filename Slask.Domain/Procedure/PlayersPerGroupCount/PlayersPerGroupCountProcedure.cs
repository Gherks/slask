using Slask.Domain.Rounds.Bases;

namespace Slask.Domain.Procedure
{
    public abstract class PlayersPerGroupCountProcedure : ProcedureInterface<int, RoundBase>
    {
        public abstract bool NewValueValid(int inValue, out int outValue, RoundBase parent);
        public abstract void ApplyPostAssignmentOperations(RoundBase parent);
    }
}
