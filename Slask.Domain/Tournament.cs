using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
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
            return Rounds.Last();
        }

        public RoundBase AddDualTournamentRound(string name, int bestOf)
        {
            RoundBase round = DualTournamentRound.Create(name, bestOf, this);

            if (round == null)
            {
                return null;
            }

            Rounds.Add(round);
            return Rounds.Last();
        }

        public RoundBase AddRoundRobinRound(string name, int bestOf, int advanceAmount)
        {
            RoundBase round = RoundRobinRound.Create(name, bestOf, advanceAmount, this);

            if (round == null)
            {
                return null;
            }

            Rounds.Add(round);
            return Rounds.Last();
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
            return GetPlayerReferencesInTournament().FirstOrDefault(playerReference => playerReference.Id == id);
        }

        public PlayerReference GetPlayerReferenceByPlayerName(string name)
        {
            return GetPlayerReferencesInTournament().FirstOrDefault(playerReference => playerReference.Name.ToLower() == name.ToLower());
        }

        public Better GetBetterById(Guid id)
        {
            return Betters.FirstOrDefault(better => better.Id == id);
        }

        public Better GetBetterByName(string name)
        {
            return Betters.FirstOrDefault(better => better.User.Name.ToLower() == name.ToLower());
        }

        public List<PlayerReference> GetPlayerReferencesInTournament()
        {
            bool tournamentHasNoRounds = Rounds.Count == 0;

            if(tournamentHasNoRounds)
            {
                // LOGG Error: 
                return new List<PlayerReference>();
            }

            //Dictionary<string, PlayerReference> playerReferenceDictionary = new Dictionary<string, PlayerReference>();

            ////foreach (RoundBase round in Rounds)
            //{
            //    //foreach (GroupBase group in Rounds.First().Groups)
            //    {
            //        foreach(PlayerReference playerReference in group.ParticipatingPlayers)
            //        {
            //            try
            //            {
            //                playerReferenceDictionary.Add(playerReference.Name, playerReference);
            //            } catch (Exception) { }
            //        }
            //    }
            //}

            //return playerReferenceDictionary.Values.ToList();

            return Rounds.First().PlayerReferences;
        }
    }
}
