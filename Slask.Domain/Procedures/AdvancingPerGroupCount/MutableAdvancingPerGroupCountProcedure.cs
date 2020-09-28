using Slask.Domain.Rounds;
using Slask.Domain.Utilities;

namespace Slask.Domain.Procedures.AdvancingPerGroupCount
{
    public class MutableAdvancingPerGroupCountProcedure : AdvancingPerGroupCountProcedure
    {
        public override bool NewValueValid(int inValue, out int outValue, RoundBase parent)
        {
            bool tournamentHasNotBegun = parent.GetPlayState() == PlayStateEnum.NotBegun;

            if (tournamentHasNotBegun)
            {
                outValue = inValue;
                return true;
            }

            outValue = -1;
            return false;
        }

        public override void ApplyPostAssignmentOperations(RoundBase parent)
        {
            parent.Construct();
            parent.Tournament.FindIssues();
        }
    }
}
