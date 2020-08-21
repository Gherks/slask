using Slask.Domain.Utilities;

namespace Slask.Domain.Bets.BetTypes
{
    public class MatchBet : BetBase
    {
        private MatchBet()
        {
            BetType = BetTypeEnum.MatchBet;
        }

        public static MatchBet Create(Better better, Match match, Player player)
        {
            bool anyParameterIsInvalid = !ParametersAreValid(better, match, player);

            if (anyParameterIsInvalid)
            {
                return null;
            }

            bool matchBetIsInvalid = !MatchBetIsValid(match, player);

            if (matchBetIsInvalid)
            {
                return null;
            }

            return new MatchBet
            {
                BetterId = better.Id,
                Better = better,
                MatchId = match.Id,
                PlayerId = player.Id,
            };
        }

        public override bool IsWon()
        {
            Match match = Better.Tournament.GetMatchByMatchId(MatchId);

            if (match != null)
            {
                bool matchNotFinished = match.GetPlayState() != PlayStateEnum.Finished;

                if (matchNotFinished)
                {
                    return false;
                }

                Player winningPlayer = match.GetWinningPlayer();

                bool betIsWon = PlayerId == winningPlayer.Id;
                return betIsWon;
            }

            return false;
        }

        private static bool ParametersAreValid(Better better, Match match, Player player)
        {
            bool invalidBetterGiven = better == null;

            if (invalidBetterGiven)
            {
                // LOG Error: Cannot create match bet because given better was invalid
                return false;
            }

            bool invalidMatchGiven = match == null;

            if (invalidMatchGiven)
            {
                // LOG Error: Cannot create match bet because given match was invalid
                return false;
            }

            bool invalidPlayerGiven = player == null;

            if (invalidPlayerGiven)
            {
                // LOG Error: Cannot create match bet because given player was invalid
                return false;
            }

            return true;
        }

        private static bool MatchBetIsValid(Match match, Player player)
        {
            bool givenPlayerIsNotParticipantInGivenMatch = match.FindPlayer(player.Id) == null;

            if (givenPlayerIsNotParticipantInGivenMatch)
            {
                // LOG Error: Cannot create match bet because given player was not part of given match
                return false;
            }

            bool matchIsNotReady = !match.IsReady();

            if (matchIsNotReady)
            {
                // LOG Issue?: Cannot create match bet because given match is not ready
                return false;
            }

            bool matchHasBegun = match.GetPlayState() != PlayStateEnum.NotBegun;

            if (matchHasBegun)
            {
                // LOG Issue?: Cannot create match bet because given match has already begun
                return false;
            }

            return true;
        }
    }
}
