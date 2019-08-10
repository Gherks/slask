using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Slask.Domain
{
    public class DualTournamentGroup : GroupBase
    {
        private DualTournamentGroup()
        {
        }

        [NotMapped]
        private const int ParticipatingPlayerCapacity = 4;

        public static DualTournamentGroup Create(Round round)
        {
            if (round == null)
            {
                return null;
            }

            DualTournamentGroup group = new DualTournamentGroup
            {
                Id = Guid.NewGuid(),
                IsReady = false,
                RoundId = round.Id,
                Round = round
            };

            group.Matches.Add(Match.Create(group));
            group.Matches.Add(Match.Create(group));
            group.Matches.Add(Match.Create(group));
            group.Matches.Add(Match.Create(group));
            group.Matches.Add(Match.Create(group));

            return group;
        }

        public override void Clear()
        {
            ParticipatingPlayers.Clear();
            Matches.Clear();
            IsReady = false;

            Matches.Add(Match.Create(this));
            Matches.Add(Match.Create(this));
            Matches.Add(Match.Create(this));
            Matches.Add(Match.Create(this));
            Matches.Add(Match.Create(this));
        }
        public override void AddPlayerReference(string name)
        {
            if (ParticipatingPlayers.Count < ParticipatingPlayerCapacity)
            {
                base.AddPlayerReference(name);
            }
        }

        protected override void UpdateMatchLayout()
        {
            for(int index = 0; index < ParticipatingPlayers.Count; index += 2)
            {
                Matches[index].AssignPlayerReferences(ParticipatingPlayers[index], ParticipatingPlayers[index + 1]);
            }
        }

        public override void MatchScoreChanged(Match match)
        {
        }
    }
}
