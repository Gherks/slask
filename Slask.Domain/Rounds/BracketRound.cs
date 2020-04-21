using Slask.Domain.Groups;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds.Bases;
using System;
using System.Linq;

namespace Slask.Domain.Rounds
{
    public class BracketRound : ResizableRound
    {
        private BracketRound()
        {
        }

        public static BracketRound Create(string name, int bestOf, int playersPerGroupCount, Tournament tournament)
        {
            if (!InitialValidationSucceeds(name, bestOf, playersPerGroupCount) || tournament == null)
            {
                return null;
            }

            BracketRound round = new BracketRound
            {
                Id = Guid.NewGuid(),
                Name = name,
                PlayersPerGroupCount = playersPerGroupCount,
                BestOf = bestOf,
                AdvancingPerGroupCount = 1,
                TournamentId = tournament.Id,
                Tournament = tournament
            };

            return round;
        }

        protected override GroupBase AddGroup()
        {
            return BracketGroup.Create(this);
        }

        protected static bool InitialValidationSucceeds(string name, int bestOf, int playersPerGroupCount)
        {
            bool nameIsNotEmpty = name.Length > 0;
            bool bestOfIsNotEven = bestOf % 2 != 0;
            bool bestOfIsGreaterThanZero = bestOf > 0;
            bool playersPerGroupCountIsGreaterThanZero = playersPerGroupCount >= 2;

            return nameIsNotEmpty && bestOfIsNotEven && bestOfIsGreaterThanZero && playersPerGroupCountIsGreaterThanZero;
        }
    }
}
