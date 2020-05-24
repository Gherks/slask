using Slask.Domain.Bets;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundUtilities;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Slask.Domain.Groups
{
    public partial class GroupBase : GroupInterface
    {
        protected GroupBase()
        {
            Matches = new List<Match>();
            PlayerReferences = new List<PlayerReference>();
            ChoosenTyingPlayerEntries = new List<PlayerStandingEntry>();
        }

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public List<Match> Matches { get; protected set; }
        public Guid RoundId { get; protected set; }
        public RoundBase Round { get; protected set; }

        [NotMapped]
        public List<PlayerReference> PlayerReferences { get; private set; }

        [NotMapped]
        public List<PlayerStandingEntry> ChoosenTyingPlayerEntries { get; private set; }

        public bool AddPlayerReferences(List<PlayerReference> playerReferences)
        {
            bool parentRoundHasStarted = Round.GetPlayState() != PlayState.NotBegun;

            if (parentRoundHasStarted)
            {
                // LOG Error: Cannot add player references to a group in a round that has already started
                return false;
            }

            bool successfullyFilledGroup = FillMatchesWithPlayerReferences(playerReferences);

            if (successfullyFilledGroup)
            {
                PlayerReferences = playerReferences;
                return true;
            }

            return false;
        }

        public PlayState GetPlayState()
        {
            bool noGroupHasBegun = AllMatchesPlayStatesAre(PlayState.NotBegun);

            if (noGroupHasBegun)
            {
                return PlayState.NotBegun;
            }

            bool allMatchesHasFinished = AllMatchesPlayStatesAre(PlayState.Finished);

            if (allMatchesHasFinished)
            {
                if (HasProblematicTie())
                {
                    if (HasSolvedTie())
                    {
                        return PlayState.Finished;
                    }
                    else
                    {
                        return PlayState.Ongoing;
                    }
                }
                else
                {
                    return PlayState.Finished;
                }
            }

            return PlayState.Ongoing;
        }

        private bool AllMatchesPlayStatesAre(PlayState playState)
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

        public List<PlayerStandingEntry> FindProblematiclyTyingPlayers()
        {
            bool notAllMatchesHasBeenPlayed = !AllMatchesPlayStatesAre(PlayState.Finished);

            if (notAllMatchesHasBeenPlayed)
            {
                return new List<PlayerStandingEntry>();
            }

            List<PlayerStandingEntry> problematicPlayers = new List<PlayerStandingEntry>();
            List<PlayerStandingEntry> playerStandings = PlayerStandingsSolver.FetchFrom(this);

            PlayerStandingEntry aboveThresholdPlayer = playerStandings[Round.AdvancingPerGroupCount - 1];
            PlayerStandingEntry belowThresholdPlayer = playerStandings[Round.AdvancingPerGroupCount];

            bool playersPartOfProblematicTie = aboveThresholdPlayer.Wins == belowThresholdPlayer.Wins;

            if (playersPartOfProblematicTie)
            {
                foreach (PlayerStandingEntry entry in playerStandings)
                {
                    bool isPartOfTie = entry.Wins == aboveThresholdPlayer.Wins;

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
            List<PlayerStandingEntry> tyingPlayers = FindProblematiclyTyingPlayers();
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
                List<PlayerStandingEntry> playerStandings = FindProblematiclyTyingPlayers();
                int tyingScore = playerStandings.First().Wins;

                playerStandings = PlayerStandingsSolver.FetchFrom(this);

                int clearWinners = 0;

                foreach (PlayerStandingEntry entry in playerStandings)
                {
                    if (entry.Wins > tyingScore)
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
        }

        protected void AssignDefaultName()
        {
            Name = "Group " + Labeler.GetLabelForIndex(Round.Groups.Count);
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
                            better.Bets.Remove(better.Bets[betIndex--]);
                        }
                    }
                }
            }
        }

        private bool ChooseTyingPlayer(string playerName)
        {
            List<PlayerStandingEntry> tyingPlayers = FindProblematiclyTyingPlayers();

            foreach (PlayerStandingEntry entry in tyingPlayers)
            {
                if (entry.PlayerReference.Name == playerName)
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
