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
                
        public override void MatchScoreChanged()
        {
            
        }
    }
}
