using Slask.Domain.Rounds.Bases;

namespace Slask.Domain.Procedures.PlayersPerGroupCount
{
    public abstract class PlayersPerGroupCountProcedure : ProcedureInterface<int, RoundBase>
    {
        public abstract bool NewValueValid(int inValue, out int outValue, RoundBase parent);
        public abstract void ApplyPostAssignmentOperations(RoundBase parent);
    }
}
