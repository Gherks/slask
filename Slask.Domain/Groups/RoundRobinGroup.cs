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
            // Even amount
            // numMatches = (ParticipatingPlayers.Count / 2) * (ParticipatingPlayers.Count - 1)

            // Uneven amount
            // numMatches = ((ParticipatingPlayers.Count - 1) / 2) * (ParticipatingPlayers.Count)
        }
    }
}
