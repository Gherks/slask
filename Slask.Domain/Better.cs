using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public class Better
    {
        Better()
        {
            MatchBets = new List<Bets.MatchBet>();
            MiscBets = new List<Bets.MiscBet>();
        }

        public Guid Id { get; set; }
        public List<Bets.MatchBet> MatchBets { get; set; }
        public List<Bets.MiscBet> MiscBets { get; set; }
    }
}
