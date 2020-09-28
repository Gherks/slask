using System;

namespace Slask.Dto
{
    public sealed class PlayerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
    }
}