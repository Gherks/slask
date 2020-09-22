using System;
using System.Collections.Generic;

namespace Slask.Dto
{
    public sealed class TournamentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public List<RoundDto> Rounds { get; set; }
        public List<BetterDto> Betters { get; set; }
        public List<string> Issues { get; set; }
    }
}
