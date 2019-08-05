using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Slask.Domain
{
    public class DualTournamentGroup : GroupBase
    {
        private DualTournamentGroup()
        {
            Matches.Add(Match.Create());
            Matches.Add(Match.Create());
            Matches.Add(Match.Create());
            Matches.Add(Match.Create());
            Matches.Add(Match.Create());
        }

        public static DualTournamentGroup Create(Round round)
        {
            if (round == null)
            {
                return null;
            }

            return new DualTournamentGroup
            {
                Id = Guid.NewGuid(),
                IsReady = false,
                RoundId = round.Id,
                Round = round
            };
        }

        public override void MatchScoreChanged()
        {

        }
    }
}
