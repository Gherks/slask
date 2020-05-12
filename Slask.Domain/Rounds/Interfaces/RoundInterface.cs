﻿using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds.Bases;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;

namespace Slask.Domain.Rounds.Interfaces
{
    public interface RoundInterface
    {
        Guid Id { get; }
        int AdvancingPerGroupCount { get; }
        int BestOf { get; }
        List<GroupBase> Groups { get; }
        string Name { get; }
        List<PlayerReference> PlayerReferences { get; }
        int PlayersPerGroupCount { get; }
        Guid TournamentId { get; }
        Tournament Tournament { get; }

        bool RenameTo(string name);
        PlayerReference RegisterPlayerReference(string name);
        bool ExcludePlayerReference(string name);
        bool Construct();
        bool FillGroupsWithPlayerReferences();
        bool SetBestOf(int bestOf);
        bool SetAdvancingPerGroupCount(int count);
        bool IsFirstRound();
        bool IsLastRound();
        Match GetFirstMatch();
        Match GetLastMatch();
        List<PlayerReference> GetAdvancingPlayerReferences();
        RoundBase GetNextRound();
        RoundBase GetPreviousRound();
        PlayState GetPlayState();
        bool PlayerReferenceIsAdvancingPlayer(PlayerReference playerReference);
    }
}