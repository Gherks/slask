using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public class Group
    {
        Group()
        {
            Matches = new List<Match>();
        }

        public Guid Id { get; set; }
        public List<Match> Matches { get; set; }
    }
}
