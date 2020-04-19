using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain
{
    public class Tournament
    {
        private Tournament()
        {
            Rounds = new List<RoundBase>();
            Betters = new List<Better>();
            Settings = new List<Settings>();
            MiscBetCatalogue = new List<MiscBetCatalogue>();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public List<RoundBase> Rounds { get; private set; }
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

        public RoundBase AddBracketRound(string name, int bestOf)
        {
            RoundBase round = BracketRound.Create(name, bestOf, this);

            if (round == null)
            {
                return null;
            }

            Rounds.Add(round);
            round.Construct();

            return round;
        }

        public RoundBase AddDualTournamentRound(string name, int bestOf)
        {
            RoundBase round = DualTournamentRound.Create(name, bestOf, this);

            if (round == null)
            {
                return null;
            }

            Rounds.Add(round);
            round.Construct();

            return round;
        }

        public RoundBase AddRoundRobinRound(string name, int bestOf, int advancingPerGroupCount)
        {
            RoundBase round = RoundRobinRound.Create(name, bestOf, advancingPerGroupCount, this);

            if (round == null)
            {
                return null;
            }

            Rounds.Add(round);
            round.Construct();

            return round;
        }

        public Better AddBetter(User user)
        {
            bool betterAlreadyExists = GetBetterByName(user.Name) != null;

            if (betterAlreadyExists)
            {
                return null;
            }

            Betters.Add(Better.Create(user, this));

            return Betters.Last();
        }

        public RoundBase GetRoundByRoundId(Guid id)
        {
            return Rounds.FirstOrDefault(round => round.Id == id);
        }

        public RoundBase GetRoundByRoundName(string name)
        {
            return Rounds.FirstOrDefault(round => round.Name.ToLower() == name.ToLower());
        }

        public PlayerReference GetPlayerReferenceByPlayerId(Guid id)
        {
            return GetPlayerReferences().FirstOrDefault(playerReference => playerReference.Id == id);
        }

        public PlayerReference GetPlayerReferenceByPlayerName(string name)
        {
            return GetPlayerReferences().FirstOrDefault(playerReference => playerReference.Name.ToLower() == name.ToLower());
        }

        public Better GetBetterById(Guid id)
        {
            return Betters.FirstOrDefault(better => better.Id == id);
        }

        public Better GetBetterByName(string name)
        {
            return Betters.FirstOrDefault(better => better.User.Name.ToLower() == name.ToLower());
        }

        public List<PlayerReference> GetPlayerReferences()
        {
            bool tournamentHasNoRounds = Rounds.Count == 0;

            if(tournamentHasNoRounds)
            {
                // LOGG Error: 
                return new List<PlayerReference>();
            }

            return Rounds.First().PlayerReferences;
        }
    }
}
