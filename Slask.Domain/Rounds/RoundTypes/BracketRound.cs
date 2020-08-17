using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Procedures.AdvancingPerGroupCount;
using Slask.Domain.Procedures.PlayersPerGroupCount;
using Slask.Domain.Utilities;
using System;

namespace Slask.Domain.Rounds.RoundTypes
{
    public class BracketRound : RoundBase
    {
        private BracketRound()
        {
            AssignProcedures(new MutablePlayersPerGroupCountProcedure(), new ImmutableAdvancingPerGroupCountProcedure());
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
                ContestType = ContestTypeEnum.Bracket,
                PlayersPerGroupCount = 2,
                AdvancingPerGroupCount = 1,
                TournamentId = tournament.Id,
                Tournament = tournament
            };

            round.AssignDefaultName();

            return round;
        }

        protected override GroupBase AddGroup()
        {
            return BracketGroup.Create(this);
        }
    }
}
