using Slask.Domain.Groups;
using Slask.Domain.Rounds.RoundUtilities;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;

namespace Slask.Domain.Rounds
{
    public interface RoundInterface
    {
        Guid Id { get; }
        string Name { get; }
        int PlayersPerGroupCount { get; }
        int BestOf { get; }
        int AdvancingPerGroupCount { get; }
        List<GroupBase> Groups { get; }
        List<PlayerReference> PlayerReferences { get; }
        Guid TournamentId { get; }
        Tournament Tournament { get; }

        int GetExpectedParticipantCount();
        bool RenameTo(string name);
        PlayerReference RegisterPlayerReference(string name);
        bool ExcludePlayerReference(string name);
        bool Construct();
        bool FillGroupsWithPlayerReferences();
        bool SetPlayersPerGroupCount(int count);
        bool SetBestOf(int bestOf);
        bool SetAdvancingPerGroupCount(int count);
        bool HasProblematicTie();
        bool IsFirstRound();
        bool IsLastRound();
        Match GetFirstMatch();
        Match GetLastMatch();
        List<PlayerReference> GetAdvancingPlayerReferences();
        RoundBase GetNextRound();
        RoundBase GetPreviousRound();
        PlayState GetPlayState();
        bool PlayerReferenceIsAdvancingPlayer(PlayerReference playerReference);
        void ReceiveTransferedPlayerReferences(AdvancingPlayerTransfer advancingPlayerTransfer);
    }
}
