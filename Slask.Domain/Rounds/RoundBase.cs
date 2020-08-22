using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.ObjectState;
using Slask.Domain.Procedures.AdvancingPerGroupCount;
using Slask.Domain.Procedures.PlayersPerGroupCount;
using Slask.Domain.Rounds.RoundUtilities;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain.Rounds
{
    public class RoundBase : ObjectStateBase, RoundInterface, SortableInterface
    {
        protected RoundBase()
        {
            Id = Guid.NewGuid();
            PlayersPerGroupCount = 2;
            AdvancingPerGroupCount = 1;
            Groups = new List<GroupBase>();
        }

        private PlayersPerGroupCountProcedure _playersPerGroupCountProcedure;
        private AdvancingPerGroupCountProcedure _advancingPerGroupCountProcedure;

        public Guid Id { get; private set; }
        public ContestTypeEnum ContestType { get; protected set; }
        public int SortOrder { get; private set; }
        public string Name { get; protected set; }
        public int PlayersPerGroupCount { get; protected set; }
        public int AdvancingPerGroupCount { get; protected set; }
        public List<GroupBase> Groups { get; protected set; }
        public Guid TournamentId { get; protected set; }
        public Tournament Tournament { get; protected set; }

        public int GetExpectedParticipantCount()
        {
            int participants;

            if (IsFirstRound())
            {
                participants = Tournament.PlayerReferences.Count;
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
            name = name.Trim();
            bool newNameIsEmpty = name.Length == 0;

            if (newNameIsEmpty)
            {
                return false;
            }

            foreach (RoundBase round in Tournament.Rounds)
            {
                bool nameIsInUse = round.Name.ToUpper() == name.ToUpper();

                if (nameIsInUse)
                {
                    return false;
                }
            }

            Name = name;
            return true;
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

            MarkAsModified();

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
            if (GetPlayState() != PlayStateEnum.Finished)
            {
                return null;
            }

            return AdvancingPlayersSolver.FetchFrom(this);
        }

        public bool PlayerReferenceIsAdvancingPlayer(PlayerReference playerReference)
        {
            if (GetPlayState() != PlayStateEnum.Finished)
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

        public PlayStateEnum GetPlayState()
        {
            bool noGroupHasBegun = AllGroupsPlayStatesAre(PlayStateEnum.NotBegun);

            if (noGroupHasBegun)
            {
                return PlayStateEnum.NotBegun;
            }

            bool allGroupsHasFinished = AllGroupsPlayStatesAre(PlayStateEnum.Finished);

            if (allGroupsHasFinished)
            {
                return PlayStateEnum.Finished;
            }

            return PlayStateEnum.Ongoing;
        }

        private bool AllGroupsPlayStatesAre(PlayStateEnum playState)
        {
            foreach (GroupBase group in Groups)
            {
                if (group.GetPlayState() != playState)
                {
                    return false;
                }
            }

            return true;
        }

        public void ReceiveTransferedPlayerReferences(AdvancingPlayerTransfer advancingPlayerTransfer)
        {
            PlayerReferences = advancingPlayerTransfer.PlayerReferences;
            MarkAsModified();

            int perGroupCount = PlayerReferences.Count / Groups.Count;
            int playerReferenceIndex = 0;

            foreach (GroupBase group in Groups)
            {
                List<PlayerReference> playerReferences = new List<PlayerReference>();

                for (int perGroupIndex = 0; perGroupIndex < perGroupCount; ++perGroupIndex)
                {
                    PlayerReference playerReference = PlayerReferences[playerReferenceIndex++];

                    playerReferences.Add(playerReference);
                    playerReference.MarkAsModified();
                }

                group.AddPlayerReferences(playerReferences);
            }
        }

        public bool SetPlayersPerGroupCount(int count)
        {
            if (_playersPerGroupCountProcedure.NewValueValid(count, out int newValue, this))
            {
                PlayersPerGroupCount = newValue;
                _playersPerGroupCountProcedure.ApplyPostAssignmentOperations(this);
                MarkAsModified();

                return true;
            }

            return false;
        }

        public bool SetAdvancingPerGroupCount(int count)
        {
            if (_advancingPerGroupCountProcedure.NewValueValid(count, out int newValue, this))
            {
                AdvancingPerGroupCount = newValue;
                _advancingPerGroupCountProcedure.ApplyPostAssignmentOperations(this);
                MarkAsModified();

                return true;
            }

            return false;
        }

        public void UpdateSortOrder()
        {
            if (ObjectState == ObjectStateEnum.Deleted)
            {
                return;
            }

            for (int index = 0; index < Tournament.Rounds.Count; ++index)
            {
                if (Tournament.Rounds[index].Id == Id)
                {
                    if (SortOrder != index)
                    {
                        MarkAsModified();
                    }

                    SortOrder = index;
                    return;
                }
            }
        }

        public void SortEntities()
        {
            Groups = Groups.OrderBy(group => group.SortOrder).ToList();

            foreach (GroupBase group in Groups)
            {
                group.SortEntities();
            }
        }

        public bool HasProblematicTie()
        {
            foreach (GroupBase group in Groups)
            {
                bool groupHasProblematicTie = group.HasProblematicTie();
                bool groupHasNotSolvedTie = !group.HasSolvedTie();

                if (groupHasProblematicTie && groupHasNotSolvedTie)
                {
                    return true;
                }
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
                        defaultName = "Round " + Labeler.GetLabelForIndex(++index);
                        renameFailed = !RenameTo(defaultName);
                    }
                    while (renameFailed);
                }
            }

            defaultName = "Round " + Labeler.GetLabelForIndex(Tournament.Rounds.Count);
            return RenameTo(defaultName);
        }

        protected void AssignProcedures(PlayersPerGroupCountProcedure playersPerGroupCountProcedure, AdvancingPerGroupCountProcedure advancingPerGroupCountProcedure)
        {
            _playersPerGroupCountProcedure = playersPerGroupCountProcedure;
            _advancingPerGroupCountProcedure = advancingPerGroupCountProcedure;
        }

        protected virtual GroupBase AddGroup()
        {
            // LOG Error: Adding group using base, something went horribly wrong.
            throw new NotImplementedException();
        }

        private bool ConstructGroups()
        {
            int expectedParticipantCount = GetExpectedParticipantCount();
            int groupCount = (int)Math.Ceiling(expectedParticipantCount / (double)PlayersPerGroupCount);

            foreach (GroupBase group in Groups)
            {
                group.MarkForDeletion();

                foreach (Match match in group.Matches)
                {
                    match.MarkForDeletion();
                }
            }

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
    }
}
