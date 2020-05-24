using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Procedures.AdvancingPerGroupCount;
using Slask.Domain.Procedures.PlayersPerGroupCount;
using System;

namespace Slask.Domain.Rounds.RoundTypes
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
            round.AssignProcedures(new ImmutablePlayersPerGroupCountProcedure(), new ImmutableAdvancingPerGroupCountProcedure());

            return round;
        }

        protected override GroupBase AddGroup()
        {
            return DualTournamentGroup.Create(this);
        }
    }
}
