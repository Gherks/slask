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
            PlayerReferenceId = Guid.Empty;
            Score = 0;
        }

        public Guid Id { get; private set; }
        public Guid PlayerReferenceId { get; private set; }
        public int Score { get; private set; }
        public Guid MatchId { get; private set; }
        public Match Match { get; private set; }

        public static Player Create(Match match)
        {
            if (match == null)
            {
                return null;
            }

            return new Player
            {
                MatchId = match.Id,
                Match = match
            };
        }
        public string GetName()
        {
            PlayerReference playerReference = GetPlayerReference();

            if (playerReference != null)
            {
                return playerReference.Name;
            }

            return "";
        }

        public bool AssignPlayerReference(Guid playerReferenceId)
        {
            bool matchWithThisPlayerHasNotBegun = Match.GetPlayState() == PlayStateEnum.NotBegun;

            if (matchWithThisPlayerHasNotBegun)
            {
                PlayerReferenceId = playerReferenceId;
                MarkAsModified();

                return true;
            }

            return false;
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

        private PlayerReference GetPlayerReference()
        {
            if (PlayerReferenceId != Guid.Empty)
            {
                foreach (PlayerReference playerReference in Match.Group.Round.PlayerReferences)
                {
                    if (playerReference.Id == PlayerReferenceId)
                    {
                        return playerReference;
                    }
                }
            }

            return null;
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
