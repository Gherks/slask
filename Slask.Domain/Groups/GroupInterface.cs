using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using Slask.Domain.Utilities.StandingsSolvers;
using System;
using System.Collections.Generic;

namespace Slask.Domain.Groups
{
    public interface GroupInterface
    {
        Guid Id { get; }
        string Name { get; }
        List<Match> Matches { get; }
        public Guid RoundId { get; }
        public RoundBase Round { get; }

        public List<PlayerReference> PlayerReferences { get; }
        public List<StandingsEntry<PlayerReference>> ChoosenTyingPlayerEntries { get; }

        bool AddPlayerReferences(List<PlayerReference> playerReferences);
        PlayStateEnum GetPlayState();
        bool ConstructGroupLayout(int playersPerGroupCount);
        bool FillMatchesWithPlayerReferences(List<PlayerReference> playerReferences);
        bool NewDateTimeIsValid(Match match, DateTime dateTime);
        bool HasProblematicTie();
        List<StandingsEntry<PlayerReference>> FindProblematiclyTyingPlayers();
        bool SolveTieByChoosing(string playerName);
        bool HasSolvedTie();
        void OnMatchScoreDecreased(Match match);
        void OnMatchScoreIncreased(Match match);
    }
}
