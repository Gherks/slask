using System;
using System.Collections.Generic;
using System.Linq;
using Slask.Common;
using Slask.Domain;

namespace Slask.Persistence.Services
{
    public class TournamentService
    {
        private readonly SlaskContext _slaskContext;

        public TournamentService(SlaskContext slaskContext)
        {
            _slaskContext = slaskContext;
        }

        public Tournament CreateTournament(string name)
        {
            Tournament tournament = Create(name);
            _slaskContext.SaveChanges();
            return tournament;
        }

        public Tournament CreateTournamentAsync(string name)
        {
            Tournament tournament = Create(name);
            _slaskContext.SaveChangesAsync();
            return tournament;
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

            if (tournament == null)
            {
                // LOGG
                return null;
            }

            return tournament.GetPlayerReferences();
        }

        public List<PlayerReference> GetPlayerReferencesByTournamentName(string name)
        {
            Tournament tournament = GetTournamentByName(name);

            if (tournament == null)
            {
                // LOGG
                return null;
            }

            return tournament.GetPlayerReferences();
        }

        public List<Better> GetBettersByTournamentId(Guid id)
        {
            Tournament tournament = GetTournamentById(id);

            if (tournament == null)
            {
                // LOGG
                return null;
            }

            return tournament.Betters;
        }

        public List<Better> GetBettersByTournamentName(string name)
        {
            Tournament tournament = GetTournamentByName(name);

            if (tournament == null)
            {
                // LOGG
                return null;
            }

            return tournament.Betters;
        }

        public bool RenameTournament(Guid id, string name)
        {
            name = name.Trim();

            bool nameIsNotEmpty = name != "";
            bool nameIsNotInUse = GetTournamentByName(name) == null;

            if (nameIsNotEmpty && nameIsNotInUse)
            {
                Tournament tournament = GetTournamentById(id);

                if (tournament != null)
                {
                    tournament.ChangeName(name);
                    _slaskContext.SaveChanges();

                    return true;
                }
            }

            return false;
        }

        private Tournament Create(string name)
        {
            bool nameIsEmpty = name == "";
            bool tournamentAlreadyExist = GetTournamentByName(name) != null;

            if (nameIsEmpty || tournamentAlreadyExist)
            {
                // LOGG
                return null;
            }

            Tournament tournament = Tournament.Create(name);
            _slaskContext.Add(tournament);

            return tournament;
        }
    }
}
