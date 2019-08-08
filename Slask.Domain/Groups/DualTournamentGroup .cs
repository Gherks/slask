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
        }

        public static DualTournamentGroup Create(Round round)
        {
            if (round == null)
            {
                return null;
            }

            DualTournamentGroup group = new DualTournamentGroup
            {
                Id = Guid.NewGuid(),
                IsReady = false,
                RoundId = round.Id,
                Round = round
            };

            group.Matches.Add(Match.Create(group));
            group.Matches.Add(Match.Create(group));
            group.Matches.Add(Match.Create(group));
            group.Matches.Add(Match.Create(group));
            group.Matches.Add(Match.Create(group));

            return group;
        }

        public override void MatchScoreChanged()
        {

        }
    }
}
