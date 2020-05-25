using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public interface MatchInterface
    {
        Guid Id { get; }
        int BestOf { get; }
        DateTime StartDateTime { get; }
        Guid GroupId { get; }
        GroupBase Group { get; }

        Player Player1 { get; }
        Player Player2 { get; }

        bool SetBestOf(int bestOf);
        bool SetStartDateTime(DateTime dateTime);
        bool SetPlayers(PlayerReference player1Reference, PlayerReference player2Reference);
        bool AddPlayer(PlayerReference playerReference);
        Player FindPlayer(Guid id);
        Player FindPlayer(string name);
        bool IsReady();
        PlayState GetPlayState();
        Player GetWinningPlayer();
        Player GetLosingPlayer();
    }
}
