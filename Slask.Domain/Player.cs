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
        public int Score { get; private set; }
        private PlayerNameReference PlayerNameReference { get; set; }
        public Guid MatchId { get; private set; }
        public Match Match { get; private set; }

        [NotMapped]
        public string Name
        {
            get { return PlayerNameReference != null ? PlayerNameReference.Name : null; }
            private set { }
        }

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

        public void RenameTo(string v)
        {
            throw new NotImplementedException();
        }

        public void SubtractScore(int value)
        {
            Score -= value;
        }

        public static Player Create(string name, Match match)
        {
            if (name == "" || match == null)
            {
                return null;
            }

            return new Player
            {
                Id = Guid.NewGuid(),
                PlayerNameReference = PlayerNameReference.Create(name, match.Group.Round.Tournament),
                Score = 0,
                MatchId = match.Id,
                Match = match,
            };
        }
    }
}
