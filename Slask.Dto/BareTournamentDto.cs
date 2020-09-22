using System;

namespace Slask.Dto
{
    public sealed class BareTournamentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }
}
