using Slask.Domain.Rounds.Interfaces;
using Slask.Domain.Utilities;
using System;

namespace Slask.Domain.Rounds.Bases
{
    public class ResizableRound : RoundBase, ResizableRoundInterface
    {
        public bool SetPlayersPerGroupCount(int count)
        {
            bool roundIsFirstRound = IsFirstRound();
            bool tournamentHasNotBegun = GetPlayState() == PlayState.NotBegun;

            if (roundIsFirstRound && tournamentHasNotBegun)
            {
                PlayersPerGroupCount = Math.Max(2, count);

                Construct();
                FillGroupsWithPlayerReferences();

                return true;
            }

            return false;
        }
    }
}
