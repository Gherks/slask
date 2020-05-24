﻿using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using System;

namespace Slask.Domain.Procedures.PlayersPerGroupCount
{
    public class MutablePlayersPerGroupCountProcedure : PlayersPerGroupCountProcedure
    {
        public override bool NewValueValid(int inValue, out int outValue, RoundBase parent)
        {
            bool tournamentHasNotBegun = parent.Tournament.GetPlayState() == PlayState.NotBegun;

            if (tournamentHasNotBegun)
            {
                outValue = Math.Max(2, inValue);
                return true;
            }

            outValue = -1;
            return false;
        }

        public override void ApplyPostAssignmentOperations(RoundBase parent)
        {
            parent.Construct();
            parent.FillGroupsWithPlayerReferences();
            parent.Tournament.FindIssues();
        }
    }
}
