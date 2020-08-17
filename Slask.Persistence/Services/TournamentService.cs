using Microsoft.EntityFrameworkCore;
using Slask.Domain;
using Slask.Domain.Bets.BetTypes;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Persistence.Services
{
    public class TournamentService : TournamentServiceInterface
    {
        private readonly SlaskContext _slaskContext;

        public TournamentService(SlaskContext slaskContext)
        {
            _slaskContext = slaskContext;
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

        public bool RenameTournament(Guid id, string name)
        {
            name = name.Trim();

            bool nameIsNotEmpty = name != "";
            bool nameIsNotInUse = GetTournamentByName(name) == null;

            if (nameIsNotEmpty && nameIsNotInUse)
            {
                Tournament tournament = GetTournamentById(id);
                bool tournamentFound = tournament != null;

                if (tournamentFound)
                {
                    tournament.RenameTo(name);
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

        public Tournament GetTournamentById(Guid id)
        {
            Tournament tournament = _slaskContext.Tournaments
                .Where(tournament => tournament.Id == id)
                .Include(tournament => tournament.Betters)
                .Include(tournament => tournament.Rounds)
                .FirstOrDefault();

            if (tournament != null)
            {
                tournament.FindIssues();
            }

            return tournament;
        }

        public Tournament GetTournamentByName(string name)
        {
            Tournament tournament = _slaskContext.Tournaments
                .Where(tournament => tournament.Name.ToLower() == name.ToLower())
                .Include(tournament => tournament.Betters)
                .Include(tournament => tournament.Rounds)
                .FirstOrDefault();

            if (tournament != null)
            {
                tournament.FindIssues();
            }

            return tournament;
        }

        public List<PlayerReference> GetPlayerReferencesByTournamentId(Guid id)
        {
            Tournament tournament = GetTournamentById(id);

            bool tournamentIsInvalid = tournament == null;

            if (tournamentIsInvalid)
            {
                // LOG Error: Cannot fetch players references from tournament by id, tournament does not exist.
                return null;
            }

            return tournament.GetPlayerReferences();
        }

        public List<PlayerReference> GetPlayerReferencesByTournamentName(string name)
        {
            Tournament tournament = GetTournamentByName(name);

            bool tournamentIsInvalid = tournament == null;

            if (tournamentIsInvalid)
            {
                // LOG Error: Cannot fetch players references from tournament by name, tournament does not exist.
                return null;
            }

            return tournament.GetPlayerReferences();
        }

        public List<Better> GetBettersByTournamentId(Guid id)
        {
            Tournament tournament = GetTournamentById(id);

            bool tournamentIsInvalid = tournament == null;

            if (tournamentIsInvalid)
            {
                // LOG Error: Cannot fetch betters from tournament by id, tournament does not exist.
                return null;
            }

            return tournament.Betters;
        }

        public List<Better> GetBettersByTournamentName(string name)
        {
            Tournament tournament = GetTournamentByName(name);

            bool tournamentIsInvalid = tournament == null;

            if (tournamentIsInvalid)
            {
                // LOG Error: Cannot fetch betters from tournament by name, tournament does not exist.
                return null;
            }

            return tournament.Betters;
        }

        public Better AddBetterToTournament(Tournament tournament, User user)
        {
            Better better = tournament.AddBetter(user);

            if (better != null)
            {
                _slaskContext.Add(better.User);
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

        public PlayerReference AddPlayerReference(Tournament tournament, string name)
        {
            PlayerReference playerReference = null;
            RoundBase firstRound = tournament.GetFirstRound();

            if (firstRound != null)
            {
                playerReference = firstRound.RegisterPlayerReference(name);
            }

            return playerReference;
        }

        public bool RenameRoundInTournament(RoundBase round, string name)
        {
            return round.RenameTo(name);
        }

        public bool SetAdvancingPerGroupCountInRound(RoundBase round, int count)
        {
            return round.SetAdvancingPerGroupCount(count);
        }

        public bool SetPlayersPerGroupCountInRound(RoundBase round, int count)
        {
            return round.SetPlayersPerGroupCount(count);
        }

        public bool BetterPlacesMatchBetOnMatch(Guid tournamentId, Guid matchId, string betterName, string playerName)
        {
            Tournament tournament = GetTournamentById(tournamentId);

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

        public bool AddScoreToPlayerInMatch(Guid tournamentId, Guid matchId, Guid playerId, int score)
        {
            Tournament tournament = GetTournamentById(tournamentId);

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

            bool tournamentAlreadyExist = GetTournamentByName(name) != null;

            if (tournamentAlreadyExist)
            {
                // LOG Error: Cannot create tournament with given name, it's already in use.
                return false;
            }

            return true;
        }
    }
}
