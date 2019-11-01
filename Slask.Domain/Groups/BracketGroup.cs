using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public class BracketGroup : GroupBase
    {
        private BracketGroup()
        {
        }

        public static BracketGroup Create(Round round)
        {
            if (round == null)
            {
                return null;
            }

            return new BracketGroup
            {
                Id = Guid.NewGuid(),
                RoundId = round.Id,
                Round = round
            };
        }

        protected override void OnParticipantAdded(PlayerReference playerReference)
        {
            int numMatches = CalculateMatchAmount();
        }

        public override List<PlayerReference> TallyUpAdvancingPlayers()
        {
            Match lastMatch = Matches[Matches.Count - 1];

            return new List<PlayerReference>
            {
                lastMatch.GetWinningPlayer().PlayerReference
            };
        }

        private int CalculateMatchAmount()
        {
            int matchAmount = 0;
            int tiers = ParticipatingPlayers.Count;

            while(tiers > 1)
            {
                tiers = (int)Math.Ceiling(tiers / 2.0);
                matchAmount += tiers;
            }

            return matchAmount;
        }
    }
}
