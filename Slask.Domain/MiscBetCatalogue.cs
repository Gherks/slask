using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public class MiscBetCatalogue
    {
        private MiscBetCatalogue()
        {
            MiscBetPlayerEntries = new List<MiscBetPlayerEntry>();
        }

        public Guid Id { get; private set; }
        public int Type { get; private set; } // Should probably be expanded upon...
        public List<MiscBetPlayerEntry> MiscBetPlayerEntries { get; private set; }
        public Guid TournamentId { get; private set; }
        public Tournament Tournament { get; private set; }

        public static MiscBetCatalogue Create(int type, Tournament tournament)
        {
            if (tournament == null)
            {
                return null;
            }

            return new MiscBetCatalogue
            {
                Id = Guid.NewGuid(),
                Type = type,
                TournamentId = tournament.Id,
                Tournament = tournament
            };
        }
    }
}
