using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Procedures.AdvancingPerGroupCount;
using Slask.Domain.Procedures.PlayersPerGroupCount;
using Slask.Domain.Utilities;
using System;

namespace Slask.Domain.Rounds.RoundTypes
{
    public class DualTournamentRound : RoundBase
    {
        private DualTournamentRound()
        {
            PlayersPerGroupCount = 4;
            AdvancingPerGroupCount = 2;

            AssignProcedures(new ImmutablePlayersPerGroupCountProcedure(), new ImmutableAdvancingPerGroupCountProcedure());
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
                ContestType = ContestTypeEnum.DualTournament,
                TournamentId = tournament.Id,
                Tournament = tournament
            };

            round.AssignDefaultName();

            return round;
        }

        protected override GroupBase AddGroup()
        {
            return DualTournamentGroup.Create(this);
        }
    }
}
