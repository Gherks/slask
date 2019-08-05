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


            int numMatches = CalculateMatchAmount();

            for (int index = 0; index < numMatches; ++index)
            {
                Matches.Add(Match.Create());

                // Assign PlayerReferences to players in matches
            }
        }

        private int CalculateMatchAmount()
        {
            // Even amount
            // numMatches = (ParticipatingPlayers.Count / 2) * (ParticipatingPlayers.Count - 1)

            // Uneven amount
            // numMatches = ((ParticipatingPlayers.Count - 1) / 2) * (ParticipatingPlayers.Count)

            int playerAmount = ParticipatingPlayers.Count;
            bool evenAmountOfPlayers = (playerAmount % 2) == 0;

            if (evenAmountOfPlayers)
            {
                return (playerAmount / 2) * (playerAmount - 1);
            }
            else
            {
                return ((playerAmount - 1) / 2) * playerAmount;
            }
        }
    }
}
