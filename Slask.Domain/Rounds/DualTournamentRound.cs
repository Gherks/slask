using Slask.Domain.Groups;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Procedures.AdvancingPerGroupCount;
using Slask.Domain.Procedures.PlayersPerGroupCount;
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

        public static DualTournamentRound Create(Tournament tournament)
        {
            bool givenTournamentIsInvalid = tournament == null;

            if (givenTournamentIsInvalid)
            {
                return null;
            }

            DualTournamentRound round = new DualTournamentRound
            {
                Id = Guid.NewGuid(),
                PlayersPerGroupCount = 4,
                BestOf = 3,
                AdvancingPerGroupCount = 2,
                TournamentId = tournament.Id,
                Tournament = tournament
            };

            round.AssignDefaultName();
            round.AssignProcedures(new ImmutablePlayersPerGroupCountProcedure());

            return round;
        }

        public override bool SetAdvancingPerGroupCount(int count)
        {
            return false;
        }

        protected override GroupBase AddGroup()
        {
            return DualTournamentGroup.Create(this);
        }
    }
}
