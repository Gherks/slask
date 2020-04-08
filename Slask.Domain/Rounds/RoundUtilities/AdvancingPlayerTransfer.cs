using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;

namespace Slask.Domain.Rounds.RoundUtilities
{
    public static class AdvancingPlayerTransfer
    {
        public static bool TransferToNextRound(RoundBase round)
        {
            bool roundHasFinished = round.GetPlayState() == PlayState.IsFinished;

            if (roundHasFinished)
            {
                RoundBase nextRound = round.GetNextRound();

                if (nextRound != null)
                {
                    return DistributeAdvancingPlayerReferencesToNextRoundEvenly(round.GetAdvancingPlayerReferences(), nextRound);
                }

                // LOGG Error: Tried to transfer advancing player references when there is no next round.
                return false;
            }

            // LOGG Error: Tried to transfer advancing player references to next round before current round is finished.
            return false;
        }

        private static bool DistributeAdvancingPlayerReferencesToNextRoundEvenly(List<PlayerReference> advancingPlayerReferences, RoundBase targetRound)
        {
            int playerReferencesPerGroupCount = advancingPlayerReferences.Count / targetRound.Groups.Count;
            int playerReferenceIndex = 0;

            foreach (GroupBase group in targetRound.Groups)
            {
                for (int perGroupIndex = 0; perGroupIndex < playerReferencesPerGroupCount; ++perGroupIndex)
                {
                    bool addWasSuccessful = group.AddPlayerReference(advancingPlayerReferences[playerReferenceIndex]);

                    if (!addWasSuccessful)
                    {
                        // LOGG Error: Failed to add advancing player to next round.
                        return false;
                    }

                    playerReferenceIndex++;
                }
            }

            return true;
        }
    }
}
