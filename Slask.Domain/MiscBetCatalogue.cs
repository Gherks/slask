using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public class MiscBetCatalogue
    {
        MiscBetCatalogue()
        {
            MiscBetPlayerEntries = new List<MiscBetPlayerEntry>();
        }

        public Guid Id { get; set; }
        public List<MiscBetPlayerEntry> MiscBetPlayerEntries { get; set; }
        public int Type { get; set; } // Should probably be expanded upon...
    }
}
