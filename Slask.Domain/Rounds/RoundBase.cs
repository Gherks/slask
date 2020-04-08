using Slask.Domain.Groups;
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
            throw new NotImplementedException();
        }

        public void OnGroupJustFinished()
        {
            if (GetPlayState() == PlayState.IsFinished)
            {
                AddAdvancingPlayersToNextRound();
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

        public List<PlayerReference> GetAdvancingPlayers()
        {
            if (GetPlayState() != PlayState.IsFinished)
            {
                return null;
            }

            List<PlayerReference> winningPlayers = new List<PlayerReference>();

            foreach (GroupBase group in Groups)
            {
                winningPlayers.AddRange(group.GetAdvancingPlayers());
            }

            return winningPlayers;
        }

        public bool PlayerReferenceIsAdvancingPlayer(PlayerReference playerReference)
        {
            if (GetPlayState() != PlayState.IsFinished)
            {
                return false;
            }

            foreach (GroupBase group in Groups)
            {
                foreach (PlayerReference advancingPlayerReference in group.GetAdvancingPlayers())
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

        private void AddAdvancingPlayersToNextRound()
        {
            RoundBase nextRound = GetNextRound();

            if (nextRound != null)
            {
                bool currentRoundHasFinished = GetPlayState() == PlayState.IsFinished;

                if (currentRoundHasFinished)
                {
                    AssignAdvancingPlayerReferencesToGroupsWithinNextRoundEvenly(nextRound, GetAdvancingPlayers());
                }
            }
        }

        private void AssignAdvancingPlayerReferencesToGroupsWithinNextRoundEvenly(RoundBase nextRound, List<PlayerReference> advancingPlayerReferences)
        {
            int playerReferencesPerGroupCount = advancingPlayerReferences.Count / nextRound.Groups.Count;
            int playerReferenceIndex = 0;

            foreach (GroupBase group in nextRound.Groups)
            {
                for (int perGroupIndex = 0; perGroupIndex < playerReferencesPerGroupCount; ++perGroupIndex)
                {
                    group.AddAdvancingPlayerReferenceFromPreviousRound(this, advancingPlayerReferences[playerReferenceIndex]);
                    playerReferenceIndex++;
                }
            }
        }
    }
}
