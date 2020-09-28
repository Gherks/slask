using System;
using System.Collections.Generic;

namespace Slask.Dto
{
    public sealed class GroupDto
    {
        public Guid Id { get; set; }
        public string ContestType { get; set; }
        public int SortOrder { get; set; }
        public string Name { get; set; }
        public List<MatchDto> Matches { get; set; }
    }
}