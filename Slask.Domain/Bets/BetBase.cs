using Slask.Domain.ObjectState;
using Slask.Domain.Utilities;
using System;

namespace Slask.Domain.Bets
{
    public abstract class BetBase : ObjectStateBase, BetInterface
    {
        protected BetBase()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public BetTypeEnum BetType { get; protected set; }
        public Guid BetterId { get; protected set; }
        public Better Better { get; protected set; }
        public Guid MatchId { get; protected set; }
        public Guid PlayerReferenceId { get; protected set; }

        public abstract bool IsWon();
    }
}
