using System;
using System.Collections.Generic;
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

        public Tournament CreateTournament(string v)
        {
            throw new NotImplementedException();
        }

        public Better AddBetter(User user)
        {
            throw new NotImplementedException();
        }

        public Tournament GetTournamentById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Tournament GetTournamentByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<PlayerReference> GetPlayerReferencesByTournamentId(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<PlayerReference> GetPlayerReferencesByTournamentName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Better> GetBettersByTournamentId(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Better> GetBettersByTournamentName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
