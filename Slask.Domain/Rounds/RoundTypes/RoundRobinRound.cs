﻿using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Procedures.AdvancingPerGroupCount;
using Slask.Domain.Procedures.PlayersPerGroupCount;
using Slask.Domain.Utilities;

namespace Slask.Domain.Rounds.RoundTypes
{
    public class RoundRobinRound : RoundBase
    {
        private RoundRobinRound()
        {
            ContestType = ContestTypeEnum.RoundRobin;
            PlayersPerGroupCount = 2;
            AdvancingPerGroupCount = 1;

            AssignProcedures(new MutablePlayersPerGroupCountProcedure(), new MutableAdvancingPerGroupCountProcedure());
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
                TournamentId = tournament.Id,
                Tournament = tournament
            };

            round.AssignDefaultName();

            return round;
        }

        protected override GroupBase AddGroup()
        {
            return RoundRobinGroup.Create(this);
        }
    }
}
