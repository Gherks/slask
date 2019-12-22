using System;

namespace Slask.Domain.Bets
{
    public class MatchBet : BetBase
    {
        private MatchBet()
        {
        }

        public Guid MatchId { get; private set; }
        public Match Match { get; private set; }
        public Guid PlayerId { get; private set; }
        public Player Player { get; private set; }

        public static MatchBet Create(Better better, Match match, Player player)
        {
            if (better == null || match == null || player == null)
            {
                // LOGG
                return null;
            }

            bool givenPlayerIsNotParticipantInGivenMatch = match.FindPlayer(player.Id) == null;
            bool matchIsNotReady = !match.IsReady();
            bool matchHasBegun = match.GetPlayState() != PlayState.NotBegun;

            if (givenPlayerIsNotParticipantInGivenMatch || matchIsNotReady || matchHasBegun)
            {
                // LOGG
                return null;
            }

            return new MatchBet
            {
                Id = Guid.NewGuid(),
                BetterId = better.Id,
                Better = better,
                MatchId = match.Id,
                Match = match,
                PlayerId = player.Id,
                Player = player
            };
        }

        public void UpdatePlayer(Player player)
        {
            if (player == null)
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
