using Slask.Domain.Bets;
using Slask.Domain.Bets.BetTypes;
using Slask.Domain.ObjectState;
using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public class Better : ObjectStateBase
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
                Tournament = tournament,
                ObjectState = ObjectStateEnum.Added
            };
        }

        public MatchBet PlaceMatchBet(Match match, Player player)
        {
            bool anyParameterIsInvalid = !PlaceMatchBetParametersAreValid(match, player);

            if (anyParameterIsInvalid)
            {
                return null;
            }

            MatchBet newMatchBet = MatchBet.Create(this, match, player);
            MatchBet existingMatchBet = FindMatchBet(match);

            bool createdNewMatchBetSuccessfully = newMatchBet != null;
            bool matchBetForThisMatchAlreadyExists = existingMatchBet != null;

            if (createdNewMatchBetSuccessfully)
            {
                if (matchBetForThisMatchAlreadyExists)
                {
                    Bets.Remove(existingMatchBet);
                    existingMatchBet.MarkForDeletion();
                }

                Bets.Add(newMatchBet);
                MarkAsModified();
            }

            return newMatchBet;
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

        private static bool PlaceMatchBetParametersAreValid(Match match, Player player)
        {
            bool invalidMatchGiven = match == null;

            if (invalidMatchGiven)
            {
                // LOG Error: Cannot place match bet because given match was invalid
                return false;
            }

            bool invalidPlayerGiven = player == null;

            if (invalidPlayerGiven)
            {
                // LOG Error: Cannot place match bet because given player was invalid
                return false;
            }

            return true;
        }
    }
}
