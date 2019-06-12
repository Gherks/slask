﻿using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public class Tournament
    {
        private Tournament()
        {
            Rounds = new List<Round>();
            Players = new List<Player>();
            Betters = new List<Better>();
            Settings = new List<Settings>();
            MiscBetCatalogue = new List<MiscBetCatalogue>();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public List<Round> Rounds { get; private set; }
        public List<Player> Players { get; private set; }
        public List<Better> Betters { get; private set; }
        public List<Settings> Settings { get; private set; }
        public List<MiscBetCatalogue> MiscBetCatalogue { get; private set; }

        public void Rename(string name)
        {
            name = name.Trim();
            if(name.Length > 0)
            {
                Name = name;
            }
        }

        public Round AddRound(string name, int type, int bestOf, int advanceAmount)
        {
            throw new NotImplementedException();
        }

        public static Tournament Create(string name)
        {
            return new Tournament
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }

        public Better AddBetter(User user)
        {
            throw new NotImplementedException();
        }
    }
}
