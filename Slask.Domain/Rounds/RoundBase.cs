﻿using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        [NotMapped]
        public int AdvancingCount
        {
            get { return AdvancingPerGroupCount * Groups.Count; }
            private set { }
        }

        // CREATE TESTS
        public bool IsFirstRound()
        {
            bool belongsToATournament = Tournament != null;

            if (belongsToATournament)
            {
                bool tournamentHasRounds = Tournament.Rounds.Count > 0;

                if (tournamentHasRounds)
                {
                    return Id == Tournament.Rounds.First().Id;
                }
            }

            return false;
        }

        // CREATE TESTS
        public bool IsLastRound()
        {
            bool belongsToATournament = Tournament != null;

            if (belongsToATournament)
            {
                bool tournamentHasRounds = Tournament.Rounds.Count > 0;

                if (tournamentHasRounds)
                {
                    return Id == Tournament.Rounds.Last().Id;
                }
            }

            return false;
        }

        public virtual GroupBase AddGroup()
        {
            // LOGG Error: Adding group using base, something when horribly wrong.
            throw new NotImplementedException();
        }

        public void OnGroupJustFinished()
        {
            if (GetPlayState() == PlayState.IsFinished)
            {
                AdvancingPlayerTransfer.TransferToNextRound(this);
            }
        }

        // CREATE TESTS?
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

        // CREATE TESTS
        public RoundBase GetNextRound()
        {
            for (int index = 0; index < Tournament.Rounds.Count - 1; ++index)
            {
                if (Tournament.Rounds[index].Id == Id)
                {
                    return Tournament.Rounds[index + 1];
                }
            }

            return null;
        }

        // CREATE TESTS
        public Match GetFirstMatch()
        {
            List<Match> matches = new List<Match>();

            foreach(GroupBase group in Groups)
            {
                matches.AddRange(group.Matches);
            }

            return matches.OrderBy(match => match.StartDateTime).FirstOrDefault();
        }

        // CREATE TESTS
        public Match GetLastMatch()
        {
            List<Match> matches = new List<Match>();

            foreach (GroupBase group in Groups)
            {
                matches.AddRange(group.Matches);
            }

            return matches.OrderBy(match => match.StartDateTime).LastOrDefault();
        }

        public List<PlayerReference> GetAdvancingPlayerReferences()
        {
            if (GetPlayState() != PlayState.IsFinished)
            {
                return null;
            }

            return PlayerStandingsCalculator.GetAdvancingPlayers(this);
        }

        public bool PlayerReferenceIsAdvancingPlayer(PlayerReference playerReference)
        {
            if (GetPlayState() != PlayState.IsFinished)
            {
                return false;
            }

            foreach (GroupBase group in Groups)
            {
                foreach (PlayerReference advancingPlayerReference in PlayerStandingsCalculator.GetAdvancingPlayers(group))
                {
                    if (playerReference.Id == advancingPlayerReference.Id)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public PlayState GetPlayState()
        {
            if (Groups.First().GetPlayState() == PlayState.NotBegun)
            {
                return PlayState.NotBegun;
            }

            if (Groups.Last().GetPlayState() == PlayState.IsFinished)
            {
                return PlayState.IsFinished;
            }

            return PlayState.IsPlaying;
        }
    }
}
