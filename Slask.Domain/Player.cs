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

        public static Player Create(Match match)
        {
            if (match == null)
            {
                return null;
            }

            return new Player
            {
                Id = Guid.NewGuid(),
                PlayerReference = null,
                Score = 0,
                MatchId = match.Id,
                Match = match
            };
        }

        public void SetPlayerReference(PlayerReference playerReference)
        {
            if (PlayerReference != null)
            {
                bool playerReferenceIsRemoved = playerReference == null;
                bool playerReferenceChanged = playerReference != null && PlayerReference.Id != playerReference.Id;

                if(playerReferenceIsRemoved || playerReferenceChanged)
                {
                    Match.Group.RemovePlayerReference(PlayerReference);
                }
            }
            
            PlayerReference = playerReference;
        }

        //public void IncrementScore()
        //{
        //    bool matchHasNotBegun = Match.GetPlayState() == PlayState.NotBegun;

        //    if (matchHasNotBegun)
        //    {
        //        return;
        //    }

        //    bool isAlreadyFinished = Match.GetPlayState() == PlayState.IsFinished;

        //    Score++;

        //    if (!isAlreadyFinished)
        //    {
        //        Match.Group.MatchScoreIncreased(Match);
        //    }
        //}

        //public void DecrementScore()
        //{
        //    Score = Math.Max(Score - 1, 0);

        //    bool isPlaying = Match.GetPlayState() == PlayState.IsPlaying;
        //    if (!isPlaying)
        //    {
        //        Match.Group.MatchScoreDecreased(Match);
        //    }
        //}

        public void IncreaseScore(int value)
        {
            bool matchHasNotBegun = Match.GetPlayState() == PlayState.NotBegun;

            if(matchHasNotBegun)
            {
                return;
            }

            bool matchIsAlreadyFinished = Match.GetPlayState() == PlayState.IsFinished;

            Score += value;

            if (!matchIsAlreadyFinished)
            {
                Match.Group.MatchScoreIncreased(Match);
            }
        }

        public void DecreaseScore(int value)
        {
            bool matchHasNotBegun = Match.GetPlayState() == PlayState.NotBegun;

            if (matchHasNotBegun)
            {
                return;
            }

            Score = Math.Max(Score - value, 0);

            bool isPlaying = Match.GetPlayState() == PlayState.IsPlaying;
            if (!isPlaying)
            {
                Match.Group.MatchScoreDecreased(Match);
            }
        }
    }
}
