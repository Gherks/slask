using Slask.Domain.Utilities;
using System;

namespace Slask.Domain.Bets
{
    public interface BetInterface
    {
        Guid Id { get; }
        BetTypeEnum BetType { get; }
        Guid BetterId { get; }
        Better Better { get; }
        Guid MatchId { get; }
        Guid PlayerReferenceId { get; }

        public bool IsWon();
    }
}