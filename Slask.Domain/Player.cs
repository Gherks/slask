using Slask.Domain.ObjectState;
using Slask.Domain.Rounds.RoundUtilities;
using Slask.Domain.Utilities;
using System;

namespace Slask.Domain
{
    public class Player : ObjectStateBase
    {
        private Player()
        {
            Id = Guid.NewGuid();
            Score = 0;
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
                PlayerReference = playerReference,
                MatchId = match.Id,
                Match = match,
                ObjectState = ObjectStateEnum.Added
            };
        }

        public bool IncreaseScore(int value)
        {
            if (CanChangeScore())
            {
                Score += value;
                Match.Group.OnMatchScoreIncreased(Match);

                bool groupJustFinished = Match.Group.GetPlayState() == PlayStateEnum.Finished;

                if (groupJustFinished)
                {
                    if (Match.Group.Round.GetPlayState() == PlayStateEnum.Finished)
                    {
                        AdvancingPlayerTransfer advancingPlayerTransfer = new AdvancingPlayerTransfer();
                        advancingPlayerTransfer.TransferToNextRound(Match.Group.Round);
                    }
                }

                MarkAsModified();

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

                MarkAsModified();

                return true;
            }

            return false;
        }

        private bool CanChangeScore()
        {
            bool matchIsReady = Match.IsReady();
            bool matchIsOngoing = Match.GetPlayState() == PlayStateEnum.Ongoing;
            bool tournamentHasNoIssues = !Match.Group.Round.Tournament.HasIssues();

            return matchIsReady && matchIsOngoing && tournamentHasNoIssues;
        }
    }
}
