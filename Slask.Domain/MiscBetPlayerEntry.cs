using System;

namespace Slask.Domain
{
    public class MiscBetPlayerEntry
    {
        private MiscBetPlayerEntry()
        {
        }

        public Guid Id { get; private set; }
        public Player Player { get; private set; }
        public int Value { get; private set; }

        public static MiscBetPlayerEntry Create(Player player, int value)
        {
            return new MiscBetPlayerEntry
            {
                Id = Guid.NewGuid(),
                Player = player,
                Value = value
            };
        }
    }
}
