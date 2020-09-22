using System;
using System.Collections.Generic;

namespace Slask.Dto
{
    public class RoundDto
    {
        public Guid Id { get; set; }
        public string ContestType { get; set; }
        public string Name { get; set; }
        public List<GroupDto> Groups { get; set; }
    }
}