using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public class Round
    {
        private Round()
        {
            Groups = new List<Group>();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Type { get; private set; } // Should probably be expanded upon...
        public int BestOf { get; private set; }
        public int AdvanceAmount { get; private set; }
        public List<Group> Groups { get; private set; }
        public Guid TournamentId { get; private set; }
        public Tournament Tournament { get; private set; }

        public static Round Create(string name, int type, int bestOf, int advanceAmount)
        {
            return new Round
            {
                Id = Guid.NewGuid(),
                Name = name,
                Type = type,
                BestOf = bestOf,
                AdvanceAmount = advanceAmount
            };
        }

        public Group AddGroup()
        {
            throw new NotImplementedException();
        }
    }
}
