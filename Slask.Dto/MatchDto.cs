using System;

namespace Slask.Dto
{
    public sealed class MatchDto
    {
        public Guid Id { get; set; }
        public int SortOrder { get; set; }
        public int BestOf { get; set; }
        public DateTime StartDateTime { get; set; }
        public PlayerDto Player1 { get; set; }
        public PlayerDto Player2 { get; set; }
    }
}