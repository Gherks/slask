using System;
using System.Collections.Generic;
using System.Linq;
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
            bool nameIsEmpty = name == "";
            bool tournamentAlreadyExists = GetTournamentByName(name) != null;

            if (nameIsEmpty || tournamentAlreadyExists)
            {
                // LOGG
                return null;
            }

            Tournament tournament = Tournament.Create(name);

            _slaskContext.Add(tournament);
            _slaskContext.SaveChanges();

            return tournament;
        }

        public Tournament GetTournamentById(Guid id)
        {
            return _slaskContext.Tournaments.Where(tournament => tournament.Id == id).FirstOrDefault();
        }

        public Tournament GetTournamentByName(string name)
        {
            return _slaskContext.Tournaments.Where(tournament => tournament.Name == name).FirstOrDefault();
        }

        public List<PlayerReference> GetPlayerReferencesByTournamentId(Guid id)
        {
            Tournament tournament = GetTournamentById(id);

            if(tournament == null)
            {
                // LOGG
                return null;
            }

            return tournament.PlayerReferences;
        }

        public List<PlayerReference> GetPlayerReferencesByTournamentName(string name)
        {
            Tournament tournament = GetTournamentByName(name);

            if (tournament == null)
            {
                // LOGG
                return null;
            }

            return tournament.PlayerReferences;
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
    }
}
