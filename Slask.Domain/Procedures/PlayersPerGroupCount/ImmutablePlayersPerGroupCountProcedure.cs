using Slask.Domain.Rounds.Bases;

namespace Slask.Domain.Procedures.PlayersPerGroupCount
{
    class ImmutablePlayersPerGroupCountProcedure : PlayersPerGroupCountProcedure
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
