using Slask.Domain.Rounds;

namespace Slask.Domain.Procedures.AdvancingPerGroupCount
{
    public class ImmutableAdvancingPerGroupCountProcedure : AdvancingPerGroupCountProcedure
    {
        public override bool NewValueValid(int inValue, out int outValue, RoundBase parent)
        {
            outValue = -1;
            return false;
        }

        public override void ApplyPostAssignmentOperations(RoundBase parent)
        {
        }
    }
}
