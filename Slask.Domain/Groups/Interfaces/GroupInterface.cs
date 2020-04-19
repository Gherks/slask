using Slask.Domain.Rounds.Bases;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;

namespace Slask.Domain.Groups.Interfaces
{
    public interface GroupInterface
    {
        Guid Id { get; }
        List<Match> Matches { get; }
        RoundBase Round { get; }
        Guid RoundId { get; }

        bool AddPlayerReferences(List<PlayerReference> playerReferences);
        PlayState GetPlayState();
        bool ConstructGroupLayout(int playersPerGroupCount);
        bool FillMatchesWithPlayerReferences(List<PlayerReference> playerReferences);
        bool NewDateTimeIsValid(Match match, DateTime dateTime);
        void OnMatchScoreDecreased(Match match);
        void OnMatchScoreIncreased(Match match);
    }
}