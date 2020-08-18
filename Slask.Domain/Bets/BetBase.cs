using Slask.Domain.ObjectState;
using System;

namespace Slask.Domain.Bets
{
    public class BetBase : ObjectStateBase, BetInterface
    {
        protected BetBase()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public Guid BetterId { get; protected set; }
        public Better Better { get; protected set; }

        public virtual bool IsWon()
        {
            return false;
        }
    }
}
