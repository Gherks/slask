using Slask.Domain.Bets;
using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public class Better
    {
        private Better()
        {
            Bets = new List<BetBase>();
        }

        public Guid Id { get; private set; }
        public User User { get; private set; }
        public List<BetBase> Bets { get; private set; }
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

        public void PlaceMatchBet(Match match, Player player)
        {
            throw new NotImplementedException();
        }
    }
}
