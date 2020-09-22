using System;
using System.Collections.Generic;

namespace Slask.Dto
{
    public class BetterDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public List<MatchBetDto> MatchBets { get; set; }
    }
}