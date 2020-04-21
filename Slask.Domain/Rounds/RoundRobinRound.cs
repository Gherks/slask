using Slask.Domain.Groups;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds.Bases;
using System;
using System.Linq;

namespace Slask.Domain.Rounds
{
    public class RoundRobinRound : ResizableRound
    {
        private RoundRobinRound()
        {
        }

        public static RoundRobinRound Create(string name, int bestOf, int advancingPerGroupCount, int playersPerGroupCount, Tournament tournament)
        {
            bool validationFails = !InitialValidationSucceeds(name, bestOf, advancingPerGroupCount, playersPerGroupCount);
            bool givenTournamentIsInvalid = tournament == null;

            if (validationFails || givenTournamentIsInvalid)
            {
                return null;
            }

            RoundRobinRound round = new RoundRobinRound
            {
                Id = Guid.NewGuid(),
                Name = name,
                PlayersPerGroupCount = playersPerGroupCount,
                BestOf = bestOf,
                AdvancingPerGroupCount = advancingPerGroupCount,
                TournamentId = tournament.Id,
                Tournament = tournament
            };

            return round;
        }

        protected override GroupBase AddGroup()
        {
            return RoundRobinGroup.Create(this);
        }

        public static bool InitialValidationSucceeds(string name, int bestOf, int advancingPerGroupCount, int playersPerGroupCount)
        {
            bool nameIsNotEmpty = name.Length > 0;
            bool bestOfIsNotEven = bestOf % 2 != 0;
            bool bestOfIsGreaterThanZero = bestOf > 0;
            bool advancingPerGroupCountIsGreaterThanZero = advancingPerGroupCount > 0;
            bool playersPerGroupCountIsGreaterThanZero = playersPerGroupCount >= 2;

            return nameIsNotEmpty && bestOfIsNotEven && bestOfIsGreaterThanZero && advancingPerGroupCountIsGreaterThanZero && playersPerGroupCountIsGreaterThanZero;
        }
    }
}
