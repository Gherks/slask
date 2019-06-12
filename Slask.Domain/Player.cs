using System;

namespace Slask.Domain
{
    public class Player
    {
        private Player()
        {
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid MatchPlayerId { get; private set; }
        public MatchPlayer MatchPlayer { get; private set; }

        public static Player Create(string name)
        {
            return new Player
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }

        public void Rename(string v)
        {
            throw new NotImplementedException();
        }
    }
}
