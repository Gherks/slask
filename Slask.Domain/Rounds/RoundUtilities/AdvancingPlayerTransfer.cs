using Slask.Domain.Utilities;
using System.Collections.Generic;

namespace Slask.Domain.Rounds.RoundUtilities
{
    public class AdvancingPlayerTransfer
    {
        public List<PlayerReference> PlayerReferences { get; private set; }

        public bool TransferToNextRound(RoundBase currentRound)
        {
            bool roundHasFinished = currentRound.GetPlayState() == PlayStateEnum.Finished;

            if (roundHasFinished)
            {
                RoundBase nextRound = currentRound.GetNextRound();
                bool hasRoundToTransferTo = nextRound != null;

                if (hasRoundToTransferTo)
                {
                    PlayerReferences = currentRound.GetAdvancingPlayerReferences();
                    bool hasPlayerReferencesToTransfer = PlayerReferences.Count > 0;

                    if (hasPlayerReferencesToTransfer)
                    {
                        nextRound.ReceiveTransferedPlayerReferences(this);
                        return true;
                    }

                    // LOG Error: Tried to transfer advancing player references to next round without any player references.
                    return false;
                }

                // LOG Error: Tried to transfer advancing player references to nonexistent next round.
                return false;
            }

            // LOG Error: Tried to transfer advancing player references to next round before current round is finished.
            return false;
        }
    }
}
