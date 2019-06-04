using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public class Tournament
    {
        public Tournament()
        {
            Rounds = new List<Round>();
            Players = new List<Player>();
            Betters = new List<Better>();
            Settings = new List<Settings>();
            MiscBetCatalogue = new List<MiscBetCatalogue>();
        }

        public Guid Id { get; set; }
        public List<Round> Rounds { get; set; }
        public List<Player> Players { get; set; }
        public List<Better> Betters { get; set; }
        public List<Settings> Settings { get; set; }
        public List<MiscBetCatalogue> MiscBetCatalogue { get; set; }
    }
}
