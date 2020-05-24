using Slask.Domain.Rounds.RoundUtilities;
using Slask.Domain.Utilities;
using System;

namespace Slask.Domain
{
    public class Player
    {
        private Player()
        {
        }

        public Guid Id { get; private set; }
        public PlayerReference PlayerReference { get; private set; }
        public int Score { get; private set; }
        public Guid MatchId { get; private set; }
        public Match Match { get; private set; }

        // Ignored by SlaskContext
        public string Name
        {
            get { return PlayerReference != null ? PlayerReference.Name : ""; }
            private set { }
        }

        public static Player Create(Match match, PlayerReference playerReference)
        {
            if (match == null || playerReference == null)
            {
                return null;
            }

            return new Player
            {
                Id = Guid.NewGuid(),
                PlayerReference = playerReference,
                Score = 0,
                MatchId = match.Id,
                Match = match
            };
        }

        public bool IncreaseScore(int value)
        {
            if (CanChangeScore())
            {
                Score += value;
                Match.Group.OnMatchScoreIncreased(Match);

                bool groupJustFinished = Match.Group.GetPlayState() == PlayState.Finished;

                if (groupJustFinished)
                {
                    if (Match.Group.Round.GetPlayState() == PlayState.Finished)
                    {
                        AdvancingPlayerTransfer advancingPlayerTransfer = new AdvancingPlayerTransfer();
                        advancingPlayerTransfer.TransferToNextRound(Match.Group.Round);
                    }
                }

                return true;
            }

            return false;
        }

        public bool DecreaseScore(int value)
        {
            if (CanChangeScore())
            {
                Score -= value;
                Score = Math.Max(Score, 0);
                return true;
            }

            return false;
        }

        private bool CanChangeScore()
        {
            bool matchIsReady = Match.IsReady();
            bool matchIsOngoing = Match.GetPlayState() == PlayState.Ongoing;
            bool tournamentHasNoIssues = !Match.Group.Round.Tournament.HasIssues();

            return matchIsReady && matchIsOngoing && tournamentHasNoIssues;
        }
    }
}
