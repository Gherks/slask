using Slask.Domain.Groups;
using System;
using System.Linq;

namespace Slask.Domain.Rounds
{
    public class BracketRound : RoundBase
    {
        private BracketRound()
        {
        }

        public static BracketRound Create(string name, int bestOf, Tournament tournament)
        {
            if (!InitialValidationSucceeds(name, bestOf) || tournament == null)
            {
                return null;
            }

            return new BracketRound
            {
                Id = Guid.NewGuid(),
                Name = name,
                BestOf = bestOf,
                AdvancingPerGroupCount = 1,
                TournamentId = tournament.Id,
                Tournament = tournament
            };
        }

        protected override GroupBase AddGroup()
        {
            return BracketGroup.Create(this);
        }

        protected static bool InitialValidationSucceeds(string name, int bestOf)
        {
            bool nameIsNotEmpty = name.Length > 0;
            bool bestOfIsNotEven = bestOf % 2 != 0;
            bool bestOfIsGreaterThanZero = bestOf > 0;

            return nameIsNotEmpty && bestOfIsNotEven && bestOfIsGreaterThanZero;
        }
    }
}
