using Slask.Domain.Bets;
using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public class Better
    {
        private Better()
        {
            MatchBets = new List<MatchBet>();
            MiscBets = new List<MiscBet>();
        }

        public Guid Id { get; private set; }
        public User User { get; private set; }
        public List<MatchBet> MatchBets { get; private set; }
        public List<MiscBet> MiscBets { get; private set; }
        public Guid TournamentId { get; private set; }
        public Tournament Tournament { get; private set; }

        public static Better Create(User user)
        {
            return new Better
            {
                Id = Guid.NewGuid(),
                User = user
            };
        }
    }
}
