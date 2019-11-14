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
            int matchAmount = ParticipatingPlayers.Count - 1;
            ChangeMatchAmountTo(matchAmount);
        }

        protected override void OnParticipantRemoved(PlayerReference playerReference)
        {
            int matchAmount = ParticipatingPlayers.Count - 1;
            ChangeMatchAmountTo(matchAmount);
        }
    }
}
