using Slask.Domain.Rounds.Bases;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;

namespace Slask.Domain.Groups.Interfaces
{
    public interface GroupInterface
    {
        Guid Id { get; }
        string Name { get; }
        List<Match> Matches { get; }
        public Guid RoundId { get; }
        public RoundBase Round { get; }

        public List<PlayerReference> PlayerReferences { get; }
        public List<PlayerStandingEntry> ChoosenTyingPlayerEntries { get; }

        bool AddPlayerReferences(List<PlayerReference> playerReferences);
        PlayState GetPlayState();
        bool ConstructGroupLayout(int playersPerGroupCount);
        bool FillMatchesWithPlayerReferences(List<PlayerReference> playerReferences);
        bool NewDateTimeIsValid(Match match, DateTime dateTime);
        bool HasProblematicTie();
        List<PlayerStandingEntry> FindProblematiclyTyingPlayers();
        bool SolveTieByChoosing(string playerName);
        bool HasSolvedTie();
        void OnMatchScoreDecreased(Match match);
        void OnMatchScoreIncreased(Match match);
    }
}
