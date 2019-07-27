using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain
{
    public class Tournament
    {
        private Tournament()
        {
            Rounds = new List<Round>();
            PlayerReferences = new List<PlayerReference>();
            Betters = new List<Better>();
            Settings = new List<Settings>();
            MiscBetCatalogue = new List<MiscBetCatalogue>();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public List<Round> Rounds { get; private set; }
        public List<PlayerReference> PlayerReferences { get; private set; }
        public List<Better> Betters { get; private set; }
        public List<Settings> Settings { get; private set; }
        public List<MiscBetCatalogue> MiscBetCatalogue { get; private set; }

        public static Tournament Create(string name)
        {
            return new Tournament
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }

        public void RenameTo(string name)
        {
            name = name.Trim();
            if (name.Length > 0)
            {
                Name = name;
            }
        }

        public Round AddRoundRobinRound(string name, int bestOf, int advanceAmount)
        {
            throw new NotImplementedException();
        }

        public Round AddDualTournamentRound(string name, int bestOf)
        {
            throw new NotImplementedException();
        }

        public Round AddBracketRound(string name, int bestOf)
        {
            throw new NotImplementedException();
        }

        public Better AddBetter(User user)
        {
            throw new NotImplementedException();
        }

        public Round GetRoundByRoundId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Round GetRoundByRoundName(string name)
        {
            throw new NotImplementedException();
        }

        public PlayerReference GetPlayerReferenceByPlayerId(Guid id)
        {
            return PlayerReferences.FirstOrDefault(playerReference => playerReference.Id == id);
        }

        public PlayerReference GetPlayerReferenceByPlayerName(string name)
        {
            string lowerCaseName = name.ToLower();

            return PlayerReferences.FirstOrDefault(playerReference => playerReference.Name.ToLower() == lowerCaseName);
        }

        public Better GetBetterById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Better GetBetterByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
