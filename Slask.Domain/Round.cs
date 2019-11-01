using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain // Change to 'Slask.Domain.Round' so the Enum makes more sense, what happens to migrations?
{
    public enum RoundType
    {
        RoundRobin,
        DualTournament,
        Bracket
    }

    public class Round
    {
        private Round()
        {
            Groups = new List<GroupBase>();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public RoundType Type { get; private set; }
        public int BestOf { get; private set; }
        public int AdvancingPerGroupAmount { get; private set; }
        public List<GroupBase> Groups { get; private set; }
        public Guid TournamentId { get; private set; }
        public Tournament Tournament { get; private set; }

        public static Round Create(string name, RoundType type, int bestOf, int advancingPerGroupAmount, Tournament tournament)
        {
            if (tournament == null)
            {
                return null;
            }

            return new Round
            {
                Id = Guid.NewGuid(),
                Name = name,
                Type = type,
                BestOf = bestOf,
                AdvancingPerGroupAmount = advancingPerGroupAmount,
                TournamentId = tournament.Id,
                Tournament = tournament
            };
        }

        public GroupBase AddGroup()
        {
            switch (Type)
            {
                case RoundType.RoundRobin:
                    Groups.Add(RoundRobinGroup.Create(this));
                    break;
                case RoundType.DualTournament:
                    Groups.Add(DualTournamentGroup.Create(this));
                    break;
                case RoundType.Bracket:
                    Groups.Add(BracketGroup.Create(this));
                    break;
                default:
                    return null;
            }

            return Groups.Last();
        }

        public Round GetPreviousRound()
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

            foreach(GroupBase group in Groups)
            {
                winningPlayers.AddRange(group.GetAdvancingPlayers());
            }

            return winningPlayers;
        }
    }
}
