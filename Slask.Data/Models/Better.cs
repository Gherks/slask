using System.Collections.Generic;

namespace Slask.Data.Models
{
    public class Better
    {
        Better()
        {
            MatchBets = new List<Bets.MatchBet>();
            MiscBets = new List<Bets.MiscBet>();
        }

        public int Id { get; set; }
        public List<Bets.MatchBet> MatchBets { get; set; }
        public List<Bets.MiscBet> MiscBets { get; set; }
    }
}
