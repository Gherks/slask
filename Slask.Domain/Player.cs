using Slask.Domain.Rounds;
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

        public void IncreaseScore(int value)
        {
            bool matchIsReady = Match.IsReady();
            bool matchIsOngoing = Match.GetPlayState() == PlayState.Ongoing;

            if (matchIsReady && matchIsOngoing)
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
            }
        }

        public void DecreaseScore(int value)
        {
            bool matchIsReady = Match.IsReady();
            bool matchIsOngoing = Match.GetPlayState() == PlayState.Ongoing;

            if (matchIsReady && matchIsOngoing)
            {
                Score -= value;
                Score = Math.Max(Score, 0);
            }
        }
    }
}
