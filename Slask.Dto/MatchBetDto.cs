using System;

namespace Slask.Dto
{
    public sealed class MatchBetDto
    {
        public Guid Id { get; set; }
        public Guid BetterId { get; set; }
        public Guid MatchId { get; set; }
        public Guid PlayerId { get; set; }
    }
}