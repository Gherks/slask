using Slask.Domain.Utilities;
using System;

namespace Slask.Domain.Bets.BetTypes
{
    public class MiscellaneousBet : BetBase
    {
        private MiscellaneousBet()
        {
            BetType = BetTypeEnum.MiscellaneousBet;
        }

        public static MiscellaneousBet Create(Better better, Guid playerReferenceId)
        {
            bool anyParameterIsInvalid = !ParametersAreValid(better, playerReferenceId);

            if (anyParameterIsInvalid)
            {
                return null;
            }

            return new MiscellaneousBet
            {
                BetterId = better.Id,
                PlayerReferenceId = playerReferenceId
            };
        }

        public override bool IsWon()
        {
            throw new System.NotImplementedException();
        }

        private static bool ParametersAreValid(Better better, Guid playerReferenceId)
        {
            bool invalidBetterGiven = better == null;

            if (invalidBetterGiven)
            {
                // LOG Error: Cannot create miscellaneous bet because given better was invalid
                return false;
            }

            bool invalidPlayerReferenceIdGiven = playerReferenceId == Guid.Empty;

            if (invalidPlayerReferenceIdGiven)
            {
                // LOG Error: Cannot create miscellaneous bet because given player was invalid
                return false;
            }

            return true;
        }
    }
}
