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

        public static Better Create(User user, Tournament tournament)
        {
            if (user == null || tournament == null)
            {
                return null;
            }

            return new Better
            {
                Id = Guid.NewGuid(),
                User = user,
                TournamentId = tournament.Id,
                Tournament = tournament
            };
        }

        public bool PlaceMatchBet(Match match, Player player)
        {
            if(match == null || player == null)
            {
                // LOGG
                return false;
            }

            if (!match.IsReady())
            {
                // LOGG MATCH IS NOT READY
                return false;
            }

            MatchBet matchBet = FindMatchBet(match);

            if (matchBet == null)
            {
                // LOGG THAT BET WAS CREATED
                Bets.Add(MatchBet.Create(match, player));
            }
            else
            {
                // LOGG THAT BET WAS UPDATED
                matchBet.UpdatePlayer(player);
            }

            return true;
        }

        private MatchBet FindMatchBet(Match match)
        {
            foreach (BetBase bet in Bets)
            {
                if (bet is MatchBet matchBet)
                {
                    if (matchBet.Match.Id == match.Id)
                    {
                        return matchBet;
                    }
                }
            }

            return null;
        }
    }
}
