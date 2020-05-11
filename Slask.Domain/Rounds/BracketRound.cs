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

        public static BracketRound Create(Tournament tournament)
        {
            bool givenTournamentIsInvalid = tournament == null;

            if (givenTournamentIsInvalid)
            {
                return null;
            }

            BracketRound round = new BracketRound
            {
                Id = Guid.NewGuid(),
                PlayersPerGroupCount = 2,
                BestOf = 3,
                AdvancingPerGroupCount = 1,
                TournamentId = tournament.Id,
                Tournament = tournament
            };

            round.AssignDefaultName();

            return round;
        }

        public override bool SetAdvancingPerGroupCount(int count)
        {
            return false;
        }

        protected override GroupBase AddGroup()
        {
            return BracketGroup.Create(this);
        }
    }
}
