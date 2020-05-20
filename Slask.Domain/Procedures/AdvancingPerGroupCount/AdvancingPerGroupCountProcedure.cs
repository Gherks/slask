using Slask.Domain.Rounds.Bases;

namespace Slask.Domain.Procedures.AdvancingPerGroupCount
{
    public abstract class AdvancingPerGroupCountProcedure : ProcedureInterface<int, RoundBase>
    {
        public abstract bool NewValueValid(int inValue, out int outValue, RoundBase parent);
        public abstract void ApplyPostAssignmentOperations(RoundBase parent);
    }
}
