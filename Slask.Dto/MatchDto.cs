using System;

namespace Slask.Dto
{
    public sealed class MatchDto
    {
        public Guid Id { get; set; }
        public int SortOrder { get; set; }
        public int BestOf { get; set; }
        public DateTime StartDateTime { get; set; }
        public Guid PlayerReference1Id { get; set; }
        public string Player1Name { get; set; }
        public int Player1Score { get; set; }
        public Guid PlayerReference2Id { get; set; }
        public string Player2Name { get; set; }
        public int Player2Score { get; set; }
    }
}