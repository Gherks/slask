using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundUtilities;
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
            PlayersPerGroupCount = 2;
            Groups = new List<GroupBase>();
            PlayerReferences = new List<PlayerReference>();
        }

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        // CREATE TESTS do not allow group sizes to be less than advancing amount
        // CREATE TESTS make sure PlayerPerGroupCount cant be set to anything less than 2
        public int PlayersPerGroupCount { get; private set; }
        public int BestOf { get; protected set; }
        public int AdvancingPerGroupCount { get; protected set; }
        public List<GroupBase> Groups { get; protected set; }
        public List<PlayerReference> PlayerReferences { get; protected set; }
        public Guid TournamentId { get; protected set; }
        public Tournament Tournament { get; protected set; }

        [NotMapped]
        public int AdvancingPlayersCount
        {
            get { return AdvancingPerGroupCount * Groups.Count; }
            private set { }
        }

        public int SetPlayersPerGroupCount(int count)
        {
            return PlayersPerGroupCount = Math.Max(2, count);
        }

        public PlayerReference RegisterPlayerReference(string name)
        {
            bool roundIsFirstRound = IsFirstRound();
            bool tournamentHasNotBegun = GetPlayState() == PlayState.NotBegun;
            bool nameIsNotRegistered = !PlayerReferences.Any(playerReference => playerReference.Name == name);
            bool nameIsNotEmpty = name.Length != 0;

            if (roundIsFirstRound && tournamentHasNotBegun && nameIsNotRegistered && nameIsNotEmpty)
            {
                PlayerReferences.Add(PlayerReference.Create(name, Tournament));

                if (PlayerReferences.Count > 1)
                {
                    ConstructRound();
                    FillGroupsWithPlayerReferences();
                }

                return PlayerReferences.Last();
            }

            return null;
        }

        public bool ConstructRound()
        {
            int participantCount;

            if (IsFirstRound())
            {
                participantCount = PlayerReferences.Count;
            }
            else
            {
                participantCount = GetPreviousRound().AdvancingPlayersCount;
            }

            ConstructGroups(participantCount);
            RoundIssueFinder.FindIssues(this, participantCount);

            RoundBase nextRound = GetNextRound();
            bool nextRoundExist = nextRound != null;

            if (nextRoundExist)
            {
                return nextRound.ConstructRound();
            }

            return true;
        }

        public bool FillGroupsWithPlayerReferences()
        {
            if (PlayerReferences.Count <= 1)
            {
                // LOGG Error: Cant fill groups with players references unless round contains two or more player references.
                return false;
            }

            for (int groupIndex = 0; groupIndex < Groups.Count; ++groupIndex)
            {
                int startPlayerReferenceIndex = groupIndex * PlayersPerGroupCount;

                List<PlayerReference> playerReferences = GetNextGroupOfPlayers(startPlayerReferenceIndex, PlayersPerGroupCount);

                Groups[groupIndex].AddPlayerReferences(playerReferences);
            }

            return true;
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

        public void OnGroupJustFinished()
        {
            if (GetPlayState() == PlayState.IsFinished)
            {
                AdvancingPlayerTransfer.TransferToNextRound(this);
            }
        }

        protected virtual GroupBase AddGroup()
        {
            // LOGG Error: Adding group using base, something when horribly wrong.
            throw new NotImplementedException();
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

            foreach (GroupBase group in Groups)
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
            bool hasNoGroups = Groups.Count == 0;
            bool hasNotBegun = hasNoGroups || Groups.First().GetPlayState() == PlayState.NotBegun;

            if (hasNotBegun)
            {
                return PlayState.NotBegun;
            }

            bool lastGroupIsFinished = Groups.Last().GetPlayState() == PlayState.IsFinished;

            return lastGroupIsFinished ? PlayState.IsFinished : PlayState.IsPlaying;
        }

        private bool ConstructGroups(int participantCount)
        {
            int groupCount = (int)Math.Ceiling(participantCount / (double)PlayersPerGroupCount);

            Groups.Clear();

            for (int groupIndex = 0; groupIndex < groupCount; ++groupIndex)
            {
                Groups.Add(AddGroup());
                Groups.Last().ConstructGroupLayout(PlayersPerGroupCount);
            }

            return true;
        }

        private List<PlayerReference> GetNextGroupOfPlayers(int startPlayerReferenceIndex, int playerReferenceCount)
        {
            List<PlayerReference> playerReferences = new List<PlayerReference>();

            for (int playerReferenceIndex = startPlayerReferenceIndex; playerReferenceIndex < PlayerReferences.Count; ++playerReferenceIndex)
            {
                playerReferences.Add(PlayerReferences[playerReferenceIndex]);

                if (playerReferences.Count >= playerReferenceCount)
                {
                    return playerReferences;
                }
            }

            return playerReferences;
        }
    }
}
