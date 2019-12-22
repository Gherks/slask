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
            if (match == null || player == null)
            {
                // LOGG
                return false;
            }

            MatchBet newMatchBet = MatchBet.Create(this, match, player);
            MatchBet existingMatchBet = FindMatchBet(match);

            bool matchBetForThisMatchAlreadyExists = existingMatchBet != null;
            bool createdNewMatchBetSuccessfully = newMatchBet != null;

            if (createdNewMatchBetSuccessfully)
            {
                if (matchBetForThisMatchAlreadyExists)
                {
                    Bets.Remove(existingMatchBet);
                }

                Bets.Add(newMatchBet);
                return true;
            }

            return false;
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
