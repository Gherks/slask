using Slask.Data.Models.Tournament;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slask.Data.Models
{
    public class TournamentModel
    {
        public Guid Id { get; set; }
        public IList<RoundModel> Rounds { get; set; }
        public IList<PlayerModel> Players { get; set; }
        public IList<BetterModel> Betters { get; set; }
        public IList<SettingsModel> Settings { get; set; }
    }
}
