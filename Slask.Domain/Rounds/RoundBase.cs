using Slask.Domain.Groups;
using System;
using System.Collections.Generic;

namespace Slask.Domain.Rounds
{
    public class RoundBase
    {
        protected RoundBase()
        {
            Groups = new List<GroupBase>();
        }

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public int BestOf { get; protected set; }
        public int AdvancingPerGroupCount { get; protected set; }
        public List<GroupBase> Groups { get; protected set; }
        public Guid TournamentId { get; protected set; }
        public Tournament Tournament { get; protected set; }

        public virtual GroupBase AddGroup()
        {
            throw new NotImplementedException();
        }

        public RoundBase GetPreviousRound()
        {
            for (int index = 1; index < Tournament.Rounds.Count; ++index)
            {
                if (Tournament.Rounds[index].Id == Id)
                {
                    return Tournament.Rounds[index - 1];
                }
            }

            return null;
        }

        public List<PlayerReference> GetAdvancingPlayers()
        {
            List<PlayerReference> winningPlayers = new List<PlayerReference>();

            foreach (GroupBase group in Groups)
            {
                winningPlayers.AddRange(group.GetAdvancingPlayers());
            }

            return winningPlayers;
        }
    }
}
