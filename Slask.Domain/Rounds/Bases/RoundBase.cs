﻿using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds.Interfaces;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain.Rounds.Bases
{
    public class RoundBase : RoundInterface
    {
        protected RoundBase()
        {
            PlayersPerGroupCount = 2;
            AdvancingPerGroupCount = 1;
            Groups = new List<GroupBase>();
            PlayerReferences = new List<PlayerReference>();
        }

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public int PlayersPerGroupCount { get; protected set; }
        public int BestOf { get; protected set; }
        public int AdvancingPerGroupCount { get; protected set; }
        public List<GroupBase> Groups { get; protected set; }
        public List<PlayerReference> PlayerReferences { get; protected set; }
        public Guid TournamentId { get; protected set; }
        public Tournament Tournament { get; protected set; }

        public int GetExpectedParticipantCount()
        {
            int participants;

            if (IsFirstRound())
            {
                participants = PlayerReferences.Count;
            }
            else
            {
                RoundBase round = GetPreviousRound();
                participants = round.Groups.Count * round.AdvancingPerGroupCount;
            }

            return Math.Max(2, participants);
        }

        public bool RenameTo(string name)
        {
            foreach (RoundBase roundBase in Tournament.Rounds)
            {
                bool newNameIsEmpty = name.Length == 0;
                bool nameIsInUse = roundBase.Name.ToUpper() == name.ToUpper();

                if (newNameIsEmpty || nameIsInUse)
                {
                    return false;
                }
            }

            Name = name;
            return true;
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
                OnPlayerReferencesChanged();

                return PlayerReferences.Last();
            }

            return null;
        }

        public bool ExcludePlayerReference(string name)
        {
            bool roundIsFirstRound = IsFirstRound();
            bool tournamentHasNotBegun = GetPlayState() == PlayState.NotBegun;
            bool nameIsNotEmpty = name.Length != 0;

            if (roundIsFirstRound && tournamentHasNotBegun && nameIsNotEmpty)
            {
                PlayerReference playerReference = PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == name);
                bool playerReferenceExistInRound = playerReference != null;

                if (playerReferenceExistInRound)
                {
                    PlayerReferences.Remove(PlayerReference.Create(name, Tournament));
                    OnPlayerReferencesChanged();

                    return true;
                }
            }

            return false;
        }

        public bool Construct()
        {
            ConstructGroups();
            AssignDefaultStartTimeToMatchesInRound();

            RoundBase nextRound = GetNextRound();
            bool nextRoundExist = nextRound != null;

            if (nextRoundExist)
            {
                return nextRound.Construct();
            }

            return true;
        }

        public bool FillGroupsWithPlayerReferences()
        {
            if (PlayerReferences.Count == 0)
            {
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

        public bool SetBestOf(int bestOf)
        {
            bool newBestOfIsUneven = bestOf % 2 != 0;
            bool bestOfIsGreaterThanZero = bestOf > 0;

            if (newBestOfIsUneven && bestOfIsGreaterThanZero)
            {
                BestOf = bestOf;
                return true;
            }

            return false;
        }

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

        public Match GetFirstMatch()
        {
            List<Match> matches = new List<Match>();

            foreach (GroupBase group in Groups)
            {
                matches.AddRange(group.Matches);
            }

            return matches.OrderBy(match => match.StartDateTime).FirstOrDefault();
        }

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

            return AdvancingPlayersSolver.FetchFrom(this);
        }

        public bool PlayerReferenceIsAdvancingPlayer(PlayerReference playerReference)
        {
            if (GetPlayState() != PlayState.IsFinished)
            {
                return false;
            }

            foreach (GroupBase group in Groups)
            {
                foreach (PlayerReference advancingPlayerReference in AdvancingPlayersSolver.FetchFrom(group))
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
            bool hasNotBegun = Groups.First().GetPlayState() == PlayState.NotBegun;

            if (hasNotBegun)
            {
                return PlayState.NotBegun;
            }

            bool lastGroupIsFinished = Groups.Last().GetPlayState() == PlayState.IsFinished;

            return lastGroupIsFinished ? PlayState.IsFinished : PlayState.IsPlaying;
        }

        public virtual bool SetAdvancingPerGroupCount(int count)
        {
            bool tournamentHasNotBegun = GetPlayState() == PlayState.NotBegun;

            if (tournamentHasNotBegun)
            {
                AdvancingPerGroupCount = count;

                Construct();
                Tournament.FindIssues();
                return true;
            }

            return false;
        }

        protected bool AssignDefaultName()
        {
            string defaultName;

            for (int index = 0; index < Tournament.Rounds.Count; ++index)
            {
                bool roundAlreadyAddedToTournament = Tournament.Rounds[index].Id == Id;

                if (roundAlreadyAddedToTournament)
                {
                    --index;

                    bool renameFailed = true;
                    do
                    {
                        defaultName = "Round " + GetNextDefaultRoundLettering(++index);
                        renameFailed = !RenameTo(defaultName);
                    }
                    while (renameFailed);
                }
            }

            defaultName = "Round " + GetNextDefaultRoundLettering(Tournament.Rounds.Count);
            return RenameTo(defaultName);
        }

        protected virtual GroupBase AddGroup()
        {
            // LOG Error: Adding group using base, something went horribly wrong.
            throw new NotImplementedException();
        }

        private string GetNextDefaultRoundLettering(int letterIndex)
        {
            const string lookup = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string lettering = "";

            int zCount = letterIndex / lookup.Length;
            for (int index = 0; index < zCount; ++index)
            {
                lettering += "Z";
            }

            int endingLetter = letterIndex % lookup.Length;
            lettering += lookup[endingLetter];

            return lettering.ToUpper();
        }

        private bool ConstructGroups()
        {
            int expectedParticipantCount = GetExpectedParticipantCount();
            int groupCount = (int)Math.Ceiling(expectedParticipantCount / (double)PlayersPerGroupCount);

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

        private void AssignDefaultStartTimeToMatchesInRound()
        {
            DateTime startTime;

            if (IsFirstRound())
            {
                startTime = SystemTime.Now.AddDays(7);
            }
            else
            {
                startTime = GetPreviousRound().GetLastMatch().StartDateTime.AddHours(1);
            }

            foreach (GroupBase group in Groups)
            {
                foreach (Match match in group.Matches)
                {
                    match.SetStartDateTime(startTime);
                    startTime = startTime.AddHours(1);
                }
            }
        }

        private void OnPlayerReferencesChanged()
        {
            Construct();
            FillGroupsWithPlayerReferences();
            Tournament.FindIssues();
        }
    }
}
