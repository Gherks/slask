using Slask.Domain.Bets;
using Slask.Domain.Bets.BetTypes;
using Slask.Domain.ObjectState;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundUtilities;
using Slask.Domain.Utilities;
using Slask.Domain.Utilities.StandingsSolvers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain.Groups
{
    public abstract class GroupBase : ObjectStateBase, GroupInterface, SortableInterface
    {
        protected GroupBase()
        {
            Id = Guid.NewGuid();
            Matches = new List<Match>();
            ChoosenTyingPlayerEntries = new List<StandingsEntry<PlayerReference>>();
        }

        public Guid Id { get; private set; }
        public ContestTypeEnum ContestType { get; protected set; }
        public int SortOrder { get; private set; }
        public string Name { get; protected set; }
        public List<Match> Matches { get; protected set; }
        public Guid RoundId { get; protected set; }
        public RoundBase Round { get; protected set; }

        public List<StandingsEntry<PlayerReference>> ChoosenTyingPlayerEntries { get; private set; }

        public bool AddPlayerReferences(List<PlayerReference> playerReferences)
        {
            bool parentRoundHasStarted = Round.GetPlayState() != PlayStateEnum.NotBegun;

            if (parentRoundHasStarted)
            {
                // LOG Error: Cannot add player references to a group in a round that has already started
                return false;
            }

            FillMatchesWithPlayerReferences(playerReferences);
            MarkAsModified();

            return true;
        }

        public List<Guid> GetPlayerReferenceIds()
        {
            HashSet<Guid> playerReferenceIds = new HashSet<Guid>();

            foreach (Match match in Matches)
            {
                if (match.Player1.PlayerReferenceId != Guid.Empty)
                {
                    playerReferenceIds.Add(match.Player1.PlayerReferenceId);
                }

                if (match.Player2.PlayerReferenceId != Guid.Empty)
                {
                    playerReferenceIds.Add(match.Player2.PlayerReferenceId);
                }
            }

            return playerReferenceIds.ToList();
        }

        public List<PlayerReference> GetPlayerReferences()
        {
            List<PlayerReference> playerReferences = new List<PlayerReference>();

            foreach (Guid playerReferenceId in GetPlayerReferenceIds())
            {
                foreach (PlayerReference playerReference in Round.Tournament.PlayerReferences)
                {
                    if (playerReferenceId == playerReference.Id)
                    {
                        playerReferences.Add(playerReference);
                        break;
                    }
                }
            }

            return playerReferences;
        }

        public PlayStateEnum GetPlayState()
        {
            bool noGroupHasBegun = AllMatchesPlayStatesAre(PlayStateEnum.NotBegun);

            if (noGroupHasBegun)
            {
                return PlayStateEnum.NotBegun;
            }

            bool allMatchesHasFinished = AllMatchesPlayStatesAre(PlayStateEnum.Finished);

            if (allMatchesHasFinished)
            {
                if (HasProblematicTie())
                {
                    if (HasSolvedTie())
                    {
                        return PlayStateEnum.Finished;
                    }
                    else
                    {
                        return PlayStateEnum.Ongoing;
                    }
                }
                else
                {
                    return PlayStateEnum.Finished;
                }
            }

            return PlayStateEnum.Ongoing;
        }

        private bool AllMatchesPlayStatesAre(PlayStateEnum playState)
        {
            foreach (Match match in Matches)
            {
                if (match.GetPlayState() != playState)
                {
                    return false;
                }
            }

            return true;
        }

        public abstract bool ConstructGroupLayout(int playersPerGroupCount);

        public abstract void FillMatchesWithPlayerReferences(List<PlayerReference> playerReferences);

        public abstract bool NewDateTimeIsValid(Match match, DateTime dateTime);

        public bool HasProblematicTie()
        {
            return FindProblematiclyTyingPlayers().Count > 0;
        }

        public List<StandingsEntry<PlayerReference>> FindProblematiclyTyingPlayers()
        {
            bool notAllMatchesHasBeenPlayed = !AllMatchesPlayStatesAre(PlayStateEnum.Finished);

            if (notAllMatchesHasBeenPlayed)
            {
                return new List<StandingsEntry<PlayerReference>>();
            }

            PlayerStandingsSolver playerStandingsSolver = new PlayerStandingsSolver();
            List<StandingsEntry<PlayerReference>> playerStandings = playerStandingsSolver.FetchFrom(this);
            List<StandingsEntry<PlayerReference>> problematicPlayers = new List<StandingsEntry<PlayerReference>>();

            StandingsEntry<PlayerReference> aboveThresholdPlayer = playerStandings[Round.AdvancingPerGroupCount - 1];
            StandingsEntry<PlayerReference> belowThresholdPlayer = playerStandings[Round.AdvancingPerGroupCount];

            bool playersPartOfProblematicTie = aboveThresholdPlayer.Points == belowThresholdPlayer.Points;

            if (playersPartOfProblematicTie)
            {
                foreach (StandingsEntry<PlayerReference> entry in playerStandings)
                {
                    bool isPartOfTie = entry.Points == aboveThresholdPlayer.Points;

                    if (isPartOfTie)
                    {
                        problematicPlayers.Add(entry);
                    }
                }
            }

            return problematicPlayers;
        }

        public bool SolveTieByChoosing(Guid playerReferenceId)
        {
            PlayerReference playerReference = Round.Tournament.GetPlayerReferenceById(playerReferenceId);

            return SolveTieByChoosing(playerReference);
        }

        public bool SolveTieByChoosing(PlayerReference playerReference)
        {
            List<StandingsEntry<PlayerReference>> tyingPlayers = FindProblematiclyTyingPlayers();
            bool hasTyingPlayers = tyingPlayers.Count > 0;

            if (hasTyingPlayers)
            {
                bool playerChosen = ChooseTyingPlayer(playerReference);
                bool tieSolved = HasSolvedTie();

                if (playerChosen && tieSolved)
                {
                    AdvancingPlayerTransfer advancingPlayerTransfer = new AdvancingPlayerTransfer();
                    advancingPlayerTransfer.TransferToNextRound(Round);
                    return true;
                }
            }

            return false;
        }

        public bool HasSolvedTie()
        {
            bool hasPlayersChosenForSolvingTie = ChoosenTyingPlayerEntries.Count > 0;

            if (hasPlayersChosenForSolvingTie)
            {
                List<StandingsEntry<PlayerReference>> playerStandings = FindProblematiclyTyingPlayers();
                int tyingScore = playerStandings.First().Points;

                PlayerStandingsSolver playerStandingsSolver = new PlayerStandingsSolver();
                playerStandings = playerStandingsSolver.FetchFrom(this);

                int clearWinners = 0;

                foreach (StandingsEntry<PlayerReference> entry in playerStandings)
                {
                    if (entry.Points > tyingScore)
                    {
                        clearWinners += 1;
                    }
                }

                bool hasEnoughToTransferToNextRound = (clearWinners + ChoosenTyingPlayerEntries.Count) == Round.AdvancingPerGroupCount;

                return hasEnoughToTransferToNextRound;
            }

            return false;
        }

        public virtual void OnMatchScoreIncreased(Match match)
        {
        }

        public virtual void OnMatchScoreDecreased(Match match)
        {
        }

        protected void ChangeMatchCountTo(int count)
        {
            Matches.Clear();

            while (Matches.Count < count)
            {
                Matches.Add(Match.Create(this));
            }

            MarkAsModified();
        }

        protected void AssignDefaultName()
        {
            Name = "Group " + Labeler.GetLabelForIndex(Round.Groups.Count);
            MarkAsModified();
        }

        internal void RemoveAllMatchBetsOnMatch(Match match)
        {
            foreach (Better better in Round.Tournament.Betters)
            {
                for (int betIndex = 0; betIndex < better.Bets.Count; ++betIndex)
                {
                    if (better.Bets[betIndex] is MatchBet matchBet)
                    {
                        if (matchBet.MatchId == match.Id)
                        {
                            BetBase bet = better.Bets[betIndex--];

                            better.Bets.Remove(bet);
                            bet.MarkForDeletion();
                        }
                    }
                }
            }
        }

        private bool ChooseTyingPlayer(PlayerReference playerReference)
        {
            List<StandingsEntry<PlayerReference>> tyingPlayers = FindProblematiclyTyingPlayers();

            foreach (StandingsEntry<PlayerReference> entry in tyingPlayers)
            {
                if (entry.Object.Id == playerReference.Id)
                {
                    bool haveNotChosePlayerAlready = !ChoosenTyingPlayerEntries.Contains(entry);

                    if (haveNotChosePlayerAlready)
                    {
                        ChoosenTyingPlayerEntries.Add(entry);
                        return true;
                    }
                }
            }

            return false;
        }

        public void UpdateSortOrder()
        {
            if (ObjectState == ObjectStateEnum.Deleted)
            {
                return;
            }

            for (int index = 0; index < Round.Groups.Count; ++index)
            {
                if (Round.Groups[index].Id == Id)
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
            Matches = Matches.OrderBy(match => match.SortOrder).ToList();
        }
    }
}
