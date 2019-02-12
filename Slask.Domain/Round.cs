using System.Collections.Generic;

namespace Slask.Domain
{
    public class Round
    {
        Round()
        {
            Groups = new List<Group>();
        }

        public int Id { get; set; }
        public List<Group> Groups { get; set; }
        public string Name { get; set; }
        public int Type { get; set; } // Should probably be expanded upon...
        public int BestOf { get; set; }
        public int AdvanceNum { get; set; }
    }
}
