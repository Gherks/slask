using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds.Bases;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;

namespace Slask.Domain.Rounds
{
    public class AdvancingPlayerTransfer
    {
        public List<PlayerReference> PlayerReferences { get; private set; }

        public bool TransferToNextRound(RoundBase round)
        {
            bool roundHasFinished = round.GetPlayState() == PlayState.Finished;

            if (roundHasFinished)
            {
                RoundBase nextRound = round.GetNextRound();
                bool hasRoundToTransferTo = nextRound != null;

                if (hasRoundToTransferTo)
                {
                    PlayerReferences = round.GetAdvancingPlayerReferences();
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
