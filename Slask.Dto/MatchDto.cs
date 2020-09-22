using System;

namespace Slask.Dto
{
    public sealed class MatchDto
    {
        public Guid Id { get; set; }
        public int SortOrder { get; set; }
        public int BestOf { get; protected set; }
        public DateTime StartDateTime { get; set; }
        public Guid Player1Id { get; set; }
        public PlayerDto Player1 { get; set; }
        public Guid Player2Id { get; set; }
        public PlayerDto Player2 { get; set; }
    }
}