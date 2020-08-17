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
    public partial class GroupBase : ObjectStateBase, GroupInterface
    {
        protected GroupBase()
        {
            Matches = new List<Match>();
            ChoosenTyingPlayerEntries = new List<StandingsEntry<PlayerReference>>();
            ObjectState = ObjectStateEnum.Added;
        }

        public Guid Id { get; protected set; }
        public ContestTypeEnum ContestType { get; protected set; }
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

            bool successfullyFilledGroup = FillMatchesWithPlayerReferences(playerReferences);

            if (successfullyFilledGroup)
            {
                MarkAsModified();
                return true;
            }

            return false;
        }

        public List<PlayerReference> GetPlayerReferences()
        {
            Dictionary<Guid, PlayerReference> playerReferenceDictionary = new Dictionary<Guid, PlayerReference>();

            foreach (Match match in Matches)
            {
                if (match.Player1 != null)
                {
                    PlayerReference playerReference = match.Player1.PlayerReference;
                    playerReferenceDictionary[playerReference.Id] = playerReference;
                }

                if (match.Player2 != null)
                {
                    PlayerReference playerReference = match.Player2.PlayerReference;
                    playerReferenceDictionary[playerReference.Id] = playerReference;
                }
            }

            return playerReferenceDictionary.Values.ToList();
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

        public virtual bool ConstructGroupLayout(int playersPerGroupCount)
        {
            throw new NotImplementedException();
        }

        public virtual bool FillMatchesWithPlayerReferences(List<PlayerReference> playerReferences)
        {
            throw new NotImplementedException();
        }

        public virtual bool NewDateTimeIsValid(Match match, DateTime dateTime)
        {
            return true;
        }

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

        public bool SolveTieByChoosing(string playerName)
        {
            List<StandingsEntry<PlayerReference>> tyingPlayers = FindProblematiclyTyingPlayers();
            bool hasTyingPlayers = tyingPlayers.Count > 0;

            if (hasTyingPlayers)
            {
                bool playerChosen = ChooseTyingPlayer(playerName);
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
                        if (matchBet.Match.Id == match.Id)
                        {
                            BetBase bet = better.Bets[betIndex--];

                            better.Bets.Remove(bet);
                            bet.MarkForDeletion();
                        }
                    }
                }
            }
        }

        private bool ChooseTyingPlayer(string playerName)
        {
            List<StandingsEntry<PlayerReference>> tyingPlayers = FindProblematiclyTyingPlayers();

            foreach (StandingsEntry<PlayerReference> entry in tyingPlayers)
            {
                if (entry.Object.Name == playerName)
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
    }
}
