using System.Collections.Generic;

namespace Slask.Data.Models
{
    public class MiscBetCatalogue
    {
        MiscBetCatalogue()
        {
            MiscBetPlayerEntries = new List<MiscBetPlayerEntry>();
        }

        public int Id { get; set; }
        public List<MiscBetPlayerEntry> MiscBetPlayerEntries;
        public int Type { get; set; } // Should probably be expanded upon...
    }
}
