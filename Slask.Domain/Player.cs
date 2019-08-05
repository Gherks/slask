using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slask.Domain
{
    public class Player
    {
        private Player()
        {
        }

        public Guid Id { get; private set; }
        public PlayerReference PlayerReference
        {
            get { return PlayerReference; }
            set
            {
                if (value != null)
                {
                    PlayerReference = value;
                }
            }
        }
        public int Score { get; private set; }
        public Guid MatchId { get; private set; }
        public Match Match { get; private set; }

        [NotMapped]
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

        public void IncrementScore()
        {
            Score++;
            Match.Group.MatchScoreChanged();
        }

        public void DecrementScore()
        {
            Score--;
            Match.Group.MatchScoreChanged();
        }

        public void IncreaseScore(int value)
        {
            Score += value;
            Match.Group.MatchScoreChanged();
        }

        public void DecreaseScore(int value)
        {
            Score -= value;
            Match.Group.MatchScoreChanged();
        }
    }
}
