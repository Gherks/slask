using Slask.Domain.Groups;
using System;
using System.Linq;

namespace Slask.Domain.Rounds
{
    public class RoundRobinRound : RoundBase
    {
        private RoundRobinRound()
        {
        }

        public static RoundRobinRound Create(string name, int bestOf, int advancingPerGroupAmount, Tournament tournament)
        {
            if (!InitialValidationSucceeds(name, bestOf, advancingPerGroupAmount) || tournament == null)
            {
                return null;
            }

            return new RoundRobinRound
            {
                Id = Guid.NewGuid(),
                Name = name,
                BestOf = bestOf,
                AdvancingPerGroupCount = advancingPerGroupAmount,
                TournamentId = tournament.Id,
                Tournament = tournament
            };
        }

        public override GroupBase AddGroup()
        {
            Groups.Add(RoundRobinGroup.Create(this));
            return Groups.Last();
        }

        public static bool InitialValidationSucceeds(string name, int bestOf, int advanceAmount)
        {
            bool nameIsNotEmpty = name.Length > 0;
            bool bestOfIsNotEven = bestOf % 2 != 0;
            bool bestOfIsGreaterThanZero = bestOf > 0;
            bool advanceAmountIsGreaterThanZero = advanceAmount > 0;

            return nameIsNotEmpty && bestOfIsNotEven && bestOfIsGreaterThanZero && advanceAmountIsGreaterThanZero;
        }
    }
}
