using System;

namespace Slask.Domain.Bets
{
    public class MatchBet : BetBase
    {
        private MatchBet()
        {
        }

        public Match Match { get; private set; }
        public Guid PlayerId { get; private set; }

        public static MatchBet Create(Match match, Player player)
        {
            if (match == null || player == null)
            {
                // LOGG
                return null;
            }

            if(match.FindPlayer(player.Id) == null)
            {
                // LOGG
                return null;
            }

            return new MatchBet
            {
                Match = match,
                PlayerId = player.Id
            };
        }

        public void UpdatePlayer(Player player)
        {
            if(player == null)
            {
                // LOGG
                return;
            }

            if (Match.FindPlayer(player.Id) == null)
            {
                // LOGG
                return;
            }

            PlayerId = player.Id;
        }
    }
}
