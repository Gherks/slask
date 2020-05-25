using System;

namespace Slask.Domain.Bets
{
    public interface BetInterface
    {
        Better Better { get; }
        Guid BetterId { get; }
        Guid Id { get; }
    }
}