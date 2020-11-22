using Slask.Domain.Utilities;
using System;

namespace Slask.Domain.Bets.BetTypes
{
    public class MatchBet : BetBase
    {
        private MatchBet()
        {
            BetType = BetTypeEnum.MatchBet;
        }

        public static MatchBet Create(Better better, Match match, Guid playerReferenceId)
        {
            bool anyParameterIsInvalid = !ParametersAreValid(better, match, playerReferenceId);

            if (anyParameterIsInvalid)
            {
                return null;
            }

            bool matchBetIsInvalid = !MatchBetIsValid(match, playerReferenceId);

            if (matchBetIsInvalid)
            {
                return null;
            }

            return new MatchBet
            {
                BetterId = better.Id,
                Better = better,
                MatchId = match.Id,
                PlayerReferenceId = playerReferenceId
            };
        }

        public override bool IsWon()
        {
            Match match = Better.Tournament.GetMatch(MatchId);

            if (match != null)
            {
                bool matchNotFinished = match.GetPlayState() != PlayStateEnum.Finished;

                if (matchNotFinished)
                {
                    return false;
                }

                Guid winningPlayerReferenceId = match.GetWinningPlayerReference();

                bool betIsWon = PlayerReferenceId == winningPlayerReferenceId;
                return betIsWon;
            }

            return false;
        }

        private static bool ParametersAreValid(Better better, Match match, Guid playerReferenceId)
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

            bool invalidPlayerReferenceIdGiven = playerReferenceId == Guid.Empty;

            if (invalidPlayerReferenceIdGiven)
            {
                // LOG Error: Cannot create match bet because given player was invalid
                return false;
            }

            return true;
        }

        private static bool MatchBetIsValid(Match match, Guid playerReferenceId)
        {
            bool givenPlayerIsNotParticipantInGivenMatch = match.HasPlayer(playerReferenceId) == false;

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
