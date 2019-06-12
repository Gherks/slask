using System;

namespace Slask.Domain
{
    public class MatchPlayer
    {
        private MatchPlayer()
        {
        }

        public Guid Id { get; private set; }
        public Player Player { get; private set; }
        public int Score { get; private set; }
        public Guid MatchId { get; private set; }
        public Match Match { get; private set; }

        public void IncrementScore()
        {
            Score++;
        }

        public void DecrementScore()
        {
            Score--;
        }

        public void AddScore(int value)
        {
            Score += value;
        }

        public void SubtractScore(int value)
        {
            Score -= value;
        }

        public static MatchPlayer Create(Player player)
        {
            return new MatchPlayer
            {
                Id = Guid.NewGuid(),
                Player = player,
                Score = 0
            };
        }
    }
}
