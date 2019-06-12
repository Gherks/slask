using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slask.Domain
{
    public class Match
    {
        private Match()
        {
            MatchPlayers = new List<MatchPlayer>();
        }

        private Match(MatchPlayer matchPlayer1, MatchPlayer matchPlayer2)
        {
            MatchPlayers = new List<MatchPlayer>();
            MatchPlayers.Add(matchPlayer1);
            MatchPlayers.Add(matchPlayer2);
        }

        public Guid Id { get; private set; }
        private List<MatchPlayer> MatchPlayers { get; set; }
        [NotMapped]
        public MatchPlayer MatchPlayer1
        {
            get { return MatchPlayers.Count >= 1 ? MatchPlayers[0] : null; }
            private set {}
        }
        [NotMapped]
        public MatchPlayer MatchPlayer2
        {
            get { return MatchPlayers.Count >= 2 ? MatchPlayers[1] : null; }
            private set {}
        }
        public DateTime StartDateTime { get; private set; }
        public Guid GroupId { get; private set; }
        public Group Group { get; private set; }

        public static Match Create(MatchPlayer matchPlayer1, MatchPlayer matchPlayer2)
        {
            if (matchPlayer1 == null || matchPlayer2 == null)
            {
                // LOG ISSUE HERE
                return null;
            }

            Match match = new Match(matchPlayer1, matchPlayer2)
            {
                Id = Guid.NewGuid(),
                StartDateTime = DateTime.Now
            };

            return match;
        }
    }
}
