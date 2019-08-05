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

        public static BracketGroup Create(Round round)
        {
            if (round == null)
            {
                return null;
            }

            return new BracketGroup
            {
                Id = Guid.NewGuid(),
                IsReady = false,
                RoundId = round.Id,
                Round = round
            };
        }
    }
}
