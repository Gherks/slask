﻿using System;

namespace Slask.Domain.Bets
{
    public class BetBase : BetInterface
    {
        protected BetBase()
        {
        }

        public Guid Id { get; protected set; }
        public Guid BetterId { get; protected set; }
        public Better Better { get; protected set; }
    }
}
