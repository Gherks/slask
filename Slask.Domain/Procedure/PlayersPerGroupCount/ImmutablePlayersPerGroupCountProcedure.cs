using Slask.Domain.Rounds.Bases;

namespace Slask.Domain.Procedure
{
    class ImmutablePlayersPerGroupCountProcedure : PlayersPerGroupCountProcedure
    {
        public override bool Set(int inValue, out int outValue, RoundBase parent)
        {
            outValue = -1;
            return false;
        }
    }
}
