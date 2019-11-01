using System;
using System.Collections.Generic;
using System.Text;

namespace Slask.Domain
{
    public partial class GroupBase
    {
        private class PlayerStandingEntry
        {
            public PlayerReference PlayerReference { get; private set; }
            public int Wins { get; private set; }

            public static PlayerStandingEntry Create(PlayerReference playerReference)
            {
                return new PlayerStandingEntry
                {
                    PlayerReference = playerReference,
                    Wins = 1
                };
            }

            public void AddWin()
            {
                Wins += 1;
            }
        }
    }
}
