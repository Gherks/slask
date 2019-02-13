using System;

namespace Slask.Domain
{
    public class Match
    {
        public int Id { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public int Result { get; set; }
        public DateTime StartDateTime { get; set; }
    }
}
