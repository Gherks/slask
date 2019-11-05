using System;
using System.Collections.Generic;
using System.Text;

namespace Slask.Domain.Bets
{
    public class BetBase
    {
        protected BetBase()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
    }
}
