using Slask.Domain.Bets;
using Slask.Domain.Groups.Interfaces;
using Slask.Domain.Rounds.Bases;
using Slask.Domain.Utilities;
using Slask.Utilities.PlayerStandingsSolver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Slask.Domain.Groups.Bases
{
    public partial class GroupBase : GroupInterface
    {
        protected GroupBase()
        {
            Matches = new List<Match>();
            PlayerReferences = new List<PlayerReference>();
            ChoosenTyingPlayers = new List<PlayerReference>();
        }

        public Guid Id { get; protected set; }
        public List<Match> Matches { get; protected set; }
        public Guid RoundId { get; protected set; }
        public RoundBase Round { get; protected set; }

        [NotMapped]
        public List<PlayerReference> PlayerReferences { get; private set; }

        [NotMapped]
        public List<PlayerReference> ChoosenTyingPlayers { get; private set; }

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
            bool hasNotBegun = Matches.First().GetPlayState() == PlayState.NotBegun;

            if (hasNotBegun)
            {
                return PlayState.NotBegun;
            }

            bool lastMatchIsFinished = Matches.Last().GetWinningPlayer() != null;
            bool hasNoProblematicTie = !HasProblematicTie();

            return (lastMatchIsFinished && hasNoProblematicTie) ? PlayState.Finished : PlayState.Ongoing;
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
            bool groupHasProblematicTie = FindProblematiclyTyingPlayers().Count > 0;

            return groupHasProblematicTie;
        }

        public bool SolveTieByChoosing(string playerName)
        {
            List<PlayerReference> tyingPlayers = FindProblematiclyTyingPlayers();
            bool hasTyingPlayers = tyingPlayers.Count > 0;

            if (hasTyingPlayers)
            {
                foreach (PlayerReference playerReference in tyingPlayers)
                {
                    if(playerReference.Name == playerName)
                    {
                        bool haveNotChosePlayerAlready = !ChoosenTyingPlayers.Contains(playerReference);

                        if (haveNotChosePlayerAlready)
                        {
                            ChoosenTyingPlayers.Add(playerReference);
                            return true;
                        }
                    }
                }
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

        private List<PlayerReference> FindProblematiclyTyingPlayers()
        {
            List<PlayerReference> problematicPlayers = new List<PlayerReference>();

            List<PlayerStandingEntry> playerStandings = PlayerStandingsSolver.FetchFrom(this);

            for (int index = Round.AdvancingPerGroupCount; index < playerStandings.Count; ++index)
            {
                PlayerStandingEntry previousPlayer = playerStandings[Round.AdvancingPerGroupCount - 1];
                PlayerStandingEntry currentPlayer = playerStandings[Round.AdvancingPerGroupCount];

                bool playersPartOfProblematicTie = previousPlayer.Wins == currentPlayer.Wins;

                if (playersPartOfProblematicTie)
                {
                    problematicPlayers.Add(currentPlayer.PlayerReference);
                }
                else
                {
                    break;
                }
            }

            return problematicPlayers;
        }
    }
}
