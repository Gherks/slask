using Slask.Domain.Rounds.Bases;
using Slask.Domain.Utilities;
using System;

namespace Slask.Domain.Procedure
{
    class MutablePlayersPerGroupCountProcedure : PlayersPerGroupCountProcedure
    {
        public override bool Set(int inValue, out int outValue, RoundBase parent)
        {
            bool tournamentHasNotBegun = parent.Tournament.GetPlayState() == PlayState.NotBegun;

            if (tournamentHasNotBegun)
            {
                outValue = Math.Max(2, inValue);

                parent.Construct();
                parent.FillGroupsWithPlayerReferences();
                parent.Tournament.FindIssues();

                return true;
            }

            outValue = -1;
            return false;
        }
    }
}
