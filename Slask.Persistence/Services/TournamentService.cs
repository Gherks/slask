using Slask.Domain;
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

        public Tournament GetTournamentById(Guid id)
        {
            return _slaskContext.Tournaments.FirstOrDefault(tournament => tournament.Id == id);
        }

        public Tournament GetTournamentByName(string name)
        {
            return _slaskContext.Tournaments.FirstOrDefault(tournament => tournament.Name.ToLower() == name.ToLower());
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
                _slaskContext.Add(better);
            }

            return better;
        }

        public BracketRound AddBracketRoundToTournament(Tournament tournament)
        {
            BracketRound round = tournament.AddBracketRound();

            if (round != null)
            {
                _slaskContext.Add(round);
            }

            return round;
        }

        public DualTournamentRound AddDualTournamentRoundToTournament(Tournament tournament)
        {
            DualTournamentRound round = tournament.AddDualTournamentRound();

            if (round != null)
            {
                _slaskContext.Add(round);
            }

            return round;
        }

        public RoundRobinRound AddRoundRobinRoundToTournament(Tournament tournament)
        {
            RoundRobinRound round = tournament.AddRoundRobinRound();

            if (round != null)
            {
                _slaskContext.Add(round);
            }

            return round;
        }

        public PlayerReference RegisterPlayerReference(Tournament tournament, string name)
        {
            PlayerReference playerReference = null;
            RoundBase firstRound = tournament.GetFirstRound();

            if (firstRound != null)
            {
                _slaskContext.RemoveRange(firstRound.Groups);
                playerReference = firstRound.RegisterPlayerReference(name);
                _slaskContext.AddRange(firstRound.Groups);

                if (playerReference != null)
                {
                    _slaskContext.Add(playerReference);
                }
            }

            return playerReference;
        }

        public void RenameRoundInTournament(RoundBase round, string name)
        {
            round.RenameTo(name);
        }

        public void SetPlayersPerGroupCountInRound(RoundBase round, int count)
        {
            _slaskContext.RemoveRange(round.Groups);

            round.SetPlayersPerGroupCount(count);

            _slaskContext.AddRange(round.Groups);
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
