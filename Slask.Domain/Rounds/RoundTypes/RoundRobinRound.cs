using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Procedures.AdvancingPerGroupCount;
using Slask.Domain.Procedures.PlayersPerGroupCount;
using Slask.Domain.Utilities;
using System;

namespace Slask.Domain.Rounds.RoundTypes
{
    public class RoundRobinRound : RoundBase
    {
        private RoundRobinRound()
        {
        }

        public static RoundRobinRound Create(Tournament tournament)
        {
            bool givenTournamentIsInvalid = tournament == null;

            if (givenTournamentIsInvalid)
            {
                return null;
            }

            RoundRobinRound round = new RoundRobinRound
            {
                Id = Guid.NewGuid(),
                ContestType = ContestTypeEnum.RoundRobin,
                PlayersPerGroupCount = 2,
                AdvancingPerGroupCount = 1,
                TournamentId = tournament.Id,
                Tournament = tournament
            };

            round.AssignDefaultName();
            round.AssignProcedures(new MutablePlayersPerGroupCountProcedure(), new MutableAdvancingPerGroupCountProcedure());

            return round;
        }

        protected override GroupBase AddGroup()
        {
            return RoundRobinGroup.Create(this);
        }
    }
}
