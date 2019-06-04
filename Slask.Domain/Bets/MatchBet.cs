using System;

namespace Slask.Domain.Bets
{
    public class MatchBet
    {
        public Guid Id { get; set; }
        public Match Match { get; set; }
        public Player Player { get; set; }
    }
}
