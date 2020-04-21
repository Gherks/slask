using Slask.Domain.Groups;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds.Bases;
using System;
using System.Linq;

namespace Slask.Domain.Rounds
{
    public class DualTournamentRound : RoundBase
    {
        private DualTournamentRound()
        {
        }

        public static DualTournamentRound Create(string name, int bestOf, Tournament tournament)
        {
            if (!InitialValidationSucceeds(name, bestOf) || tournament == null)
            {
                return null;
            }

            DualTournamentRound round = new DualTournamentRound
            {
                Id = Guid.NewGuid(),
                Name = name,
                PlayersPerGroupCount = 4,
                BestOf = bestOf,
                AdvancingPerGroupCount = 2,
                TournamentId = tournament.Id,
                Tournament = tournament
            };

            return round;
        }

        protected override GroupBase AddGroup()
        {
            return DualTournamentGroup.Create(this);
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
