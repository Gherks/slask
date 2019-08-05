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

        public static RoundRobinGroup Create(Round round)
        {
            if (round == null)
            {
                return null;
            }

            return new RoundRobinGroup()
            {
                Id = Guid.NewGuid(),
                IsReady = false,
                RoundId = round.Id,
                Round = round
            };
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
