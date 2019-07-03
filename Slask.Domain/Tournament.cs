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
            PlayerNameReferences = new List<PlayerNameReference>();
            Betters = new List<Better>();
            Settings = new List<Settings>();
            MiscBetCatalogue = new List<MiscBetCatalogue>();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public List<Round> Rounds { get; private set; }
        public List<PlayerNameReference> PlayerNameReferences { get; private set; }
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

        public Round AddRound(string name, int type, int bestOf, int advanceAmount)
        {
            throw new NotImplementedException();
        }

        public Better AddBetter(User user)
        {
            throw new NotImplementedException();
        }

        public PlayerNameReference GetPlayerNameReferenceByPlayerNameReferenceId(Guid id)
        {
            return PlayerNameReferences.FirstOrDefault(playerNameReference => playerNameReference.Id == id);
        }

        public PlayerNameReference GetPlayerNameReferenceByPlayerName(string name)
        {
            string lowerCaseName = name.ToLower();

            return PlayerNameReferences.FirstOrDefault(playerNameReference => playerNameReference.Name.ToLower().Contains(lowerCaseName));
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
