﻿using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using Slask.Domain.Rounds.RoundUtilities;
using Slask.Domain.Utilities;
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
            TournamentIssueReporter = new TournamentIssueReporter();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public List<RoundBase> Rounds { get; private set; }
        public List<Better> Betters { get; private set; }
        public List<Settings> Settings { get; private set; }
        public List<MiscBetCatalogue> MiscBetCatalogue { get; private set; }

        // Ignored by SlaskContext
        public TournamentIssueReporter TournamentIssueReporter { get; private set; }

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

        public RoundBase AddBracketRound(string name, int bestOf, int playersPerGroupCount = 2)
        {
            RoundBase round = BracketRound.Create(name, bestOf, playersPerGroupCount, this);

            if (round == null)
            {
                return null;
            }

            IntegrateRoundToTournament(round);
            return round;
        }

        public RoundBase AddDualTournamentRound(string name, int bestOf)
        {
            RoundBase round = DualTournamentRound.Create(name, bestOf, this);

            if (round == null)
            {
                return null;
            }

            IntegrateRoundToTournament(round);
            return round;
        }

        public RoundBase AddRoundRobinRound(string name, int bestOf, int advancingPerGroupCount, int playersPerGroupCount = 2)
        {
            RoundBase round = RoundRobinRound.Create(name, bestOf, advancingPerGroupCount, playersPerGroupCount, this);

            if (round == null)
            {
                return null;
            }

            IntegrateRoundToTournament(round);
            return round;
        }

        public bool RemoveRound(RoundBase round)
        {
            bool soughtRoundIsValid = round != null;
            bool tournamentHasNotBegun = GetFirstRound().GetPlayState() == PlayState.NotBegun;

            if (soughtRoundIsValid && tournamentHasNotBegun)
            {
                return ManageRoundRemoval(round);
            }

            return false;
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

        public void FindIssues()
        {
            TournamentIssueReporter.Clear();

            foreach (RoundBase round in Rounds)
            {
                RoundIssueFinder.FindIssues(round);
            }
        }

        public RoundBase GetRoundByRoundId(Guid id)
        {
            return Rounds.FirstOrDefault(round => round.Id == id);
        }

        public RoundBase GetRoundByRoundName(string name)
        {
            return Rounds.FirstOrDefault(round => round.Name.ToLower() == name.ToLower());
        }

        public RoundBase GetFirstRound()
        {
            bool hasNoRounds = Rounds.Count == 0;

            if (hasNoRounds)
            {
                return null;
            }

            return Rounds.First();
        }

        public RoundBase GetLastRound()
        {
            bool hasNoRounds = Rounds.Count == 0;

            if (hasNoRounds)
            {
                return null;
            }

            return Rounds.Last();
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

            if (tournamentHasNoRounds)
            {
                return new List<PlayerReference>();
            }

            return Rounds.First().PlayerReferences;
        }

        private void IntegrateRoundToTournament(RoundBase round)
        {
            Rounds.Add(round);
            round.Construct();
            FindIssues();
        }

        private bool ManageRoundRemoval(RoundBase round)
        {
            bool containsSoughtRound = Rounds.Contains(round);

            if (containsSoughtRound)
            {
                bool soughtRoundIsTheFirstOne = GetFirstRound().Id == round.Id;
                List<PlayerReference> playerReferences = new List<PlayerReference>();

                if (soughtRoundIsTheFirstOne)
                {
                    playerReferences = round.PlayerReferences;
                }

                bool removalSuccessful = Rounds.Remove(round);

                if (removalSuccessful)
                {
                    bool stillContainsRounds = Rounds.Count > 0;

                    if(stillContainsRounds)
                    {
                        RoundBase firstRound = GetFirstRound();

                        foreach (PlayerReference playerReference in playerReferences)
                        {
                            firstRound.RegisterPlayerReference(playerReference.Name);
                        }

                        firstRound.Construct();
                    }

                    FindIssues();
                }
            }

            return false;
        }
    }
}
