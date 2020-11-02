using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using System;
using System.Collections.Generic;

namespace Slask.Application.Interfaces.Persistence
{
    public interface TournamentRepositoryInterface
    {
        Tournament CreateTournament(string name);
        bool TournamentExist(Guid id);
        bool TournamentExist(string name);
        bool RemoveTournament(Guid id);
        bool RemoveTournament(string name);
        bool RenameTournament(Guid id, string name);
        IEnumerable<Tournament> GetTournaments(int startIndex = 0, int count = 128);
        Tournament GetTournament(Guid id);
        Tournament GetTournament(string name);
        PlayerReference AddPlayerReference(Tournament tournament, string name);
        bool RemovePlayerReferenceFromTournament(Tournament tournament, string name);
        bool RenamePlayerReferenceInTournament(PlayerReference playerReference, string name);
        IEnumerable<PlayerReference> GetPlayerReferencesByTournamentId(Guid id);
        IEnumerable<PlayerReference> GetPlayerReferencesByTournamentName(string name);
        Better AddBetterToTournament(Tournament tournament, User user);
        bool RemoveBetterFromTournamentById(Tournament tournament, Guid betterId);
        bool RemoveBetterFromTournamentByName(Tournament tournament, string betterName);
        IEnumerable<Better> GetBettersByTournamentId(Guid id);
        IEnumerable<Better> GetBettersByTournamentName(string name);
        BracketRound AddBracketRoundToTournament(Tournament tournament);
        DualTournamentRound AddDualTournamentRoundToTournament(Tournament tournament);
        RoundRobinRound AddRoundRobinRoundToTournament(Tournament tournament);
        bool RemoveRoundFromTournament(Tournament tournament, Guid roundId);
        bool RemoveRoundFromTournament(Tournament tournament, string roundName);
        bool RenameRoundInTournament(RoundBase round, string name);
        bool SetAdvancingPerGroupCountInRound(RoundBase round, int count);
        bool SetPlayersPerGroupCountInRound(RoundBase round, int count);
        bool SetStartTimeForMatch(Match match, DateTime dateTime);
        bool SetBestOfInMatch(Match match, int bestOf);
        bool BetterPlacesMatchBetOnMatch(Guid tournamentId, Guid matchId, string betterName, string playerName);
        bool SwitchPlayersInMatches(Player player1, Player player2);
        bool SolveTieByChoosingPlayerInGroup(GroupBase groupBase, Guid playerReferenceId);
        bool SolveTieByChoosingPlayerInGroup(GroupBase groupBase, PlayerReference playerReference);
        bool AddScoreToPlayerInMatch(Tournament tournament, Guid matchId, Guid playerId, int score);
        void Save();
    }
}