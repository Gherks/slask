﻿using Slask.Domain.Groups;
using System;
using System.Linq;

namespace Slask.Domain.Rounds
{
    public class RoundRobinRound : RoundBase
    {
        private RoundRobinRound()
        {
        }

        public static RoundRobinRound Create(string name, int bestOf, int advancingPerGroupCount, Tournament tournament)
        {
            if (!InitialValidationSucceeds(name, bestOf, advancingPerGroupCount) || tournament == null)
            {
                return null;
            }

            return new RoundRobinRound
            {
                Id = Guid.NewGuid(),
                Name = name,
                BestOf = bestOf,
                AdvancingPerGroupCount = advancingPerGroupCount,
                TournamentId = tournament.Id,
                Tournament = tournament
            };
        }

        protected override GroupBase AddGroup()
        {
            return RoundRobinGroup.Create(this);
        }

        public static bool InitialValidationSucceeds(string name, int bestOf, int advanceAmount)
        {
            bool nameIsNotEmpty = name.Length > 0;
            bool bestOfIsNotEven = bestOf % 2 != 0;
            bool bestOfIsGreaterThanZero = bestOf > 0;
            bool advanceAmountIsGreaterThanZero = advanceAmount > 0;

            return nameIsNotEmpty && bestOfIsNotEven && bestOfIsGreaterThanZero && advanceAmountIsGreaterThanZero;
        }
    }
}
