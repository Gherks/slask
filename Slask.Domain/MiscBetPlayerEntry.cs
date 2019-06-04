using System;

namespace Slask.Domain
{
    public class MiscBetPlayerEntry
    {
        public Guid Id { get; set; }
        public Player Player { get; set; }
        public int Value { get; set; }
    }
}
