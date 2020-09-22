using System;

namespace Slask.Dto
{
    public class MatchBetDto
    {
        public Guid Id { get; set; }
        public Guid BetterId { get; set; }
        public Guid MatchId { get; set; }
        public Guid PlayerId { get; set; }
    }
}