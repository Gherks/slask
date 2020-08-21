using Slask.Domain.Utilities;

namespace Slask.Domain.Bets.BetTypes
{
    public class MiscellaneousBet : BetBase
    {
        private MiscellaneousBet()
        {
            BetType = BetTypeEnum.MiscellaneousBet;
        }

        public static MiscellaneousBet Create(Better better, Player player)
        {
            bool anyParameterIsInvalid = !ParametersAreValid(better, player);

            if (anyParameterIsInvalid)
            {
                return null;
            }

            return new MiscellaneousBet
            {
                BetterId = better.Id,
                PlayerId = player.Id,
            };
        }

        public override bool IsWon()
        {
            throw new System.NotImplementedException();
        }

        private static bool ParametersAreValid(Better better, Player player)
        {
            bool invalidBetterGiven = better == null;

            if (invalidBetterGiven)
            {
                // LOG Error: Cannot create miscellaneous bet because given better was invalid
                return false;
            }

            bool invalidPlayerGiven = player == null;

            if (invalidPlayerGiven)
            {
                // LOG Error: Cannot create miscellaneous bet because given player was invalid
                return false;
            }

            return true;
        }
    }
}
