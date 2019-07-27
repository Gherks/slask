using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain
{
    public class RoundRobinGroup : GroupBase
    {
        private RoundRobinGroup()
        {
        }

        protected override void UpdateMatchLayout()
        {
            // numMatches = (ParticipatingPlayers.Count - 1) * (ParticipatingPlayers.Count / 2)
            // but what happens when uneven amount of players is participating? check wikipedia maybe
        }
    }
}
