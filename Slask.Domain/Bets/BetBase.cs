using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slask.Domain.Bets
{
    [Table("Bet")]
    public class BetBase : BetInterface
    {
        protected BetBase()
        {
        }

        public Guid Id { get; protected set; }
        public Guid BetterId { get; protected set; }
        public Better Better { get; protected set; }

        public virtual bool IsWon()
        {
            return false;
        }
    }
}
