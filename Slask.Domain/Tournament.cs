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

        public void ChangeName(string name)
        {
            Name = name;
        }

        public Round AddRoundRobinRound(string name, int bestOf, int advanceAmount)
        {
            Rounds.Add(Round.Create(name, RoundType.RoundRobin, bestOf, advanceAmount));
            return Rounds.Last();
        }

        public Round AddDualTournamentRound(string name, int bestOf)
        {
            throw new NotImplementedException();
        }

        public Round AddBracketRound(string name, int bestOf)
        {
            Rounds.Add(Round.Create(name, RoundType.RoundRobin, bestOf, 1));
            return Rounds.Last();
        }

        public Better AddBetter(User user)
        {
            bool betterAlreadyExists = GetBetterByName(user.Name) != null;

            if(betterAlreadyExists)
            {
                return null;
            }

            Betters.Add(Better.Create(user, this));

            return Betters.Last();
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
            return PlayerReferences.FirstOrDefault(playerReference => playerReference.Name.ToLower() == name.ToLower());
        }

        public Better GetBetterById(Guid id)
        {
            return Betters.FirstOrDefault(better => better.Id == id);
        }

        public Better GetBetterByName(string name)
        {
            return Betters.FirstOrDefault(better => better.User.Name.ToLower() == name.ToLower());
        }
    }
}
