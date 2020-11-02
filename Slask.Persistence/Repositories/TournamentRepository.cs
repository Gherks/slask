using Microsoft.EntityFrameworkCore;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using Slask.Domain.Bets.BetTypes;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Persistence.Repositories
{
    public class TournamentRepository : TournamentRepositoryInterface, IDisposable
    {
        private readonly SlaskContext _slaskContext;

        public TournamentRepository(SlaskContext slaskContext)
        {
            _slaskContext = slaskContext;
        }

        public void Dispose()
        {
        }

        public Tournament CreateTournament(string name)
        {
            bool givenParametersAreInvalid = !TournamentCreationParametersAreValid(name);

            if (givenParametersAreInvalid)
            {
                return null;
            }

            Tournament tournament = Tournament.Create(name);
            _slaskContext.Add(tournament);

            return tournament;
        }
        public bool HasTournament(Guid id)
        {
            return _slaskContext.Tournaments.Any(tournament => tournament.Id == id);
        }

        public bool HasTournament(string name)
        {
            return _slaskContext.Tournaments.Any(tournament => tournament.Name.ToLower() == name.ToLower());
        }

        public bool RemoveTournament(Guid id)
        {
            Tournament tournament = GetTournament(id);

            if (tournament != null)
            {
                tournament.MarkForDeletion();
                return true;
            }

            return false;
        }

        public bool RemoveTournament(string name)
        {
            Tournament tournament = GetTournament(name);

            if (tournament != null)
            {
                tournament.MarkForDeletion();
                return true;
            }

            return false;
        }

        public bool RenameTournament(Guid id, string name)
        {
            name = name.Trim();

            bool nameIsNotEmpty = name != "";
            bool nameIsNotInUse = HasTournament(name) == false;

            if (nameIsNotEmpty && nameIsNotInUse)
            {
                Tournament tournament = GetTournament(id);
                bool tournamentFound = tournament != null;

                if (tournamentFound)
                {
                    tournament.RenameTo(name);
                    return true;
                }
            }

            return false;
        }

        public bool RenameTournament(string oldName, string newName)
        {
            oldName = oldName.Trim();
            newName = newName.Trim();

            bool newNameIsNotEmpty = newName != "";
            bool nameIsNotInUse = HasTournament(newName) == false;

            if (newNameIsNotEmpty && nameIsNotInUse)
            {
                Tournament tournament = GetTournament(oldName);
                bool tournamentFound = tournament != null;

                if (tournamentFound)
                {
                    tournament.RenameTo(newName);
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<Tournament> GetTournaments(int startIndex = 0, int count = 128)
        {
            int controlledStartIndex = Math.Max(0, startIndex);
            int controlledCount = Math.Max(0, count);

            return _slaskContext.Tournaments.Skip(controlledStartIndex).Take(controlledCount).AsNoTracking();
        }

        public Tournament GetTournament(Guid id)
        {
            IQueryable<Tournament> tournamentQuery = _slaskContext.Tournaments.Where(tournament => tournament.Id == id);

            Tournament tournament = GetTournamentWithComponentsFromQuery(tournamentQuery).FirstOrDefault();

            if (tournament != null)
            {
                _slaskContext.Add(tournament);
                tournament.ResetObjectStatesOnAllEntities();
                tournament.SortEntities();
                tournament.FindIssues();
            }

            return tournament;
        }

        public Tournament GetTournament(string name)
        {
            IQueryable<Tournament> tournamentQuery = _slaskContext.Tournaments.Where(tournament => tournament.Name.ToLower() == name.ToLower());

            Tournament tournament = GetTournamentWithComponentsFromQuery(tournamentQuery).FirstOrDefault();

            if (tournament != null)
            {
                _slaskContext.Add(tournament);
                tournament.ResetObjectStatesOnAllEntities();
                tournament.SortEntities();
                tournament.FindIssues();
            }

            return tournament;
        }

        public PlayerReference AddPlayerReference(Tournament tournament, string name)
        {
            if (tournament != null)
            {
                return tournament.RegisterPlayerReference(name);
            }

            return null;
        }

        public bool RemovePlayerReferenceFromTournament(Tournament tournament, string name)
        {
            if (tournament != null)
            {
                return tournament.ExcludePlayerReference(name);
            }

            return false;
        }

        public bool RenamePlayerReferenceInTournament(PlayerReference playerReference, string name)
        {
            if (playerReference != null)
            {
                return playerReference.RenameTo(name);
            }

            return false;
        }

        public IEnumerable<PlayerReference> GetPlayerReferencesByTournamentId(Guid id)
        {
            Tournament tournament = GetTournament(id);

            bool tournamentIsInvalid = tournament == null;

            if (tournamentIsInvalid)
            {
                // LOG Error: Cannot fetch players references from tournament by id, tournament does not exist.
                return null;
            }

            return tournament.PlayerReferences;
        }

        public IEnumerable<PlayerReference> GetPlayerReferencesByTournamentName(string name)
        {
            Tournament tournament = GetTournament(name);

            bool tournamentIsInvalid = tournament == null;

            if (tournamentIsInvalid)
            {
                // LOG Error: Cannot fetch players references from tournament by name, tournament does not exist.
                return null;
            }

            return tournament.PlayerReferences;
        }

        public Better AddBetterToTournament(Tournament tournament, User user)
        {
            Better better = tournament.AddBetter(user);

            if (better != null)
            {
                _slaskContext.Attach(better.User);
            }

            return better;
        }

        public bool RemoveBetterFromTournamentById(Tournament tournament, Guid betterId)
        {
            Better better = tournament.GetBetterById(betterId);

            if (better != null)
            {
                return tournament.RemoveBetter(better);
            }

            return false;
        }

        public bool RemoveBetterFromTournamentByName(Tournament tournament, string betterName)
        {
            Better better = tournament.GetBetterByName(betterName);

            if (better != null)
            {
                return tournament.RemoveBetter(better);
            }

            return false;
        }

        public IEnumerable<Better> GetBettersByTournamentId(Guid id)
        {
            Tournament tournament = GetTournament(id);

            bool tournamentIsInvalid = tournament == null;

            if (tournamentIsInvalid)
            {
                // LOG Error: Cannot fetch betters from tournament by id, tournament does not exist.
                return null;
            }

            return tournament.Betters;
        }

        public IEnumerable<Better> GetBettersByTournamentName(string name)
        {
            Tournament tournament = GetTournament(name);

            bool tournamentIsInvalid = tournament == null;

            if (tournamentIsInvalid)
            {
                // LOG Error: Cannot fetch betters from tournament by name, tournament does not exist.
                return null;
            }

            return tournament.Betters;
        }

        public BracketRound AddBracketRoundToTournament(Tournament tournament)
        {
            return tournament.AddBracketRound();
        }

        public DualTournamentRound AddDualTournamentRoundToTournament(Tournament tournament)
        {
            return tournament.AddDualTournamentRound();
        }

        public RoundRobinRound AddRoundRobinRoundToTournament(Tournament tournament)
        {
            return tournament.AddRoundRobinRound();
        }

        public bool RemoveRoundFromTournament(Tournament tournament, Guid roundId)
        {
            RoundBase roundToRemove = tournament.GetRoundById(roundId);

            if (roundToRemove != null)
            {
                return tournament.RemoveRound(roundToRemove);
            }

            return false;
        }

        public bool RemoveRoundFromTournament(Tournament tournament, string roundName)
        {
            RoundBase roundToRemove = tournament.GetRoundByName(roundName);

            if (roundToRemove != null)
            {
                return tournament.RemoveRound(roundToRemove);
            }

            return false;
        }

        public bool RenameRoundInTournament(RoundBase round, string name)
        {
            if (round != null)
            {
                return round.RenameTo(name);
            }

            return false;
        }

        public bool SetAdvancingPerGroupCountInRound(RoundBase round, int count)
        {
            if (round != null)
            {
                return round.SetAdvancingPerGroupCount(count);
            }

            return false;
        }

        public bool SetPlayersPerGroupCountInRound(RoundBase round, int count)
        {
            if (round != null)
            {
                return round.SetPlayersPerGroupCount(count);
            }

            return false;
        }

        public bool SetStartTimeForMatch(Match match, DateTime dateTime)
        {
            if (match != null)
            {
                return match.SetStartDateTime(dateTime);
            }

            return false;
        }

        public bool SetBestOfInMatch(Match match, int bestOf)
        {
            if (match != null)
            {
                return match.SetBestOf(bestOf);
            }

            return false;
        }

        public bool BetterPlacesMatchBetOnMatch(Guid tournamentId, Guid matchId, string betterName, string playerName)
        {
            Tournament tournament = GetTournament(tournamentId);

            if (tournament == null)
            {
                /* LOG Issue: Could not place match bet; tournament with id '{tournamentId}' does not exist. */
                return false;
            }

            Better better = tournament.GetBetterByName(betterName);

            if (better == null)
            {
                /* LOG Issue: Could not place match bet; better with name '{betterName}' does not exist in tournament. */
                return false;
            }

            foreach (RoundBase round in tournament.Rounds)
            {
                foreach (GroupBase group in round.Groups)
                {
                    foreach (Match match in group.Matches)
                    {
                        if (match.Id == matchId)
                        {
                            Player player = match.FindPlayer(playerName);

                            if (player == null)
                            {
                                /* LOG Issue: Could not place match bet; sought player is not participating in sought match. */
                                return false;
                            }

                            MatchBet matchBet = better.PlaceMatchBet(match, player);

                            return matchBet != null;
                        }
                    }
                }
            }

            return false;
        }

        public bool SwitchPlayersInMatches(Player player1, Player player2)
        {
            return PlayerSwitcher.SwitchMatchesOn(player1, player2);
        }

        public bool SolveTieByChoosingPlayerInGroup(GroupBase groupBase, Guid playerReferenceId)
        {
            return groupBase.SolveTieByChoosing(playerReferenceId);
        }

        public bool SolveTieByChoosingPlayerInGroup(GroupBase groupBase, PlayerReference playerReference)
        {
            return groupBase.SolveTieByChoosing(playerReference);
        }

        public bool AddScoreToPlayerInMatch(Tournament tournament, Guid matchId, Guid playerId, int score)
        {
            if (tournament == null)
            {
                /* LOG Issue: Could not add score to player in match; tournament with id '{tournamentId}' does not exist. */
                return false;
            }

            foreach (RoundBase round in tournament.Rounds)
            {
                foreach (GroupBase group in round.Groups)
                {
                    foreach (Match match in group.Matches)
                    {
                        if (match.Id == matchId)
                        {
                            Player player = match.FindPlayer(playerId);

                            if (player == null)
                            {
                                /* LOG Issue: Could not add score to player in match; sought player is not participating in sought match. */
                                return false;
                            }

                            return player.IncreaseScore(score);
                        }
                    }
                }
            }

            return false;
        }

        public void Save()
        {
            _slaskContext.SaveChanges();
        }

        private bool TournamentCreationParametersAreValid(string name)
        {
            bool nameIsEmpty = name == "";

            if (nameIsEmpty)
            {
                // LOG Error: Cannot create tournament with empty name.
                return false;
            }

            bool tournamentAlreadyExist = GetTournament(name) != null;

            if (tournamentAlreadyExist)
            {
                // LOG Error: Cannot create tournament with given name, it's already in use.
                return false;
            }

            return true;
        }

        private IQueryable<Tournament> GetTournamentWithComponentsFromQuery(IQueryable<Tournament> tournamentQuery)
        {
            return tournamentQuery
                .Include(tournament => tournament.PlayerReferences)
                .Include(tournament => tournament.Betters)
                    .ThenInclude(better => better.Bets)
                .Include(tournament => tournament.Betters)
                    .ThenInclude(better => better.User)
                .Include(tournament => tournament.Rounds)
                .Include(tournament => tournament.Rounds)
                    .ThenInclude(round => round.Groups)
                        .ThenInclude(group => group.Matches)
                            .ThenInclude(match => match.Player1)
                .Include(tournament => tournament.Rounds)
                    .ThenInclude(round => round.Groups)
                        .ThenInclude(group => group.Matches)
                            .ThenInclude(match => match.Player2);
        }
    }
}
