using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain
{
    public class BracketGroup : GroupBase
    {
        private BracketGroup()
        {
        }

        protected override bool MatchCanBeAdded()
        {
            // 
            // CannotAddMorePlayersWhenFirstMatchStarted
            // EveryOtherMatchAnotherOneIsCreatedUntilFinal

            return false;
        }
    }
}
